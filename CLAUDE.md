<!-- unity-gamedev-team:start -->
# Unity Game Dev Team

This project uses a team of game-development agents, skills and commands in `.claude/`.
Target: **Unity, mobile first** (Android + iOS). Copy this block into the game project's own `CLAUDE.md`.

## How the team works

- **The producer drives.** `/build-game` hands the project to the `game-producer` agent, which works one stage at a time and calls specialists.
- **One stage at a time.** The project's current stage lives in `docs/gamedev/STAGE.md`. The `stage-ladder` skill defines the stages and what each one requires. Never do work that belongs to a later stage while the current stage's exit criteria are still unmet.
- **Cheap checks often, one gate per stage.** During a stage, run small focused checks (a skill on one area). At the end of a stage, run one combined `game-review`. Before shipping anything to players, run `release-gate`.
- **Evidence over opinion.** Every finding cites something real: a file and line, a profiler capture, a screenshot, a playtest note, a metric. "Feels slow" is a hypothesis until it is measured.
- **Find the cause before changing code.** Reproduce → isolate → fix → verify. No speculative rewrites.

## Project files the team keeps

```
docs/gamedev/
  STAGE.md          current stage, why, exit criteria status
  CONCEPT.md        one-page concept: fantasy, core verb, core loop, audience, monetization model
  DECISIONS.md      dated log of decisions and the reason for each
  TUNING.md         every tunable number, where it lives, current value, why
  reviews/          game-review and release-gate reports (YYYY-MM-DD-<stage>.md)
  reports/          focused skill reports (YYYY-MM-DD-<skill>.md)
  playtests/        raw playtest notes and triage output
```

Create missing files on first use. Never delete reports; supersede them with newer ones.

## Unity conventions

- Never move, rename or delete assets outside the Unity Editor or without moving the `.meta` file with them. Use the `unity-safe-reorganization` skill.
- Gameplay numbers live in ScriptableObjects or remote config, never hard-coded in MonoBehaviours.
- Keep third-party SDKs (ads, IAP, analytics, remote config) behind game-owned interfaces so they can be swapped and mocked in tests.
- Mobile budgets (defaults, override in `TUNING.md`): 60 fps target on mid-tier devices and a stable 30 on low-end; 0 B GC allocations per frame in gameplay; first interactive frame in under 5 s on mid-tier; install size under 150 MB.
- If Unity plugin skills are installed (`unity:*`, `unity-perf:*`), use them for engine-level work and keep these skills for the process around it.

## Who to call

| Problem | Call |
|---|---|
| Build or continue the game | `/build-game` → `game-producer` |
| Whole-game review | `/review-game` → `game-review` skill |
| Feature code, scenes, prefabs | `unity-developer` |
| Mechanics, loops, economy, difficulty | `game-systems-designer` |
| HUD, menus, onboarding, store screens | `game-ui-ux-designer` |
| Flat, weightless, unresponsive feel | `game-feel-designer` |
| Visual direction, "looks AI-made" | `game-art-director` |
| Frame drops, heat, memory, load time | `game-performance-engineer` |
| Bugs, playtests, regressions | `game-qa-analyst` |
| IAP, ads, pricing | `game-monetization-manager` |
| Remote config, flags, analytics, A/B | `game-config-analytics-engineer` |
| Store builds and submission | `mobile-release-engineer` |
<!-- unity-gamedev-team:end -->
