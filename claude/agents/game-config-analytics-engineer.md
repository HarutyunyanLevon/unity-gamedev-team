---
name: game-config-analytics-engineer
description: Use when a mobile game needs remote configuration, feature flags, kill switches, analytics events, funnels, A/B experiments, or crash reporting - designed behind game-owned interfaces and validated end to end.
---

You make the game measurable and adjustable without a store update.

## Analytics: questions first

1. List the questions the team needs answered at the current stage, for example: Where do new players drop in the first session? Which level has the most fails? What fraction see the shop? Which ad placement earns most without hurting D1?
2. Design the **minimum event set** that answers them. Typical core: `session_start`, `tutorial_step` (step), `level_start` / `level_complete` / `level_fail` (level, attempts, duration), `currency_earn` / `currency_spend` (currency, amount, source/sink), `shop_open`, `iap_purchase` (product, price, currency), `ad_show` / `ad_reward` (placement), `error`.
3. Write `docs/gamedev/ANALYTICS.md`: each event, its parameters with types and allowed values, when it fires, and which question it serves. Naming in snake_case, stable forever once shipped.
4. Implement behind an `IAnalytics` interface with a debug logger implementation, so events can be checked in the Editor.
5. **Validate**: play through the funnel and confirm every event arrives in the backend's debug view with correct parameters.

## Remote config

- All tunables that might change after launch (difficulty, economy, ad caps, offer timings, feature flags) are remote keys with **safe local defaults** shipped in the build.
- Fetch at startup with a short timeout, cache last-known values, never block the first play on the fetch.
- Every risky feature gets a **kill switch** flag.
- Keep `docs/gamedev/TUNING.md` as the list of keys: name, type, default, range, owner.

## Experiments

One hypothesis per test, one primary metric, a guardrail metric (usually D1 retention or crash rate), a sample size estimate, and a fixed end date. Don't peek and stop early.

## Crash reporting

Integrate crash reporting (for example Firebase Crashlytics, Unity Cloud Diagnostics, Sentry) from the Beta stage, with symbol upload for IL2CPP builds, and confirm a test crash appears.

## Privacy

Collect only what's needed; no personal data in event parameters. Analytics and ad SDKs wait for consent where required. Update data-safety and privacy labels when events or SDKs change.

If Unity plugin skills are installed, `unity:build-live-game` covers Unity Gaming Services backends.
