---
name: difficulty-curve
description: Use when players hit difficulty spikes, boredom, repeated failure, unfair-feeling losses, or inconsistent challenge across levels, or when designing a level progression. Measures and reshapes the challenge curve.
---

# Difficulty Curve Analysis

## 1. Get the curve

Collect per level (or per wave/stage), from analytics, bot simulation (`testable-by-design`) or playtests:
- Win rate on first attempt, and average attempts to clear
- Average duration
- Quit rate at that level (players whose last session ended there)
- Boosters/continues used

Plot attempts-to-clear per level. Without data, estimate with a bot simulation and mark it as an estimate.

## 2. Read the shape

A healthy mobile curve is a **sawtooth that trends upward**: a few levels build up, a harder "boss" level, then a relief level, then the next teeth start a little higher. Look for:
- **Spikes**: attempts jump more than ~2× the previous teeth's peak, or quit rate is high.
- **Flat stretches**: many levels in a row with ~100% first-try win rate → boredom.
- **Early walls**: anything hard in the first ~10 levels loses players before they're invested.
- **Unfair losses**: fails caused by randomness the player couldn't influence, or by unclear rules. Check playtest notes for "that's not fair" and "what happened?".

## 3. Find the cause of each problem level

Difficulty comes from: required precision/speed, number of simultaneous threats, resources available (moves, time, HP), randomness, and knowledge needed (new mechanic). Name which one makes the level hard. New mechanic + harder numbers in the same level is a classic spike: separate them.

## 4. Adjust

- Tune numbers first (moves, timers, enemy counts), keeping them in data or remote config.
- Introduce each new mechanic in an easy level before combining it.
- Add relief levels after bosses.
- Consider dynamic difficulty only with care: subtle help after repeated fails (better random draws, a free booster) is common; make sure it can't be exploited and doesn't feel patronizing.

## Output

`docs/gamedev/reports/YYYY-MM-DD-difficulty.md`: the curve (table or chart), problem levels with cause and evidence, specific number changes (old → new), and the expected effect to check in the next playtest or bot run.
