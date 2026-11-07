---
name: regression-check
description: Plan and run a risk-based regression pass on a candidate mobile build across gameplay, saves, UI, input, audio, performance, monetization and platform behaviour. Use before any release-gate, and scoped down for hotfixes.
---

# Game Release Regression Check

## 1. Scope by risk

List what changed since the last released build (commits, tickets, SDK updates, Unity version, settings). For each change, list what it could break, including indirect effects (a shared prefab, a save field, an SDK update touching consent).

Test effort goes: **changed areas** (deep) → **areas touching changed code** (medium) → **everything else** (smoke).

## 2. Smoke suite (every build, ~20–30 min)

- Fresh install → tutorial → first level → first reward
- Update install over the previous live version, with an existing save → progress intact
- Core loop: play 3 rounds including a win and a loss
- Meta: open every main screen and come back
- Purchase one test IAP; restore purchases
- Watch one rewarded ad; check the reward arrives once
- Background and resume mid-round; lock screen; incoming notification
- Kill the app mid-round, relaunch → state sensible
- Offline launch and offline play (if supported)
- Settings: sound, music, haptics, language toggles persist

## 3. Deep tests for changed areas

Write explicit test cases (steps, expected result) for each changed area and its edge cases. Re-run any test cases from bugs fixed in this release (the original repro) to confirm they stay fixed.

## 4. Device matrix

Minimum: low-end Android, mid/high Android, oldest supported iPhone, a newer iPhone, one tablet. Include one device with a notch/punch-hole and one with a small screen.

## 5. Automated checks

Run EditMode and PlayMode tests, any bot playthrough/soak (`testable-by-design`), and the build-size check. Compare performance against the last release's numbers on the reference device.

## Output

`docs/gamedev/reviews/YYYY-MM-DD-regression-<version>.md`:

| Area | Test | Device | Result | Bug ID / note |
|---|---|---|---|---|

Then a summary: pass/fail counts, open bugs by severity, untested areas (say so explicitly), and a recommendation for `release-gate`.
