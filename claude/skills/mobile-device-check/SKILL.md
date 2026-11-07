---
name: mobile-device-check
description: Check a Unity mobile game across real device conditions - low-end Android performance, heat and battery, memory pressure, screen sizes, notches and safe areas, aspect ratios, OS interruptions and build size. Use from the vertical slice onward and before releases.
---

# Mobile Device and Performance Check

## Device tiers (define in TUNING.md)

| Tier | Example class | Target |
|---|---|---|
| Low | 2–3 GB RAM Android, older Mali/Adreno GPU | stable 30 fps, reduced effects |
| Mid | 4–6 GB RAM, recent mid-range SoC | 60 fps |
| High | flagship Android, recent iPhone | 60 fps (or 120 on ProMotion if supported) |

Pick one real reference device per tier. If the game sets quality per tier, check the detection logic (by GPU, RAM via `SystemInfo`) actually picks the right tier on each.

## 1. Performance and heat

- Frame rate in the heaviest scene, then again after 20 minutes of continuous play (thermal throttling shows up late).
- `Application.targetFrameRate` set deliberately; on Android, check whether it's clamped by the display.
- Battery drain per 10 minutes vs a reference game, if it's a concern.
- Menus and idle screens: reduce frame rate (for example 30) to save battery.
- For details on any problem, use `unity-profiling`.

## 2. Memory

- Peak memory in the heaviest scene on the low tier; stay well under the point where the OS kills apps (low-end devices may kill above ~1 GB).
- Background the app, open a heavy app (camera), return: does the game resume or restart, and is state kept?
- Look for growth over a long session (leaks): compare Memory Profiler snapshots.

## 3. Screens

- Aspect ratios: 16:9, 19.5:9/20:9, 21:9 (tall), 4:3 tablet. Nothing important clipped; backgrounds fill.
- Safe areas: notches, punch-holes, rounded corners, home indicator. UI uses `Screen.safeArea`.
- Small phones: text and tap targets still readable and usable.
- Orientation locked as intended; rotation doesn't break UI.

## 4. Interruptions and OS behaviour

Incoming call, notifications, alarm, lock screen, switching apps, split-screen/multi-window (Android), low battery mode, Do Not Disturb, headphones plugged/unplugged (audio route), system font size changes, dark mode, airplane mode mid-session, app update while a save is open.

## 5. Build

- Install size and download size within budget; list the biggest contributors (Build Report, or the Editor log after a build).
- Texture compression ASTC; audio compressed; unused packages removed.
- IL2CPP stripping level tested.

## Output

`docs/gamedev/reports/YYYY-MM-DD-device-check.md`: device list with OS, a results table (check, device, result, evidence), the problems ranked, and recommended per-tier settings.
