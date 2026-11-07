---
name: unity-project-audit
description: Audit an existing Unity project and map its architecture, scenes, prefabs, packages, settings, Addressables, build targets and risks. Use when joining or inheriting a project, before a large change, or when nobody is sure how the project fits together.
---

# Unity Project Audit

Read-only. Produces a map of the project and a ranked risk list. Change nothing.

## 1. Identity

- `ProjectSettings/ProjectVersion.txt`: Unity version. Note whether it's an LTS release.
- Render pipeline: Built-in, URP or HDRP (check `GraphicsSettings.asset` and `Packages/manifest.json`).
- `Packages/manifest.json`: list packages and versions; flag preview/experimental and git-URL packages.
- Build targets, scripting backend, API compatibility, target/min API level (Android), min iOS version: `ProjectSettings/ProjectSettings.asset`.
- Third-party SDKs in `Assets/` (ads, analytics, IAP, Firebase, Facebook, etc.) and how they're integrated (External Dependency Manager, manual).

## 2. Structure

- Top-level `Assets/` layout. Is there a clear convention (by feature or by type)? Where do third-party assets live?
- Assembly definitions: which exist, dependency direction, any cycles. No asmdefs at all means slow compiles and no boundaries; note it.
- Scenes in Build Settings, in order. Which scene boots? How are scenes loaded (single, additive, Addressables)?
- `Resources/` folders and their size (everything in them ships and loads into an index at startup).
- Addressables: groups, packing mode, remote vs local.

## 3. Code architecture

- Entry point and bootstrapping: how services are created (singletons, DI container, ScriptableObject services, static).
- Game state flow: who decides menu → game → result.
- Events/messaging pattern.
- Data: where tunables live (ScriptableObjects, JSON, hard-coded).
- Save system: format, location, versioning/migration.
- SDK boundaries: are SDKs called directly from gameplay code?
- Tests: EditMode/PlayMode test assemblies, how many, do they pass?

## 4. Health signals

- Compiler warnings count; obsolete API usage.
- Missing scripts and missing references (search scenes/prefabs for `m_Script: {fileID: 0}` and `guid: 00000000000000000000000000000000`).
- Large assets: textures > 2048, uncompressed audio, huge meshes.
- Per-frame allocation hotspots visible in code (LINQ in Update, string concatenation, `FindObjectOfType` at runtime).
- Version control hygiene: `.gitignore` covers `Library/`, `Temp/`, `Logs/`, `UserSettings/`; meta files committed; LFS for binaries.

## Output

`docs/gamedev/reports/YYYY-MM-DD-project-audit.md`:

```markdown
# Project Audit — <project> — <date>
## Snapshot
Unity version · pipeline · platforms · key packages and SDKs
## Architecture map
Boot flow, scenes, services, data, saves (short diagram)
## Risks (ranked)
| # | Risk | Evidence | Impact | Suggested action | Effort |
## Conventions observed (follow these)
## Unknowns (couldn't determine from files)
```

If Unity plugin skills are available, `unity:generate-editor-search-query` and `unity:unity-cli` can answer editor-side questions faster.
