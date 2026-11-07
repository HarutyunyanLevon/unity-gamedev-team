---
name: ftue-review
description: Use when new players are confused, abandon early, skip or fail the tutorial, or don't reach the first satisfying moment quickly. Reviews the first-time user experience from install to the first return visit.
---

# First-Time User Experience Review

The first 60 seconds decide most mobile retention. The goal: get the player to the **first fun moment** fast, then teach only what's needed, just in time.

## 1. Walk it as a stranger

Fresh install (clear data). Record or narrate every step from tapping the icon:
- Time to first interactive frame
- Every screen, popup, permission prompt and consent dialog before play
- Time and taps to the first moment of actual play
- Time to the **first win / first reward / first "aha"**
- Every point where the player must read text to continue

Targets for casual mobile: play within 10–15 s of launch; first win within 60 s; no more than one sentence of text at a time; no account, store or ad before the first fun moment.

## 2. Check teaching

For each mechanic the player needs:
- Is it taught **by doing**, at the moment it's needed, with one thing at a time?
- Is the player able to fail safely while learning?
- Is the instruction short and visual (a hand pointing, a highlight), not a wall of text?
- Is there a check that the player actually got it before the next thing is introduced?
- Can returning or experienced players skip it?

## 3. Check the first session's end and the second visit

- Does the first session end with a reason to come back (an unlock teased, a daily reward, something unfinished)?
- On the second launch, is the player dropped back into play quickly, without re-teaching?
- Notifications permission: asked after the player has a reason to want it, not on first launch.

## 4. Use data if it exists

The `tutorial_step` funnel from analytics: find the step with the largest drop. Compare first-session length and D1 retention between players who finished and didn't finish the tutorial.

## Output

`docs/gamedev/reports/YYYY-MM-DD-ftue.md`: the timeline (step, seconds, taps, friction), the biggest 3 drop risks with evidence, and fixes. Include a proposed first-60-seconds script: exactly what the player sees and does, second by second.
