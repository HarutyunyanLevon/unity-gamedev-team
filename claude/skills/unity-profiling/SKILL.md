---
name: unity-profiling
description: Diagnose Unity CPU, GPU, memory, garbage collection, rendering, physics, UI, loading and frame-pacing problems on mobile with Profiler, Frame Debugger and Memory Profiler, and confirm fixes with before/after measurements. Use for one specific performance problem; use the game-performance-engineer agent for a whole campaign.
---

# Unity Performance Profiling

## 1. Measure on the right target

- Use a **Development Build** with **Autoconnect Profiler** on the actual device, connected by USB (`adb forward` handled by Unity for Android). Editor numbers are only for rough direction.
- Deep Profiling only for short targeted captures; it distorts timings.
- Record the worst case: heaviest scene, peak effects, after 10+ minutes (heat and memory growth).
- Write down: device, OS, build, scene, frame-rate target, measured median and 95th-percentile frame time.

## 2. Find what bounds the frame

In the Profiler's CPU module, compare **Main Thread** time, **Render Thread** time, and look for `Gfx.WaitForPresent` / `Gfx.WaitForGfxCommandsFromMainThread`:
- Main thread busy, little waiting → **CPU bound** (scripts, physics, animation, UI).
- Main thread waiting on present → **GPU bound**. Confirm by lowering the render resolution: if the frame time drops, it's GPU (fill rate/shader).
- Spikes rather than steady load → GC, loading, shader compile, or instantiation.

## 3. Drill down

| Symptom | Where to look | Typical fixes |
|---|---|---|
| GC spikes | CPU module GC Alloc column, Hierarchy sorted by allocation | Remove allocations in Update: cache, pool, avoid LINQ, string ops, boxing, closures |
| Scripts heavy | Hierarchy sorted by Self ms | Fewer Update calls (manager pattern), cache components, spread work over frames |
| Physics | Physics module | Larger fixed timestep, simpler colliders, layer matrix, fewer rigidbodies awake |
| UI | `Canvas.SendWillRenderCanvases`, `Canvas.BuildBatch` | Split static and dynamic canvases, disable raycast target on non-interactive graphics |
| Animation | `Animator.Update` | Cull offscreen animators, disable idle ones, simpler controllers |
| Draw calls | Frame Debugger, Rendering module | Atlases, SRP Batcher (URP), static/GPU instancing, fewer materials |
| Overdraw | Scene view overdraw mode, Frame Debugger | Fewer and smaller transparent layers, tighter sprite meshes, fewer full-screen effects |
| Memory | Memory Profiler package snapshot, compare two snapshots | ASTC texture compression, smaller atlases, audio load types, release Addressables handles |
| Loading | Profiler around the load, `Loading.*` markers | Async loading, Addressables, shader variant warmup, smaller first scene |

Add `ProfilerMarker` around suspect code to see it clearly:

```csharp
static readonly Unity.Profiling.ProfilerMarker k_Spawn = new("Game.Spawn");
using (k_Spawn.Auto()) { /* work */ }
```

## 4. Fix one thing, re-measure

Change one thing, rebuild, capture under the same conditions, and compare median and p95.

## Output

| Change | Device | Before (median / p95 ms) | After | Notes |
|---|---|---|---|---|

Save it as `docs/gamedev/reports/YYYY-MM-DD-profile-<topic>.md`. If installed, `unity-perf:unity-performance` and `unity-perf:csharp-zero-gc` go deeper on code-level fixes.
