---
name: unity-dependency-audit
description: Trace forward and reverse dependency graphs across Unity scenes, prefabs, prefab variants, ScriptableObjects, materials and scripts by GUID. Use before deleting or replacing an asset, to find what a scene really pulls into the build, or to find unused assets.
---

# Unity Prefab and Scene Dependency Audit

## How references work

Serialized Unity files (`.unity`, `.prefab`, `.asset`, `.mat`, `.controller`, `.anim`, etc.) are YAML when Asset Serialization is "Force Text" (check `ProjectSettings/EditorSettings.asset`, `m_SerializationMode: 2`). A reference looks like:

```yaml
m_Material: {fileID: 2100000, guid: 6a2f..., type: 2}
```

The `guid` is in the target asset's `.meta` file (`guid: 6a2f...`). So:
- **Forward deps of X** = all GUIDs inside X's file, resolved to paths via `.meta` files.
- **Reverse deps of X** = all files that contain X's GUID.

If serialization is binary, use the Editor: `AssetDatabase.GetDependencies(path, recursive)` for forward deps; reverse deps need a scan over all assets.

## Procedure

1. **Build a GUID → path index** from every `.meta` file under `Assets/` (and `Packages/` if relevant).
2. **Forward graph** for the target(s): extract `guid: ([0-9a-f]{32})` from the file, map to paths, and recurse if needed.
3. **Reverse graph**: search every serialized file for the target's GUID.
4. **Add non-GUID references**, which the graph misses:
   - `Resources.Load("...")` strings
   - Addressables addresses and labels
   - Scene names in `LoadScene`
   - Reflection or `Type.GetType` strings
   - Shaders found via `Shader.Find("name")`
5. **Report.**

## Quick scripts (Force Text projects)

```bash
# GUID of an asset
grep -m1 '^guid:' "Assets/Path/Thing.prefab.meta"

# Who references it (reverse deps)
grep -rl --include='*.unity' --include='*.prefab' --include='*.asset' --include='*.mat' --include='*.controller' \
  "<guid>" Assets/
```

## Use cases

- **Safe delete**: reverse deps empty and no string references → safe. Otherwise list what breaks.
- **Unused assets**: assets with zero reverse deps that aren't in Build Settings, Resources, Addressables or loaded by string. Verify before deleting; editor-only and test assets show up here too.
- **Build bloat**: forward deps of each Build Settings scene plus Resources plus Addressables; sort by file size.
- **Replace an asset**: swap GUIDs in referencing files only if the fileIDs are compatible (same asset type); otherwise reassign in the Editor.

## Output

`docs/gamedev/reports/YYYY-MM-DD-deps-<target>.md`: target, forward deps, reverse deps, string references found, verdict (safe to delete / move / replace, or what breaks).
