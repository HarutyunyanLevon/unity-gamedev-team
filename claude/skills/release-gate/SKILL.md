---
name: release-gate
description: Use when a candidate build is about to go to players (soft launch, store release, or a live update), or the owner asks for a release check, readiness review or sign-off. Produces a documented GO / NO-GO with evidence.
---

# Release Gate

Turns "looks ready" into a ship decision with evidence. It runs once per candidate build, after the cheap checks are done.

## Inputs

- The exact candidate: version name, version code / build number, commit hash, Unity version.
- The latest `regression-check` report for this candidate. If there is none, run `regression-check` first.
- The latest `store-readiness` report (for store releases).

## Checklist

Each item is **PASS**, **FAIL**, or **WAIVED** (owner waiver, logged in `DECISIONS.md` with a reason). Each PASS needs an evidence link or one-line note.

### Build identity
- [ ] Version name and code bumped, and match release notes
- [ ] Built from a tagged commit, with no uncommitted changes
- [ ] Release configuration: no development build, no script debugging, no debug menus or cheats reachable
- [ ] Android: AAB, target API meets current Google Play requirement, 64-bit, signed with the upload key
- [ ] iOS: archive signed with the distribution profile, build number unique

### Stability
- [ ] Regression pass done on this exact build, with no open blockers
- [ ] Crash-free sessions in internal testing ≥ 99.5% (or no crashes in N test sessions if there's no telemetry yet)
- [ ] Cold start, background/resume, low-memory kill and resume, airplane mode, and interrupted purchase all behave

### Saves and data
- [ ] An existing save from the previous live version loads and migrates correctly
- [ ] A fresh install starts clean
- [ ] Cloud save / account link conflict path tested (if any)

### Performance (on the reference low-end device)
- [ ] Meets the frame-rate budget in the heaviest scene
- [ ] Startup to first interactive frame within budget
- [ ] No thermal throttling within a 20-minute session (or it's documented)
- [ ] Install size within budget

### Business
- [ ] IAP: every product purchases, restores, and handles cancel/failure in the store's test environment
- [ ] Ads: test ads off and production IDs on; consent flow (GDPR/UMP, ATT on iOS) shows before ad requests
- [ ] Analytics: key events arrive with correct parameters in the production project
- [ ] Remote config: production defaults are safe if the fetch fails; kill switches work

### Store and legal
- [ ] `store-readiness` PASS for each target store
- [ ] Privacy policy URL live; data-safety (Play) and privacy nutrition labels (App Store) match the SDKs actually in the build
- [ ] Age rating questionnaire matches content and ads

### Rollout plan
- [ ] Staged rollout percentage chosen (for example Play 10% → 50% → 100%)
- [ ] Rollback or halt plan written: who watches which metric, and what threshold halts the rollout

## Output

Write `docs/gamedev/reviews/YYYY-MM-DD-release-<version>.md` containing the checklist with results, a **Verdict: GO / NO-GO**, the blockers if NO-GO, and the rollout plan. Never mark GO with an unresolved FAIL. Waivers must name who waived and why.
