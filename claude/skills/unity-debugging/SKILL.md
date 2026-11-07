---
name: unity-debugging
description: Reproduce, isolate, fix and verify one focused Unity functional failure (exception, wrong behaviour, missing reference, platform-only bug) using Console, Editor and Player logs, device logs, and minimal code changes. Use for any bug before changing code.
---

# Unity Debugging

One bug at a time. Reproduce → isolate → fix → verify. No fix without a reproduction, unless you clearly state the fix is speculative.

## 1. Pin down the failure

Write down: the exact symptom, build/version, platform (Editor / Android / iOS), device, steps, frequency, and the first error in the log (not the cascade after it).

Where logs live:
- **Editor**: Console window; `Editor.log` (Windows `%LOCALAPPDATA%\Unity\Editor\Editor.log`, macOS `~/Library/Logs/Unity/Editor.log`).
- **Android**: `adb logcat -s Unity` (plus `AndroidRuntime` and `CRASH` for native crashes).
- **iOS**: Xcode console or device logs.
- **Standalone player**: `Player.log` in the persistent data path's log folder.

## 2. Reproduce

Find the shortest reliable steps. If it only happens on device, note the differences: IL2CPP stripping, platform `#if` code, file paths, permissions, aspect ratio, performance-dependent timing.

## 3. Isolate

Common Unity causes to rule in or out:
- **NullReferenceException / MissingReferenceException**: unassigned serialized field, destroyed object still referenced, execution-order dependency in Awake/Start, scene object not yet loaded.
- **Works in Editor, fails on device**: code stripping (add `link.xml` or `[Preserve]`), reflection or JSON on stripped types, case-sensitive paths, `Application.dataPath` assumptions, missing Android permissions.
- **Timing**: frame-rate-dependent logic (missing `Time.deltaTime`), coroutines stopped by disabling the GameObject, async/await continuing after scene unload.
- **Physics**: wrong layer matrix, missing Rigidbody for triggers, `FixedUpdate` vs `Update` input.
- **Serialization**: renamed field without `[FormerlySerializedAs]`, prefab override lost, ScriptableObject edited at runtime (changes persist in the Editor, not in builds).
- **UI**: raycast blocked by an invisible Graphic, missing EventSystem, Canvas scaler settings.

Use bisection: disable half the suspects, check, repeat. Add temporary logs with a unique prefix, and remove them afterwards.

## 4. Fix

Change the minimum to fix the cause, not the symptom. If the cause is a design flaw that needs a bigger change, fix the immediate bug and log the bigger issue.

## 5. Verify

- The original repro no longer fails, on the platform where it failed.
- Nearby cases still work.
- Add a regression test if the logic is testable (`testable-by-design`).
- Remove temporary logs.

## Output

A short report: symptom, root cause (with evidence), fix (files changed), verification done, and any remaining risk. Save it as `docs/gamedev/reports/YYYY-MM-DD-bug-<slug>.md` if it was non-trivial. For collisions and triggers specifically, `unity:physics-3d-collision` and `unity:create-2d-physics` help if installed.
