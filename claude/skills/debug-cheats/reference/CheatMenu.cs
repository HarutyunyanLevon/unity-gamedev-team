#if UNITY_EDITOR || DEVELOPMENT_BUILD
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Game.Cheats
{
    /// <summary>
    /// On-device cheat menu (IMGUI, no art needed). Hold 3 fingers for 0.6 s, or press ` (backquote).
    /// Created automatically; exists only in the Editor and development builds.
    /// </summary>
    public sealed class CheatMenu : MonoBehaviour
    {
        const float HoldToOpen = 0.6f;
        const float ConfirmWindow = 3f;
        const float RowHeight = 44f;

        public static bool ShowFps;
        static CheatMenu _instance;

        bool _open;
        float _hold;
        string _group;
        string _filter = "";
        Vector2 _scroll;
        CheatEntry _armed;
        float _armedUntil;
        float _fps;

        public static void Hide() { if (_instance != null) _instance._open = false; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Boot()
        {
            if (_instance != null) return;
            var go = new GameObject("[Cheats]");
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<CheatMenu>();
        }

        void Update()
        {
            float dt = Time.unscaledDeltaTime;
            if (dt > 0f) _fps = Mathf.Lerp(_fps, 1f / dt, 0.1f);
            if (TogglePressed(dt)) _open = !_open;
        }

        bool TogglePressed(float dt)
        {
            int touches = 0;
            bool key = false;
#if ENABLE_INPUT_SYSTEM
            var ts = Touchscreen.current;
            if (ts != null)
                foreach (var t in ts.touches)
                    if (t.press.isPressed) touches++;
            key = Keyboard.current != null && Keyboard.current.backquoteKey.wasPressedThisFrame;
#else
            touches = Input.touchCount;
            key = Input.GetKeyDown(KeyCode.BackQuote);
#endif
            if (key) return true;
            if (touches < 3) { _hold = 0f; return false; }
            _hold += dt;
            if (_hold < HoldToOpen) return false;
            _hold = float.NegativeInfinity; // fire once per hold
            return true;
        }

        void OnGUI()
        {
            float scale = Mathf.Max(1f, Mathf.Min(Screen.width, Screen.height) / 540f);
            GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, 1f));

            if (ShowFps)
                GUI.Label(new Rect(8, 8, 160, 24), $"{_fps:0} fps  {Time.timeScale:0.##}x");

            if (!_open) return;

            Rect safe = Screen.safeArea;
            var area = new Rect(safe.x / scale, (Screen.height - safe.yMax) / scale, safe.width / scale, safe.height / scale);
            area = new Rect(area.x + 8, area.y + 8, area.width - 16, area.height - 16);

            // Drag anywhere to scroll (IMGUI scroll views don't drag-scroll with touch).
            var ev = Event.current;
            if (ev.type == EventType.MouseDrag && area.Contains(ev.mousePosition))
            {
                _scroll.y -= ev.delta.y;
                ev.Use();
            }

            GUILayout.BeginArea(area, GUI.skin.box);
            DrawHeader();
            _scroll = GUILayout.BeginScrollView(_scroll);
            if (!string.IsNullOrEmpty(_filter)) DrawEntries(null);
            else if (_group == null) DrawGroups();
            else DrawEntries(_group);
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        void DrawHeader()
        {
            GUILayout.BeginHorizontal();
            if (_group != null && string.IsNullOrEmpty(_filter))
            {
                if (GUILayout.Button("< Back", GUILayout.Height(RowHeight), GUILayout.Width(90))) { _group = null; _scroll = Vector2.zero; }
            }
            GUILayout.Label(_group ?? $"CHEATS  {Application.version}", GUILayout.Height(RowHeight));
            if (GUILayout.Button("Close", GUILayout.Height(RowHeight), GUILayout.Width(90))) _open = false;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Find", GUILayout.Width(40), GUILayout.Height(RowHeight));
            _filter = GUILayout.TextField(_filter, GUILayout.Height(RowHeight));
            if (_filter.Length > 0 && GUILayout.Button("x", GUILayout.Width(RowHeight), GUILayout.Height(RowHeight))) _filter = "";
            GUILayout.EndHorizontal();
        }

        void DrawGroups()
        {
            foreach (var g in CheatRegistry.Groups)
                if (GUILayout.Button(g, GUILayout.Height(RowHeight))) { _group = g; _scroll = Vector2.zero; }
        }

        void DrawEntries(string group)
        {
            if (_armed != null && Time.unscaledTime > _armedUntil) _armed = null;

            foreach (var e in CheatRegistry.All)
            {
                if (group != null && e.Group != group) continue;
                if (group == null && e.Path.IndexOf(_filter, System.StringComparison.OrdinalIgnoreCase) < 0) continue;

                string label = group == null ? e.Path : e.Label;
                if (e.IsToggle) label = (e.IsOn ? "[ON]  " : "[off] ") + label;
                if (_armed == e) label = "TAP AGAIN: " + label;

                if (!GUILayout.Button(label, GUILayout.Height(RowHeight))) continue;

                if (e.Confirm && _armed != e)
                {
                    _armed = e;
                    _armedUntil = Time.unscaledTime + ConfirmWindow;
                    continue;
                }
                _armed = null;
                CheatRegistry.Run(e);
            }
        }
    }
}
#endif
