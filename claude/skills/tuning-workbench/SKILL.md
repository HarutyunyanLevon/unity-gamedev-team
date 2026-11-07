---
name: tuning-workbench
description: Build a game-specific, in-game or in-editor tuning panel that exposes only the meaningful numbers (feel, camera, difficulty, economy, effects) so the owner can tune live on device and save the results. Use when tuning by editing code or assets is too slow.
---

# Game Tuning Workbench

## What to expose

Only numbers that change how the game feels or plays, grouped by purpose, typically 10–30:
- **Feel**: move/drag speeds, acceleration, hit-stop, shake strength, tween durations.
- **Camera**: zoom, follow damping, offsets.
- **Difficulty**: spawn rates, timers, enemy stats, move counts.
- **Economy**: rewards, costs, multipliers.
- **Effects and audio**: particle counts, volume levels, haptics strength.

Pick them from `TUNING.md`. Don't expose everything: a panel with 200 sliders is unusable.

## Design

1. **Single source of truth**: tunables live in ScriptableObject assets (one per group). Gameplay reads from them every time (or subscribes to a change event), so changes apply live.
2. **Runtime panel** (works on device, in development builds only):
   - Opened by a hidden gesture (for example a 3-finger tap or a long press in a corner), compiled out of release builds with `#if DEVELOPMENT_BUILD || UNITY_EDITOR`.
   - Slider + number field per value, with the min/max range from metadata; a reset-to-default button per value and group.
   - Presets: save and load named presets as JSON in `Application.persistentDataPath`; export the current values as text so they can be copied back into the assets or remote config.
3. **Editor side**: an editor window or simple inspectors to apply an exported preset back onto the ScriptableObjects.
4. **Remote config bridge** (Beta onward): optionally show which values are overridden by remote config.

## Implementation sketch

```csharp
[CreateAssetMenu(menuName = "Tuning/Feel")]
public class FeelTuning : ScriptableObject
{
    [Range(0f, 0.2f)] public float hitStop = 0.05f;
    [Range(0f, 1f)] public float shakeStrength = 0.3f;
    [Range(0.05f, 0.5f)] public float popDuration = 0.18f;
}
```

The runtime panel can reflect over `[Range]` fields to build sliders automatically, so adding a tunable is one line. Use UI Toolkit or IMGUI for a panel that doesn't need art (`unity:ui-uitk` or `unity:ui-imgui` if installed).

## Rules

- Never ship the panel in release builds; verify with `release-gate`.
- Changes made on device must be exportable; otherwise tuning sessions are lost.
- After a tuning session, update `TUNING.md` and log the reason for big changes in `DECISIONS.md`.
