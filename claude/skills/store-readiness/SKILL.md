---
name: store-readiness
description: Preflight a Unity mobile build and its store listing for Google Play and the Apple App Store - technical requirements, permissions, privacy disclosures, age rating, listing assets and review-guideline risks - before submission.
---

# Store Readiness Check

Store requirements change often (target API levels, SDK minimums, privacy rules). **Check the current official documentation for each requirement before marking it**, and cite the page and date checked. Never rely on memory for version numbers.

## Google Play

**Build**
- [ ] Android App Bundle (AAB), signed with the upload key; Play App Signing enabled
- [ ] Target API level meets the current Play requirement (check the "Target API level requirements" page)
- [ ] 64-bit (ARM64) included
- [ ] Version code higher than any previous upload
- [ ] Download size within limits; large content via Play Asset Delivery if needed
- [ ] Permissions in the final merged manifest: each one justified; remove what SDKs add but the game doesn't need (check `AndroidManifest` merge output)

**Policy and disclosures**
- [ ] Data safety form matches every SDK in the build (ads, analytics, crash reporting, attribution)
- [ ] Privacy policy URL live and linked in the listing and in the game
- [ ] Content rating questionnaire (IARC) answered to match content, including ads and in-app purchases
- [ ] Target audience and content: if children are in the target audience, Families policy and certified ad SDKs only
- [ ] Ads declaration set correctly
- [ ] Account deletion path provided, if the game creates accounts
- [ ] New developer accounts: closed testing requirement met before production (check current rules)

**Listing**
- [ ] App name, short description, full description
- [ ] Icon 512×512, feature graphic 1024×500, phone screenshots (and tablet if supported), optional promo video
- [ ] Category and tags, contact email

## Apple App Store

**Build**
- [ ] Built with the Xcode/SDK version Apple currently requires for submissions
- [ ] Unique build number; correct bundle ID; signed with a distribution profile
- [ ] Minimum iOS version deliberate
- [ ] Privacy manifest (`PrivacyInfo.xcprivacy`) present, and third-party SDKs that require one include theirs
- [ ] Usage description strings (`NS…UsageDescription`) for every permission the build can request, including `NSUserTrackingUsageDescription` if ATT is shown
- [ ] Export compliance (encryption) answered

**Policy and disclosures**
- [ ] App Privacy "nutrition labels" match every SDK's data collection
- [ ] ATT prompt shown before any tracking; ads work if the user declines
- [ ] Age rating questionnaire answered
- [ ] In-app purchases created, in "Ready to Submit" state, and submitted with the build if new
- [ ] Restore Purchases button available for non-consumables and subscriptions
- [ ] Account deletion in-app, if the game creates accounts
- [ ] Sign in with Apple offered if other third-party logins are offered (check current guideline)

**Listing**
- [ ] Name (≤ 30 chars), subtitle, keywords, description, promotional text
- [ ] Screenshots for required device sizes, optional app previews
- [ ] Support URL, privacy policy URL, review notes (with a test account or steps if needed)

## Common rejection risks for games

Crashes on launch on the reviewer's device, broken IAP in the sandbox, placeholder content, misleading screenshots, ads shown before consent, loot boxes without odds, references to other platforms in the listing.

## Output

`docs/gamedev/reviews/YYYY-MM-DD-store-readiness-<version>.md`: both checklists with PASS/FAIL/N/A and evidence, the documentation pages checked with dates, and blockers. This report feeds `release-gate`.
