---
name: non-slop
description: Make a mobile game look hand-crafted and feel alive instead of generic or AI-generated. Use when the game looks like placeholder art, an asset-store mash-up, or obviously AI-generated, or before a slice review or store screenshots.
---

# Non-Slop Pass

"Slop" is what players instantly recognize as low-effort: mismatched styles, generic fonts, AI artefacts, everything static, no personality. This pass finds and fixes it.

## 1. Spot the tells

Look at screenshots at actual phone size, then zoomed in.

**Art**
- Styles that don't match: different line weights, pixel densities, lighting directions, or levels of detail side by side.
- AI artefacts: melted details, inconsistent hands/faces, text-like garbage, over-smooth gradients, random extra elements, symmetry glitches, a different "camera" per asset.
- Palette drift: each asset with its own colours instead of one palette.
- Everything the same saturation and brightness, so nothing stands out.
- Default Unity look: default skybox, default fonts (LiberationSans/Arial), default UI sprites, flat grey materials, the default particle texture.

**Life**
- Nothing moves when the player is idle.
- No transitions: screens snap in and out.
- No sound on interactions, or one generic click everywhere.
- Rewards appear as instant number changes.

**Personality**
- Generic names, generic copy ("Level Complete!"), no voice.
- No hand-made details: no small surprises, idle animations, or environmental storytelling.

## 2. Fix, in order of visibility

1. **One palette and one rendering rule set** (from `ART_DIRECTION.md`; create it with `game-art-director` if missing). Recolour and correct assets to it.
2. **Typography**: a distinctive, readable display font plus a clean body font; consistent sizes.
3. **Clean AI assets**: repaint artefacts, unify outlines and lighting, simplify over-detail to match neighbours. Treat generated images as sketches, not finished art.
4. **Idle life**: subtle breathing/bobbing on characters and interactive items, ambient particles (dust, leaves, bubbles), background parallax, blinking.
5. **Transitions**: eased screen and popup transitions (150–300 ms), buttons that squash on press.
6. **Sound**: distinct, layered sounds for the top 5 actions, ambient bed, music with a loop that doesn't grate.
7. **Reward ceremony**: fly-to-counter, count-up, small burst, escalating with value (with `game-feel-designer`).
8. **Voice**: write copy in the game's personality; rename generic things.

## 3. Verify

Before/after screenshots and short captures of the same moments. Ask: would a player think a person made this on purpose? Check performance didn't regress (`unity-profiling`).

## Output

`docs/gamedev/reports/YYYY-MM-DD-non-slop.md`: tells found (with screenshots), fixes made, and the remaining list ranked by visibility.
