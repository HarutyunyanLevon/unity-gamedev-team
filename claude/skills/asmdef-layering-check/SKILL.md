---
name: asmdef-layering-check
description: Check that a Unity project's assembly definitions obey its layer rule (who may reference whom), and prove the compiler enforces it with a temporary cross-layer probe. Use after adding or changing a package or asmdef, before a stage gate, or when a design card asks for compile-time layering evidence.
---

# Asmdef Layering Check

Assembly definition references are the cheapest architecture enforcement Unity has: if `UI` doesn't reference `Economy`, code in `UI` can't use an `Economy` type. This skill checks the references on paper, then proves the compiler rejects a forbidden one.

## 1. Read the rule

Take the layer table from the project's `CLAUDE.md` (or the architecture doc it points to). Write it down as "assembly family → allowed Gamebox/project references". An assembly family is the runtime assembly plus its `.Editor` and `.Tests` assemblies, which may reference their own runtime assembly.

If the rule is missing or ambiguous, ask; don't infer layers from folder names.

## 2. Static check

For every project-owned `*.asmdef` (`Assets/**`, `Packages/<project prefix>.*/**`):

- Parse `name`, `references`, `includePlatforms`, `defineConstraints`, `precompiledReferences`, `overrideReferences`.
- Flag:
  - **Up-reference:** a lower layer referencing a higher one.
  - **Sideways reference:** a module referencing a sibling in the same layer (for example UI → Economy) unless the rule allows it.
  - **GUID references** (`GUID:...`). They're valid in Unity but not reviewable by name, so ask to switch them to names.
  - **Editor and test hygiene:** Editor assemblies have `includePlatforms: ["Editor"]`. Test assemblies are editor-only (EditMode), reference `UnityEngine.TestRunner` and `UnityEditor.TestRunner`, use `nunit.framework.dll` with `overrideReferences: true`, and have the `UNITY_INCLUDE_TESTS` define constraint.
  - **Empty asmdefs:** a folder with no `.cs` file isn't compiled. That's fine for runtime placeholders, but a test assembly won't show in the Test Runner (see `testable-by-design`).

Output a table: assembly | references | allowed? | notes.

## 3. Compile probe (proves enforcement)

Do this only with the project closed in the Editor, or ask the owner to close it. Then:

1. Add a public empty class to a sibling or higher assembly. Example: `Packages/<pkg-economy>/Runtime/LayeringProbe.cs`.
2. Add a class to the assembly under test that uses it. Example: `Packages/<pkg-ui>/Runtime/LayeringProbe.cs` with a field of that type.
3. Run a batch compile: `Unity.exe -batchmode -quit -projectPath <project> -logFile <scratch>/probe.log` (or `unity:unity-cli` if installed).
4. Expect `error CS0246` or `CS0234` in the assembly under test. Copy the exact line as evidence. A clean compile means the rule isn't enforced, so stop and report it.
5. Delete both probe files and any `.meta` files Unity created. Confirm `git status` shows nothing from the probe, and re-run the tests or compile to confirm the project is clean again. Never commit a probe.

## 4. Report

Put the static table, the probe error line and the clean re-run result in the task's report (for example `docs/design/STATUS.md` evidence), or in `docs/gamedev/reports/YYYY-MM-DD-asmdef-layering.md`.

## Keep it checked

Suggest one EditMode test per test assembly that reads its package's asmdefs and asserts the rule. It's cheap, runs in CI, and keeps the test assembly non-empty.

The test must also prove it can fail. Put the rule in a small method (for example `FindViolation(reference)`), and add a `[TestCase]` test that feeds it one forbidden reference of each kind (a `GUID:` reference and a sibling or higher assembly) and expects both to be rejected. Without that test, a check that never fails, like an always-true condition or a parse that returns no references, passes forever. Branches that real asmdefs never reach, such as GUID rejection, otherwise never run.
