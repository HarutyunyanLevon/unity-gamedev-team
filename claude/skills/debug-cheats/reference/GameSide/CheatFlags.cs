// Lives in the GAME's code (not in Game.Cheats), because gameplay reads these flags.
// In release builds every flag is a read-only property that returns false, so nothing can switch it on.
// Usage in gameplay:  if (!CheatFlags.GodMode) health -= damage;
namespace Game
{
    public static class CheatFlags
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        public static bool GodMode { get; set; }
        public static bool InfiniteMoves { get; set; }
        public static bool InfiniteCurrency { get; set; }
        public static bool ShowHitboxes { get; set; }
#else
        public static bool GodMode => false;
        public static bool InfiniteMoves => false;
        public static bool InfiniteCurrency => false;
        public static bool ShowHitboxes => false;
#endif
    }
}
