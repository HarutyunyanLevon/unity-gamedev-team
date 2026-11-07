---
name: game-review
description: Use at a stage gate (especially the vertical slice gate) or whenever the owner asks for a review of the whole game. Produces one combined, prioritized review across fun, feel, look, UX, tech health and business, scaled to the current stage.
---

# Game Review

One combined review of the whole game, at its current stage. It replaces many scattered opinions with one ranked list the owner can act on.

## Step 1: Set the frame

1. Run `stage-ladder` to get the stage. The review's depth follows it: stage 1 reviews only fun and the core verb; stage 2 adds feel, look, UX and perf; stage 3+ adds economy, content, tech health; stage 4+ adds business and store.
2. Read `CONCEPT.md`, `STAGE.md`, `DECISIONS.md`, and the last review in `docs/gamedev/reviews/` so you don't re-report what was already decided or fixed.

## Step 2: Gather evidence

Play or inspect the game itself, not just the code. In order of preference:
- A build or Play Mode session (if you can run it), screenshots and short captures.
- Recent playtest notes in `docs/gamedev/playtests/`.
- Recent focused reports in `docs/gamedev/reports/`.
- Code and scene inspection.

If you cannot run the game, say so at the top of the review and mark findings that come only from code as **unverified**.

## Step 3: Review each lens (only those the stage calls for)

| Lens | Key questions | Specialist to consult for depth |
|---|---|---|
| Fun / core loop | Is the core verb satisfying in the first 30 s? Is there a reason to do it again? | `core-loop-review` |
| Feel | Does every input get instant, readable feedback? Weight, anticipation, impact? | `game-feel-designer` |
| Look | One coherent style? Readable at phone size? Anything that reads as generic or AI-made? | `game-art-director`, `non-slop` |
| UX | Can a new player get from launch to the first fun moment without help? Thumb reach, tap targets ≥ 44 pt, safe areas | `ui-ux-audit`, `ftue-review` |
| Progression | Difficulty curve, economy sinks/sources, session pacing | `difficulty-curve`, `economy-balance` |
| Tech health | Frame time, GC, load time, crashes, save robustness, project structure | `unity-profiling`, `unity-project-audit` |
| Business | Monetization fits the fantasy? Ads placed at natural breaks? Analytics answer real questions? | `game-monetization-manager` |

Delegate a lens to its specialist only when that lens looks like the biggest risk. Otherwise do a short pass yourself.

## Step 4: Rank and write

Severity:
- **Blocker** stops the stage gate. The game can't move up until it's fixed.
- **Major** hurts players noticeably. Fix within this stage.
- **Minor** is polish or later-stage work. Log it and move on.

Write `docs/gamedev/reviews/YYYY-MM-DD-<stage>.md`:

```markdown
# Game Review — Stage 2 Vertical Slice — 2026-11-06
Verdict: PASS | PASS WITH MAJORS | FAIL
Ran the game: yes (Editor Play Mode, 3 sessions) | no (code-only, findings unverified)

## Top 3 (do these first)
1. ...

## Findings
| # | Lens | Severity | Finding | Evidence | Suggested fix | Owner |
|---|---|---|---|---|---|---|

## What's working (keep it)
- ...

## Stage decision
Stay at 2 / Promote to 3 / Return to 1, and why.
```

Then update `STAGE.md` exit criteria with the evidence.

## Rules

- At most **3 top items**. A review with 40 equal-weight bullets is not a review.
- Always include "What's working" so fixes don't break the good parts.
- Don't fix things during the review. The review decides; the producer schedules the fixes.
