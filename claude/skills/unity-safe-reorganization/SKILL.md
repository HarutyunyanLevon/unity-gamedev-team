---
name: unity-safe-reorganization
description: Plan and execute safe moves and renames of Unity assets, scripts, prefabs, scenes and folders while preserving GUIDs, meta files, serialized references, Addressables and asmdef boundaries. Use for any folder cleanup, rename, or restructure.
---

# Unity Safe Reorganization

Unity links assets by GUID stored in `.meta` files. Moving a file without its `.meta`, or recreating a file, silently breaks every reference to it. This skill avoids that.

## Rules

1. **Always move the `.meta` with its asset**, with the same name plus `.meta`. Folders have `.meta` files too.
2. **Prefer the Editor.** Moving in the Project window (or via `AssetDatabase.MoveAsset` in an editor script) is always safe. Moving on disk is only safe when the Editor is closed and every `.meta` moves along.
3. **Never delete and recreate** an asset to "move" it. That creates a new GUID.
4. **Script class renames**: the file name must match the class for MonoBehaviours/ScriptableObjects. Rename file and class together and keep the `.meta`. Add `[MovedFrom]` for namespace or assembly moves where serialized references (like `[SerializeReference]`) need it.
5. **Serialized field renames**: add `[FormerlySerializedAs("oldName")]`.
6. **Resources and string paths**: moving anything referenced by `Resources.Load("path")`, Addressables address strings, or scene names in `SceneManager.LoadScene("name")` breaks it. Search code for these strings first.
7. **asmdef boundaries**: moving a script into another asmdef's folder changes its assembly. Check references still compile.

## Procedure

1. **Plan**: write the move list (from → to) in `docs/gamedev/reports/YYYY-MM-DD-reorg-plan.md`. Group into small batches (one feature or folder each).
2. **Check references** for each batch: string-based loads (`Resources.Load`, `LoadScene`, Addressables keys), Build Settings scene list, asmdef membership, editor scripts with hard-coded paths.
3. **Clean working tree** in git. Commit before starting.
4. **Move one batch**, in the Editor or with a script using `AssetDatabase.MoveAsset` (returns an error string on failure, empty on success). If moving on disk, the Editor must be closed and each `.meta` must be moved too.
5. **Verify**: Editor opens with no errors; no new missing references (search for `guid: 00000000000000000000000000000000` and missing scripts); boot scene plays; Build Settings list intact; Addressables build succeeds if used.
6. **Commit the batch** with a message listing the moves. Git should show the moves as renames with the GUIDs unchanged.
7. Repeat.

## Example editor script

```csharp
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ReorgBatch
{
    [MenuItem("Tools/Reorg/Run Batch")]
    public static void Run()
    {
        var moves = new (string from, string to)[]
        {
            ("Assets/Scripts/Player.cs", "Assets/Game/Player/Player.cs"),
        };
        foreach (var (from, to) in moves)
        {
            var folder = System.IO.Path.GetDirectoryName(to).Replace('\\', '/');
            if (!AssetDatabase.IsValidFolder(folder))
                Debug.LogError($"Create folder first: {folder}");
            var err = AssetDatabase.MoveAsset(from, to);
            Debug.Log(string.IsNullOrEmpty(err) ? $"Moved {from} -> {to}" : $"FAILED {from}: {err}");
        }
        AssetDatabase.Refresh();
    }
}
#endif
```
