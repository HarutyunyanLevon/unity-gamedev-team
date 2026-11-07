---
description: Show the project's stage and why, or set it by hand (rarely needed).
argument-hint: "[empty to show | 0-6 or stage name to set] [reason]"
---

Arguments: $ARGUMENTS

- **No arguments:** run the `stage-ladder` skill and show the current stage, the evidence for it, each exit criterion with its status, and the next step. Change nothing except creating `docs/gamedev/STAGE.md` if it's missing.
- **A stage given:** set `docs/gamedev/STAGE.md` to that stage, marked "set by owner", and append a dated entry to `docs/gamedev/DECISIONS.md` with the reason given (ask for one if none was given). If moving up past unmet exit criteria, list them as waived.
