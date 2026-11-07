---
name: playtest-triage
description: Use when turning playtest notes, recordings, telemetry, surveys, store reviews, bug reports or conflicting player opinions into a prioritized, evidence-backed list of problems and changes.
---

# Playtest Feedback Triage

## 1. Collect raw material

Put everything into `docs/gamedev/playtests/YYYY-MM-DD-<source>.md`: who played (new or returning, genre experience), device, session length, observer notes, quotes, survey answers, relevant metrics. Keep raw notes unedited.

## 2. Separate observations from opinions

- **Observation**: what the player did ("tapped the shop icon 3 times, nothing happened", "quit at level 4 after 3 fails").
- **Feeling**: what they said they felt ("frustrated", "bored").
- **Suggestion**: what they proposed ("add a skip button").

Behaviour beats opinion. Suggestions point to a problem but are rarely the right fix; find the problem underneath.

## 3. Cluster into problems

Group observations into problem statements written from the player's side: "Players don't notice the merge target", not "make the target glow". For each problem record:
- How many players hit it (out of how many)
- How early in the game it happens
- Impact: confusion, frustration, quitting, lost purchase, crash
- Evidence: quotes, timestamps, metrics

## 4. Prioritize

Score = frequency × impact × earliness. Problems in the first session and the core loop come first. Note conflicts (half loved it, half hated it) and look for a segment explanation (new vs experienced players).

## 5. Route

Each problem gets an owner: `game-ui-ux-designer` (can't find/understand), `game-feel-designer` (unsatisfying), `game-systems-designer` (too hard, boring, grindy), `game-qa-analyst` (bugs), `game-performance-engineer` (lag, heat).

## Output

`docs/gamedev/playtests/YYYY-MM-DD-triage.md`:

| # | Problem (player's view) | Players hit | When | Impact | Evidence | Owner | Proposed test |
|---|---|---|---|---|---|---|---|

End with the top 3 and the questions the next playtest should answer.
