---
name: economy-balance
description: Use when designing or correcting currencies, rewards, costs, upgrades, progression pacing, resource sinks and sources, or inflation in a mobile game. Models the economy with numbers and checks it with simulation.
---

# Game Economy Balance

## 1. Map the economy

For each currency or resource: list every **source** (how players earn it, how much, how often) and every **sink** (what they spend it on, how much). Mark which sources are free, ad-gated or paid. Draw it as a simple flow: sources → currency → sinks → power/progress.

Keep all numbers in one table (in `TUNING.md` or a spreadsheet the code reads from), not scattered in code.

## 2. Model a player's path

Build a simple per-session model (spreadsheet or a small script) for 2–3 player types:
- **Non-payer, average skill**
- **Non-payer, engaged** (plays more sessions, watches rewarded ads)
- **Payer** (one starter pack, occasional packs)

For each, compute currency earned per session and day, what they can afford when, and when they hit the next meaningful upgrade or unlock. Target a meaningful reward every session and a significant unlock every 1–3 days in the early game.

## 3. Check for common problems

- **Inflation**: income grows faster than costs, and currency piles up unused → add sinks or slow income growth.
- **Walls**: costs grow faster than income, and progress stalls → check whether the wall is intentional (a purchase point) and whether it's fair.
- **Dead currency**: a currency with nothing worth buying.
- **Dominant choice**: one upgrade path is always best → rebalance so choices matter.
- **Ad/IAP value**: a rewarded ad should give roughly the value of a short play session; the cheapest IAP should clearly beat watching ads for convenience, not be required.
- **Exploits**: reward loops that generate currency without real play (replaying an easy level, clock manipulation).

## 4. Simulate

Run the model, or a seeded bot simulation (`testable-by-design`), for 30 days of play per player type and chart currency balance and progression. Adjust and re-run.

## Output

`docs/gamedev/reports/YYYY-MM-DD-economy.md`: the source/sink map, the model's key results per player type, problems found, and number changes (old → new, with the remote config key). All economy numbers should end up as remote-config keys before Beta.
