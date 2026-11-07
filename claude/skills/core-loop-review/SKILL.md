---
name: core-loop-review
description: Use when a game feels repetitive, aimless, unrewarding or unclear, can't hold interest, or when validating a concept on paper. Breaks the game into its nested loops and finds where motivation breaks.
---

# Core Loop Review

## 1. Write the loops down

| Loop | Time scale | Question |
|---|---|---|
| **Moment** | 1–10 s | What does the player do, and what feedback do they get? (the core verb) |
| **Round** | 30 s – 5 min | What's a complete unit of play, with a goal, tension and resolution? |
| **Session** | 5–20 min | Why play several rounds? What do rounds feed into? |
| **Meta** | days–weeks | What grows, unlocks or changes over many sessions? Why come back tomorrow? |

For each loop, write: action → challenge → reward → how the reward feeds the next loop. If a reward feeds nothing, it's noise.

## 2. Test each loop

- **Moment**: Is the core verb satisfying with no rewards at all? (If you removed score and progression, would it still be fun to do for 30 seconds?) If not, that's a feel or verb problem: bring in `game-feel-designer`.
- **Round**: Is there a meaningful decision, or just execution? Is there uncertainty about the outcome? Can the player tell why they won or lost?
- **Session**: Does each round change what the next round looks like (new options, new goals, escalation)? Is there a natural stopping point and a reason not to stop yet?
- **Meta**: Does progress change how you play, or only the numbers? Is there a visible next goal at all times? Is there variety (new mechanics, content, modes) on a schedule?

## 3. Diagnose with evidence

Map symptoms to loops:
- "Got bored after 5 minutes" → session loop has no escalation or variety.
- "Didn't get it" → moment or round clarity; see `ftue-review`.
- "Didn't come back" → meta has no pull; check D1 hooks (daily goals, unfinished business, a visible next unlock).
- "Felt grindy" → reward pacing; see `economy-balance`.

Use playtests (`playtest-triage`) and analytics where they exist; mark assumptions as such.

## 4. Recommend

At most 3 changes, each tied to a specific loop and a symptom, with how to test it (playtest question or metric). Prefer changes to the inner loops first: a stronger moment improves everything above it.

## Output

`docs/gamedev/reports/YYYY-MM-DD-core-loop.md` with the loop table, findings per loop, and recommendations. At Concept stage, write the loops into `CONCEPT.md` instead.
