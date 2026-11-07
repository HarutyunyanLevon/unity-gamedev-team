---
name: testable-by-design
description: Use when building or changing game systems so that unit tests, bot playthroughs, soak runs and balance simulations can run fast and headless - separating rules from Unity, making time and randomness controllable, and adding test hooks.
---

# Testable by Design

Games are hard to test when rules live inside MonoBehaviours, depend on real time and unseeded randomness, and can only be checked by a human playing. Fix the structure, and tests, bots and balance sims become cheap.

## Structural rules

1. **Rules in plain C#.** Core logic (board state, combat math, economy, progression, save data) lives in classes with no `UnityEngine` dependency where practical. MonoBehaviours are adapters: they read input, call the rules, and render the result.
2. **Deterministic randomness.** Inject a seeded random source (`System.Random` or a custom PRNG) instead of calling `UnityEngine.Random` from rules. Same seed, same game.
3. **Injected time.** Rules take `deltaTime` or a clock interface as input, never `Time.time` or `DateTime.Now` directly. This enables fast-forward and daily-reward tests.
4. **Commands in, events out.** Player actions are command objects (`Merge(a, b)`, `Cast(at)`); rule outcomes are events. Bots send the same commands players do.
5. **Data from ScriptableObjects or plain data**, loadable in tests without scenes.
6. **SDKs behind interfaces** with fakes (ads always reward, IAP always succeeds or fails on demand, analytics records events to a list).
7. **Save data as a plain serializable model** with a version number and migration functions you can test.

## Test layers

| Layer | Tool | Speed | What it catches |
|---|---|---|---|
| Unit | EditMode tests (Unity Test Framework / NUnit) | ms | Rule bugs, math, edge cases, save migration |
| Simulation | EditMode test running thousands of seeded games with a bot policy | seconds | Balance: win rates, economy inflation, softlocks |
| Integration | PlayMode tests loading a scene | seconds–minutes | Wiring, prefabs, scene flow |
| Soak | Bot playing a dev build on device for 30–60 min | minutes | Leaks, heat, rare crashes |

## Bot policies

Write at least two: **random** (finds crashes and softlocks) and **greedy** (approximates a decent player, for balance). For difficulty, compare win rate by level across policies; a level that greedy can't beat is a spike.

## Adding to an existing project

Don't rewrite. When touching a system, extract its rules into a plain class, cover it with tests, and leave the MonoBehaviour as a thin shell. Log the extracted systems in `DECISIONS.md`.

## Output

Tests in `Assets/Tests/EditMode/` and `Assets/Tests/PlayMode/` with their own asmdefs, plus a short note on how to run them (Test Runner window, or `-runTests -testPlatform EditMode` in batch mode, or `unity:unity-cli` if installed).
