#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

namespace Game.Cheats
{
    /// <summary>Engine-level cheats that work in any project. Game-specific cheats go in their own files (EconomyCheats.cs, ProgressCheats.cs, ...).</summary>
    static class CoreCheats
    {
        [Cheat("Engine/Time scale x0.1", 0.1f)]
        [Cheat("Engine/Time scale x0.5", 0.5f)]
        [Cheat("Engine/Time scale x1", 1f)]
        [Cheat("Engine/Time scale x3", 3f)]
        [Cheat("Engine/Time scale x10", 10f)]
        static void SetTimeScale(float scale) => Time.timeScale = scale;

        [Cheat("Engine/FPS overlay")]
        static bool FpsOverlay { get => CheatMenu.ShowFps; set => CheatMenu.ShowFps = value; }

        [Cheat("Engine/Target 30 fps", 30)]
        [Cheat("Engine/Target 60 fps", 60)]
        [Cheat("Engine/Target 120 fps", 120)]
        static void SetTargetFps(int fps) => Application.targetFrameRate = fps;

        [Cheat("Engine/Quality: next level")]
        static void NextQuality()
        {
            int next = (QualitySettings.GetQualityLevel() + 1) % QualitySettings.names.Length;
            QualitySettings.SetQualityLevel(next, true);
            Debug.Log($"[Cheat] Quality = {QualitySettings.names[next]}");
        }

        [Cheat("Engine/Reload scene")]
        static void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        [Cheat("Engine/Screenshot (no menu)")]
        static void Screenshot()
        {
            CheatMenu.Hide();
            string name = $"shot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            // On mobile, CaptureScreenshot already writes under persistentDataPath.
            ScreenCapture.CaptureScreenshot(Application.isMobilePlatform ? name : Path.Combine(Application.persistentDataPath, name));
            Debug.Log($"[Cheat] Screenshot {name} in {Application.persistentDataPath}");
        }

        [Cheat("Engine/Copy build + device info")]
        static void CopyInfo()
        {
            GUIUtility.systemCopyBuffer =
                $"{Application.productName} {Application.version} | Unity {Application.unityVersion} | " +
                $"{SystemInfo.deviceModel} | {SystemInfo.operatingSystem} | RAM {SystemInfo.systemMemorySize} MB | " +
                $"{Screen.width}x{Screen.height} @ {Screen.dpi:0} dpi";
        }

        [Cheat("Engine/Log memory")]
        static void LogMemory()
        {
            const long mb = 1024 * 1024;
            Debug.Log($"[Cheat] Mono used {Profiler.GetMonoUsedSizeLong() / mb} MB, " +
                      $"Unity allocated {Profiler.GetTotalAllocatedMemoryLong() / mb} MB, " +
                      $"reserved {Profiler.GetTotalReservedMemoryLong() / mb} MB");
        }

        [Cheat("Engine/Force GC")]
        static void ForceGc() => GC.Collect();

        [Cheat("Engine/Delete PlayerPrefs", Confirm = true)]
        static void DeletePrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}
#endif
