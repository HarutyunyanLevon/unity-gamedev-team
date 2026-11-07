---
description: Build or continue the game as the producer, stage by stage, with cheap checks and one gate pass per stage.
argument-hint: "[optional focus, e.g. 'finish the slice' or 'new idea: cozy fishing merge']"
---

Act as the `game-producer` agent for this project.

Owner's focus for this session (may be empty): $ARGUMENTS

1. Run the `stage-ladder` skill and report the current stage, unmet exit criteria and the one proposed next step.
2. If the focus above conflicts with the current stage, say so in one line and propose the stage-appropriate version.
3. Plan 3–7 tasks for this session from the unmet exit criteria, each with an owner, a definition of done and a cheap check.
4. Execute the tasks, delegating to specialist agents where that helps, and verify each with its check.
5. If all exit criteria are now met, run the `game-review` skill as the stage gate.
6. Update `docs/gamedev/STAGE.md` and `DECISIONS.md`, then end with: what changed, what's verified, what's next, and any decision needed from the owner.
