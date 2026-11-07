---
name: stage-ladder
description: Use at the start of any game build, review, audit, measurement or release request, and whenever the amount of work to do depends on how far along the game is. Defines the stages, their exit criteria, and which checks belong to each.
---

# Stage Ladder

The stage decides how much rigor a request deserves. Polishing particle effects in a prototype is waste; skipping a regression pass before release is negligence. Read the current stage first, then scale the work to it.

## Step 1: Read or establish the stage

1. Open `docs/gamedev/STAGE.md`. If it exists, use the stage it names and re-check the exit criteria it lists.
2. If it doesn't exist, infer the stage from evidence (below), write `STAGE.md`, and say which stage you inferred and why in one line.

Evidence for inference: is there a `CONCEPT.md`? Is the core verb playable? How many levels/content units exist? Are there store builds, SDKs, analytics? Any release tags or store listings?

## The stages

| # | Stage | Question it answers | Exit criteria |
|---|---|---|---|
| 0 | **Concept** | Is this worth building? | `CONCEPT.md` has fantasy, core verb, core loop (30 s / 5 min / session / meta), target audience, reference games, monetization model, and the single riskiest assumption. |
| 1 | **Prototype** | Is the core verb fun? | Core verb playable with placeholder art in one scene. At least 3 people played it unguided and at least 2 wanted to keep playing. Riskiest assumption tested. |
| 2 | **Vertical Slice** | Is this the game, at final quality, in a small piece? | One full session loop at target art, feel, audio and UI quality. FTUE draft. Hits perf budgets on the low-end reference device. `game-review` passes with no blockers. |
| 3 | **Content / Alpha** | Can we make enough of it? | Content pipeline proven (levels/items authored from data). Meta progression and economy in place. Save/load robust. All core screens exist. |
| 4 | **Beta / Soft Launch** | Do real players stay and pay? | Analytics, remote config, crash reporting live. Monetization integrated. Store builds pass `store-readiness`. Soft-launch KPIs defined (D1/D7 retention, session length, conversion). |
| 5 | **Release** | Is this candidate safe to ship? | `release-gate` passes. Regression pass done. Store assets final. |
| 6 | **Live** | Is it getting better? | KPI dashboard, update cadence, retrospective after each update. |

## What to run at each stage

| Stage | Cheap checks (run often) | Gate (once, at exit) |
|---|---|---|
| 0 | none | `core-loop-review` on paper |
| 1 | `unity-debugging` as needed, quick feel pass | short `game-review` (fun + core verb only) |
| 2 | `game-feel-designer`, `non-slop`, `ui-ux-audit`, `unity-profiling`, `mobile-device-check` | full `game-review` |
| 3 | `economy-balance`, `difficulty-curve`, `ftue-review`, `testable-by-design`, `unity-dependency-audit` | full `game-review` |
| 4 | `ads-mediation-review`, `store-readiness`, `playtest-triage`, analytics validation | `game-review` + `store-readiness` |
| 5 | `regression-check` | `release-gate` |
| 6 | `playtest-triage` on live feedback, `retrospective` | `release-gate` per update |

## Rules

- **Don't work ahead.** If asked for later-stage work (for example monetization in stage 1), say which stage it belongs to and offer the current stage's next step. Do it anyway only if the owner insists, and log that in `DECISIONS.md`.
- **Don't skip gates.** Moving up a stage requires its exit criteria to be met with evidence, or an explicit owner waiver logged in `DECISIONS.md`.
- **Going down is allowed.** If a slice review shows the core verb isn't fun, the stage returns to Prototype. Say so plainly.

## STAGE.md format

```markdown
# Stage: 2 — Vertical Slice
Set: 2026-11-06 (inferred | set by owner | promoted after review YYYY-MM-DD)

## Why
One or two lines of evidence.

## Exit criteria
- [x] One full session loop at target quality — evidence: reviews/2026-11-01-slice.md
- [ ] Perf budget on low-end device — current: 41 fps on Redmi 9A, target 60/30
- [ ] FTUE draft

## Next step
The single most valuable thing to do next.
```
