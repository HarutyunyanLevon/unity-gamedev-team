---
name: ads-mediation-review
description: Review or plan a mobile game's ad setup - placements, formats, frequency caps, mediation and waterfall/bidding, consent (GDPR/UMP, US state privacy, ATT), test vs production configuration and player experience. Use when adding ads, changing placements, or before release.
---

# Ads and Mediation Review

## 1. Placements and player experience

List every ad placement: format (rewarded, interstitial, banner, app open), trigger, caps, cooldown, and who's excluded (payers, new players).

Check:
- **Rewarded**: opt-in only, at a natural decision point, reward stated exactly, reward delivered exactly once (also when the ad is closed early or the app is backgrounded), button hidden when no ad is loaded.
- **Interstitials**: only at natural breaks (between rounds), never during play or right after a purchase; not in the first N sessions/minutes; time and count caps; off for "remove ads" buyers.
- **Banners**: don't cover gameplay or buttons; respect safe areas; consider whether they're worth it at all.
- **App open ads**: rarely appropriate for games; never on first launch.
- **Failure paths**: no fill, slow load, offline. The game never waits on an ad.

## 2. Consent and privacy

- A consent flow (for example Google UMP / a certified CMP) runs **before** the first ad request in regions that need it (EEA, UK, Switzerland), and a privacy options entry point exists in settings.
- US state privacy and opt-out signals handled per the ad network's guidance.
- iOS: ATT prompt shown (with a pre-prompt explaining why, if used) before requesting the IDFA; SKAdNetwork IDs for every network in `Info.plist`.
- Children/mixed audience: child-directed flags set, and only certified networks.
- Data safety / privacy labels list the ad SDKs.

## 3. Mediation setup

- Mediation platform and networks listed with SDK and adapter versions; adapters match the mediation SDK version.
- Bidding vs waterfall configured as intended; no stale waterfall instances.
- App and ad unit IDs per platform, coming from build config, not hard-coded twice.
- **Test mode**: test devices or test ads in development; production IDs and no test mode in release (a classic release bug: check it in `release-gate`).
- Android: external dependency resolution clean (no duplicate classes); iOS: pods resolve.
- If using LevelPlay, `unity:levelplay-unity-integration` covers the integration details.

## 4. Code boundaries

Ads are behind a game-owned `IAds` interface; gameplay never calls the SDK directly. A fake implementation exists for the Editor and tests. Ad events are logged to analytics (`ad_show`, `ad_reward` with placement).

## 5. Measure

KPIs: impressions per DAU per placement, rewarded opt-in rate, eCPM by format and country, and retention of ad-exposed vs not. Caps and placements should be remote-config keys so they can be tuned.

## Output

`docs/gamedev/reports/YYYY-MM-DD-ads-review.md`: placement table, consent check, mediation config, findings ranked (compliance problems first), and recommended changes.
