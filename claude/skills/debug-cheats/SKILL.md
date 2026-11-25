---
name: debug-cheats
description: Use when adding, extending or auditing debug cheats in a Unity mobile game - a dev-only cheat menu plus cheats for every system (currency, levels, time travel, saves, FTUE, IAP/ads fakes, remote config) so anyone can reach any game state in seconds. Also use when a new system is added, when QA needs a faster repro, or when checking that no cheats reach a release build.
---

> **Platform:** read the `Platform:` line in the project's `CLAUDE.md`. On `pc-steam`, use the PC row of the Platform table wherever this file says phone, touch, device or store.

# Debug Cheats

Goal: anyone on the team can reach any game state in a few taps, on device, without editing code or saves. That makes testing, tuning, playtests, store screenshots and bug repro fast. Cheats are compiled out of release builds by construction, not by discipline.

## The framework (install once)

Reference code is in `reference/` next to this file. Copy it into the game:

| File | Goes to | What it is |
|---|---|---|
| `Game.Cheats.asmdef` | `Assets/_Dev/Cheats/` | Assembly that only compiles in the Editor and development builds |
| `CheatAttribute.cs`, `CheatRegistry.cs`, `CheatMenu.cs`, `CoreCheats.cs` | `Assets/_Dev/Cheats/` | `[Cheat]` attribute, discovery and runner, on-device menu, engine-level cheats |
| `link.xml` | `Assets/_Dev/Cheats/` | Stops IL2CPP code stripping from removing cheats that are only found through reflection |
| `GameSide/CheatFlags.cs` | the game's own code | Flags that gameplay reads (god mode and so on). In release builds they are read-only `false` |

Setup notes:
- **Game code uses asmdefs:** keep `Game.Cheats.asmdef` and add the game's assemblies to its `references`. Game code must never reference `Game.Cheats`.
- **Game code is in Assembly-CSharp (no asmdefs):** an asmdef can't reference Assembly-CSharp, so delete `Game.Cheats.asmdef`. Every reference file already wraps itself in `#if UNITY_EDITOR || DEVELOPMENT_BUILD`, and `link.xml` handles both layouts.
- **Opening the menu:** hold 3 fingers on the screen for 0.6 s on device, or press the backquote key (`` ` ``) in the Editor or on desktop. It uses the new Input System when that's active and the old Input Manager otherwise.
- If a `tuning-workbench` panel exists, open it from a cheat (`[Cheat("Tuning/Open panel")]`) instead of giving it a second gesture.
- Need cheats in a non-development build (for example, for profiling without dev overhead)? Add `|| CHEATS` to the define constraint and to the `#if` lines. Then `release-gate` must confirm the `CHEATS` define is absent from store builds.

## Writing a cheat

One attribute on a static method or a static `bool` property inside the cheats code. The first path segment is the menu group.

```csharp
[Cheat("Economy/Coins +1000", 1000)]
[Cheat("Economy/Coins +100k", 100_000)]
static void AddCoins(int amount) => Services.Wallet.Add(Currency.Coins, amount, source: "cheat");

[Cheat("Gameplay/God mode")]                       // a bool property shows up as a toggle
static bool GodMode { get => CheatFlags.GodMode; set => CheatFlags.GodMode = value; }

[Cheat("Save/Wipe save and restart", Confirm = true)]  // destructive: needs a second tap
static void WipeSave() { Services.Save.DeleteAll(); SceneManager.LoadScene(0); }
```

Rules:
1. **Go through the game's real APIs and commands** (the same ones the player's actions use), so events fire, the UI updates and the save stays valid. Never poke private fields or write save files by hand. If no API exists, add a small public one to the system. That is a testability win anyway (`testable-by-design`).
2. **Gameplay reads flags, never cheats.** Gameplay code may read `CheatFlags.X`. It must never call into `Game.Cheats`.
3. **Presets over input fields.** Typing is painful on phones. Use 2–4 preset values per cheat (`+1000`, `+100k`, `level 1/10/50`).
4. **Destructive cheats** (wipe, reset, delete) get `Confirm = true`.
5. **Tag cheated data.** Pass `source: "cheat"` (or the game's equivalent) to economy and analytics calls, and set a `usedCheats` flag in the save, so dev data and bug reports can be filtered.
6. **Name cheats by outcome** ("Skip to level 20", not "SetLvl").
7. Cheats are also an API: PlayMode tests and bots can call `CheatRegistry.Run("Progress/Win level")` to set up a state.

## The sweep: "cheats everywhere"

1. **Inventory systems.** List every game system from the code and `CONCEPT.md`: economy and currencies, inventory and items, progression and levels, core gameplay, timers and energy, daily or calendar features, FTUE, save, IAP, ads, remote config and flags, analytics, notifications, network, audio, localization, UI.
2. **Compare with `docs/gamedev/CHEATS.md`** (create it if it's missing) and with the existing `[Cheat]` attributes.
3. **Add the standard set for each system** from the catalogue below. Skip what doesn't apply. Add game-specific ones where a tester would otherwise grind.
4. **Ask "what state is slow to reach?"** For every state a tester or designer needs (end-game, rare drop, a full inventory, an expired offer, a corrupt save), there should be a cheat that gets there in under 10 seconds.
5. **Compile, then verify in Play Mode.** Open the menu and run each new cheat at least once. Watch the console for `[Cheat]` logs and exceptions.
6. **Update `CHEATS.md`** with a table of group, cheat, what it does, the game API it uses, and its status.

## Catalogue (standard cheats by system)

| System | Cheats |
|---|---|
| Engine (in `CoreCheats.cs`) | time scale presets, FPS overlay, target frame rate, reload scene, screenshot, copy build/device info, log memory, quality level, delete PlayerPrefs |
| Economy | add each currency (2–3 presets), set to zero, add each item or booster, "infinite currency" toggle |
| Progression | win level, lose level, skip to level N (presets), unlock everything, reset progression, set player level/XP |
| Gameplay | god mode, infinite moves/lives/ammo, kill all enemies, spawn X, show hitboxes/debug gizmos, next wave |
| Time | +1 h, +24 h, +7 days on the game clock; finish all timers; refill energy; trigger the daily reset now. This needs the injected game clock from `testable-by-design`. Never change the device clock |
| FTUE | skip tutorial, reset tutorial, jump to step N |
| Save | save now, wipe (confirm), load fixture saves (fresh, early, mid, late, whale, old-version, corrupt), copy save JSON to the clipboard |
| IAP (fake store behind the game's interface) | next purchase succeeds / fails / is cancelled / stays pending, grant any product, toggle "no ads" purchased, reset purchases |
| Ads (fake provider) | always fill / no fill / fail to show, instant reward, reset ad caps, reset consent (GDPR/ATT flow) |
| Remote config and flags | override any value, toggle each feature flag, force an A/B variant, reset overrides |
| Analytics | toggle an on-screen event log, flush now |
| Live ops | start/end each event, expire offers now, show all offers |
| Network | simulate offline, add latency (through the game's network interface) |
| Notifications | schedule each local notification type in 10 s |
| UI and store art | hide HUD (for screenshots and trailers, see `store-art-aso`), cycle language, force small-screen or large-text layout |

## When cheats are needed, by stage

- **Prototype:** Engine cheats plus win, lose, and restart. Keep it cheap.
- **Vertical Slice:** full sweep of the systems that exist.
- **Alpha onward:** every new system ships with its cheats. A system without cheats isn't done.
- **Beta and soft launch:** IAP/ads fakes, remote config overrides and fixture saves are required for QA.
- **Release:** verify that no cheats are in the build (below).

## Release check (used by `release-gate`)

- The store build is a non-development build, so the cheats assembly and `#if` code don't exist in it.
- Search the release build log or the IL2CPP output for `Game.Cheats` / `CheatMenu`: there should be no hits. Holding 3 fingers on a release build does nothing.
- `CheatFlags` in release are read-only `false` properties, so no code path can turn them on.
- No `CHEATS` define in the release player settings (if that option is used).

## Output

Code in `Assets/_Dev/Cheats/`, an updated `docs/gamedev/CHEATS.md`, and a reply listing the cheats added per system, what was verified in Play Mode, and any system that needed a new public API to support its cheats.
