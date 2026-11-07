---
name: ui-ux-audit
description: Use when reviewing an existing mobile game's HUD, menus, navigation, readability, touch input, accessibility, or store and reward screens. Produces a ranked list of UI/UX problems with evidence and fixes.
---

# Game UI/UX Audit

## 1. Inventory

Screenshot every screen and state: HUD in play, pause, results (win and lose), main menu/meta, shop, offers, rewarded-ad prompt, settings, popups, loading, errors/offline. Draw the screen map with tap counts from launch to play.

## 2. Check each screen

**Readability**
- Text ≥ 12 pt equivalent on the smallest supported phone; important numbers bigger.
- Contrast: text and key icons readable over every background they can appear on.
- One clear focal point; the primary action is obvious within 1 second.
- Works in grayscale (colorblind check), so state isn't communicated by colour alone.

**Touch**
- Tap targets ≥ 44×44 pt with spacing; no tiny close buttons.
- Primary actions in the thumb zone; nothing important in the top corners for one-handed play.
- No important UI under the notch, rounded corners, or home indicator (safe area).
- Gestures don't conflict with system gestures (edge swipes).

**Feedback and state**
- Every button gives an immediate pressed state and sound.
- Disabled/locked items explain why and how to unlock.
- Loading longer than ~1 s shows progress.
- Errors and offline states have a message and a way forward.

**Layout**
- Check 16:9, 19.5:9/20:9, and 4:3 tablet. Nothing clipped, stretched or floating oddly.
- Canvas Scaler (uGUI) or Panel Settings (UI Toolkit) configured for the reference resolution, with consistent match settings.

**Flow**
- Taps from launch to playing; from result screen to next round (target: 1).
- Popups at session start: at most one.
- Back button (Android) works on every screen and never quits the game without confirmation.

**Honesty (monetization screens)**
- Visible close buttons; clear prices; rewarded ads say exactly what you get; no misleading "free" labels.

**Accessibility**
- Settings for sound, music, haptics separately; reduce-motion option if the game uses heavy shake/flash.

## 3. Rank

Severity by how many players hit it and how much it blocks them. A problem in the HUD or on the result screen outranks a problem in settings.

## Output

`docs/gamedev/reports/YYYY-MM-DD-ui-audit.md`: a table (screen, issue, severity, evidence screenshot, fix), the top 3, and a short list of what's working. Hand implementation to `game-ui-ux-designer` / `unity-developer`.
