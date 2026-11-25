---
name: settings-drift-check
description: Before committing in a Unity project, list every uncommitted change to ProjectSettings/, Packages/manifest.json and Packages/packages-lock.json that the current task did not make, trace each to its cause (a package, an Editor action, the owner), and say whether to commit, revert or ask. Use before any commit, when git status shows settings you didn't touch, or after adding or removing a package.
---

# Settings Drift Check

Unity and its packages write to `ProjectSettings/` and `Packages/` on their own. Those edits slip into unrelated commits or sit uncommitted for weeks, and nobody knows whether they were on purpose. This check explains each one before it is committed.

## 1. List the drift

```
git status --short ProjectSettings Packages/manifest.json Packages/packages-lock.json
git diff ProjectSettings Packages/manifest.json Packages/packages-lock.json
```

Drop the lines the current task changed on purpose. Everything left is drift.

## 2. Trace each change

| Change | Usual cause | How to confirm |
|---|---|---|
| `scriptingDefineSymbols` gains or loses a symbol | A package adds its define on import (for example `APP_UI_EDITOR_ONLY` from App UI, `SENTIS_*` from old Sentis) | Search `Packages/packages-lock.json` for the package that owns the symbol, including dependencies (App UI comes in through `com.unity.ai.inference`) |
| `VersionControlSettings` `m_Mode` is not `Visible Meta Files` in a git repo | The Unity Version Control plugin (`com.unity.collab-proxy`) switched it | Check whether the manifest has `com.unity.collab-proxy` |
| `packages-lock.json` changes with no manifest change | The Editor resolved packages again (version bump, removed orphan dependencies) | The diff only touches `depth` > 0 entries or `url`/`hash` fields |
| Settings you don't recognize, such as quality, graphics or player values | The owner changed them in the Editor | Nothing to confirm: ask |

## 3. Decide

- **Commit** the change when a package in the manifest caused it and the change matches what that package needs. Put it in its own commit and name the cause in the message.
- **Revert** the change when its cause is gone or wrong for the project (for example the Unity Version Control mode in a git repo). Remove the cause too, or the change comes back. Ask the owner before reverting a change they may have made.
- **Ask** when the cause is unknown or the owner probably made the change. Never revert or overwrite it silently.

## 4. Report

Write one line per change: file, change, cause, decision. Put it in the task report (for example `docs/design/STATUS.md` notes) or in `QUESTIONS.md` when it needs an answer.
