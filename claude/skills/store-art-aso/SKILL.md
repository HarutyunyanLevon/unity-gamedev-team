---
name: store-art-aso
description: Create store listing assets (icon, screenshots, feature graphic, preview video plan) from the game's own art and real gameplay, and write store listing copy and keywords (ASO) for Google Play and the App Store.
---

# Store Art and ASO

Store art must show the real game. Misleading screenshots get rejected and earn bad reviews.

## 1. Icon

- One clear subject (character, key object or core-mechanic symbol), large, readable at 48 px; no small text.
- Strong silhouette and contrast; test on light and dark backgrounds and next to competitors' icons in the category.
- Built from the game's art and palette (`ART_DIRECTION.md`).
- Make 2–3 variants for a store listing experiment (Play) or product page optimization (App Store).

## 2. Screenshots

- Capture from the real game in a clean state: a capture mode that hides debug UI, uses good-looking save data, and renders at the exact store resolutions.
- The first 2–3 screenshots carry most of the conversion: show the core verb in action and the most appealing moment.
- Short caption per screenshot (3–6 words) stating a benefit, in a readable font from the game's UI style.
- Order: core gameplay → progression/meta → variety/content → social or special features.
- Produce all sizes required by each store (check current requirements) and localize captions for the top markets.

## 3. Feature graphic and preview video

- Play feature graphic 1024×500: key art with the game name; keep important content away from edges.
- Preview video (15–30 s): real gameplay, the hook in the first 3 seconds, no long logos. Use the `short-form-video` skill for the edit.

## 4. Listing copy

- **Title**: game name plus 1–2 keywords if allowed and natural (App Store title ≤ 30 chars, subtitle ≤ 30; Play title ≤ 30, short description ≤ 80; check current limits).
- **Keywords**: list 20–40 candidate terms players use (genre, mechanic, theme, mood). Pick by relevance first, then estimated volume and competition. App Store has a separate 100-character keyword field: no repeats, no spaces after commas.
- **Description**: first 2–3 lines sell the experience (shown before "more"); then features as short lines; no keyword stuffing.
- Localize for the target markets, not only translated but with locally searched keywords.

## Output

A `store/` folder in the project (or `docs/gamedev/store/`) with icon variants, screenshots per size and language, feature graphic, the video plan, and `listing.md` with copy and keywords per store and language.
