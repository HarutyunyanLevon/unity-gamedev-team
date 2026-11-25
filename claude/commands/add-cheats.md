---
description: Add dev-only debug cheats across the whole game, or to one system, so any game state can be reached in seconds.
argument-hint: "[empty for a full sweep | a system, e.g. 'economy' or 'daily rewards']"
---

Act as the `unity-developer` agent and follow the `debug-cheats` skill.

Scope (empty means sweep every system): $ARGUMENTS

1. If the project has no cheat framework yet (no `CheatAttribute` / `CheatMenu`), install it from the skill's `reference/` files. Choose the asmdef or the Assembly-CSharp layout based on how the game's code is organized.
2. Inventory the systems in scope and compare them with `docs/gamedev/CHEATS.md` and the existing `[Cheat]` attributes.
3. Add the missing cheats from the skill's catalogue, plus game-specific ones for any state that is slow to reach. Go through the game's real APIs. Where a system has no API a cheat can use, add a small public one and list it.
4. Compile. If you can enter Play Mode, open the menu (backquote key) and run every new cheat once. Otherwise mark them unverified.
5. Update `docs/gamedev/CHEATS.md`. Reply with a table of the cheats added per system, the verification status, any new game APIs, and any gaps you left.
