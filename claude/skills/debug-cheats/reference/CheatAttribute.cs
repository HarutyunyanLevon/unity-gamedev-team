#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;

namespace Game.Cheats
{
    /// <summary>
    /// Marks a static method (button) or a static bool property with get/set (toggle) as a cheat.
    /// Path: "Group/Label". Args are passed to the method; stack several attributes for presets.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class CheatAttribute : Attribute
    {
        public string Path { get; }
        public object[] Args { get; }

        /// <summary>Destructive cheat: the first tap arms it, a second tap within 3 s runs it.</summary>
        public bool Confirm { get; set; }

        public CheatAttribute(string path, params object[] args)
        {
            Path = path;
            Args = args ?? Array.Empty<object>();
        }
    }
}
#endif
