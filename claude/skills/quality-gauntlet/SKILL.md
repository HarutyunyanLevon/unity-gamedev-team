---
name: quality-gauntlet
description: Run an iterative quality push toward an ambitious playable goal ("make the slice feel like a top-100 game") by breaking it into judgeable workstreams, setting a measurable bar for each, and looping build → judge → fix until each passes or is consciously cut.
---

# Game Quality Gauntlet

Use when one review isn't enough: the goal is a big jump in quality across many areas at once.

## 1. Define the goal and the bar

Write the goal as something a player would notice ("the first 3 minutes feel as polished as <reference game>"). Pick 1–3 reference games and capture the same moments in them for comparison.

## 2. Break it into workstreams

Typical streams, each with an owner agent:
- Core verb feel (`game-feel-designer`)
- Visual coherence (`game-art-director`, `non-slop`)
- First 60 seconds (`ftue-review`)
- UI clarity (`ui-ux-audit`)
- Audio
- Performance on low-end (`game-performance-engineer`)
- Stability (`game-qa-analyst`)

## 3. Make each bar judgeable

For each stream, write 3–5 pass criteria that someone else can check without debate, for example:
- "Tap-to-feedback ≤ 1 frame, measured in a 240 fps capture."
- "In grayscale, all interactive items are distinguishable from background."
- "New player reaches first win within 60 s in 4 of 5 unguided tests."
- "No frame over 33 ms in the slice on Redmi 9A over 10 minutes."

Vague criteria ("feels juicy") must be turned into a comparison with the reference capture, judged side by side.

## 4. Loop

For each round:
1. Each stream does its work.
2. A **judge** who didn't do the work (a fresh agent, or the owner) checks the criteria against evidence: captures, measurements, test results.
3. Record pass/fail per criterion in `docs/gamedev/reviews/gauntlet-<goal>.md`.
4. Failed criteria get a fix for the next round. A criterion failing for 3 rounds gets escalated: change the approach, lower the bar with the owner's agreement, or cut it.

Stop when all criteria pass or are consciously cut, then run `game-review`.

## Rules

- The person or agent that did the work never judges it.
- No criterion is marked passed without evidence attached.
- Keep the round count and time visible so the gauntlet doesn't run forever.
