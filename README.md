# unity-gamedev-team

A team of Claude Code agents, skills and commands for building **Unity mobile games** (Android + iOS), from concept to live ops.

**11 agents · 25 skills · 3 commands**, organized around a stage pipeline: one stage at a time, cheap checks often, one gate per stage.

## Install into a game project

The team lives in `claude/` in this repo. The installer copies it into your game's `.claude/` and adds the `CLAUDE.md` block.

```powershell
# Windows
.\install.ps1 -Project "D:\Projects\MyGame"
```
```bash
# macOS / Linux / Git Bash
./install.sh /path/to/MyGame
```

Re-running is safe: files you edited in the game project are kept (pass `-Force` / `--force` to overwrite). Then open Claude Code in the game project and run `/stage` or `/build-game`.

Manual install: copy `claude/` to `<project>/.claude/` and paste `CLAUDE.md` into the project's `CLAUDE.md`.

The team creates and maintains `docs/gamedev/` in your project (stage, concept, decisions, tuning, reviews, reports).

Optional: if the Unity plugin skills (`unity:*`, `unity-perf:*`) are installed, the team delegates engine-level work to them.

## Commands

| Command | What it does |
|---|---|
| `/build-game [focus]` | The producer runs the current stage: plans, delegates, verifies, and runs the gate when ready |
| `/review-game [lens]` | One combined review of the whole game at its current stage |
| `/stage [n] [reason]` | Show the stage and its exit criteria, or set it by hand |

## Stages

0 Concept → 1 Prototype → 2 Vertical Slice → 3 Content/Alpha → 4 Beta/Soft Launch → 5 Release → 6 Live. See `claude/skills/stage-ladder/SKILL.md`.

## Agents

| Agent | Use for |
|---|---|
| `game-producer` | Driving the build end to end, planning, delegating, gating |
| `unity-developer` | Gameplay code, scenes, prefabs, editor tooling |
| `game-systems-designer` | Mechanics, loops, progression, economy, difficulty |
| `game-ui-ux-designer` | HUD, menus, onboarding, store and reward screens |
| `game-feel-designer` | Input response, juice, impact, audio and haptic feedback |
| `game-art-director` | Visual direction and consistency |
| `game-performance-engineer` | Frame rate, memory, heat, load time, build size |
| `game-qa-analyst` | Bugs, exploratory testing, playtests, regressions |
| `game-monetization-manager` | IAP catalog, ads plan, offers, pricing |
| `game-config-analytics-engineer` | Remote config, flags, analytics, A/B tests, crash reporting |
| `mobile-release-engineer` | Build pipeline, signing, store tracks, rollouts, hotfixes |

## Skills

| Area | Skills |
|---|---|
| Pipeline | `stage-ladder`, `game-review`, `release-gate` |
| Unity engineering | `unity-project-audit`, `unity-safe-reorganization`, `unity-dependency-audit`, `unity-debugging`, `unity-profiling`, `testable-by-design` |
| Design and feel | `core-loop-review`, `ftue-review`, `difficulty-curve`, `economy-balance`, `ui-ux-audit`, `non-slop`, `tuning-workbench` |
| Quality | `regression-check`, `playtest-triage`, `retrospective`, `quality-gauntlet` |
| Mobile | `store-readiness`, `mobile-device-check`, `ads-mediation-review`, `store-art-aso`, `short-form-video` |

## Notes

- Store requirements change often. Skills that touch store policy tell the agent to check current official docs instead of trusting fixed numbers.
- Original content. The roster was inspired by the publicly listed roles on gameskit.ai; no files from that product were used.
