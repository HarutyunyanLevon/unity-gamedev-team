#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Game.Cheats
{
    public sealed class CheatEntry
    {
        public string Path;
        public string Group;
        public string Label;
        public MethodInfo Method;   // button
        public object[] Args;
        public PropertyInfo Toggle; // bool toggle
        public bool Confirm;

        public bool IsToggle => Toggle != null;
        public bool IsOn => Toggle != null && (bool)Toggle.GetValue(null);
    }

    /// <summary>Finds every [Cheat] in the assembly that defines CheatAttribute and runs them by entry or path.</summary>
    public static class CheatRegistry
    {
        static List<CheatEntry> _all;
        static List<string> _groups;

        public static IReadOnlyList<CheatEntry> All => _all ??= Scan();
        public static IReadOnlyList<string> Groups => _groups ??= All.Select(e => e.Group).Distinct().ToList();

        /// <summary>Run a cheat by its full path, e.g. from PlayMode tests or bots.</summary>
        public static bool Run(string path)
        {
            foreach (var e in All)
                if (e.Path == path) { Run(e); return true; }
            Debug.LogWarning($"[Cheat] not found: {path}");
            return false;
        }

        public static void Run(CheatEntry e)
        {
            try
            {
                if (e.IsToggle)
                {
                    bool next = !e.IsOn;
                    e.Toggle.SetValue(null, next);
                    Debug.Log($"[Cheat] {e.Path} = {(next ? "ON" : "OFF")}");
                }
                else
                {
                    e.Method.Invoke(null, e.Args);
                    Debug.Log($"[Cheat] {e.Path}");
                }
            }
            catch (TargetInvocationException ex) { Debug.LogException(ex.InnerException ?? ex); }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        static List<CheatEntry> Scan()
        {
            const BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            var list = new List<CheatEntry>();
            Type[] types;
            try { types = typeof(CheatAttribute).Assembly.GetTypes(); }
            catch (ReflectionTypeLoadException ex) { types = ex.Types.Where(t => t != null).ToArray(); }

            foreach (var type in types)
            {
                foreach (var m in type.GetMethods(flags))
                foreach (var a in m.GetCustomAttributes<CheatAttribute>())
                {
                    if (m.GetParameters().Length != a.Args.Length)
                    {
                        Debug.LogWarning($"[Cheat] {a.Path}: {type.Name}.{m.Name} takes {m.GetParameters().Length} args, attribute gives {a.Args.Length}. Skipped.");
                        continue;
                    }
                    list.Add(Make(a, m, null));
                }

                foreach (var p in type.GetProperties(flags))
                foreach (var a in p.GetCustomAttributes<CheatAttribute>())
                {
                    if (p.PropertyType != typeof(bool) || !p.CanRead || !p.CanWrite)
                    {
                        Debug.LogWarning($"[Cheat] {a.Path}: toggles must be static bool properties with get and set. Skipped.");
                        continue;
                    }
                    list.Add(Make(a, null, p));
                }
            }

            // Stable sort by group keeps declaration order inside a group.
            return list.OrderBy(e => e.Group, StringComparer.Ordinal).ToList();
        }

        static CheatEntry Make(CheatAttribute a, MethodInfo m, PropertyInfo p)
        {
            int slash = a.Path.IndexOf('/');
            return new CheatEntry
            {
                Path = a.Path,
                Group = slash > 0 ? a.Path.Substring(0, slash) : "Misc",
                Label = slash > 0 ? a.Path.Substring(slash + 1) : a.Path,
                Method = m,
                Args = a.Args,
                Toggle = p,
                Confirm = a.Confirm,
            };
        }
    }
}
#endif
