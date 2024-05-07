using System;
using MonoMod.ModInterop;

namespace Celeste.Mod.Hateline
{
    [ModImportName("GravityHelper")]
    public static class GravityHelperImports
    {
        public static Func<bool> IsPlayerInverted;

        public static Func<Actor, bool> IsActorInverted;
    }
}
