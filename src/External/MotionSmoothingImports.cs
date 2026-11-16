using System;
using Monocle;
using MonoMod.ModInterop;

namespace Celeste.Mod.Hateline;

[ModImportName("MotionSmoothing")]
public static class MotionSmoothingImports
{
    // Ties a component's rendering to Madeline's smoothed position, so any attachment that anchors
    // itself to her (e.g. a hat, extra jump indicators) stays glued to her under both object smoothing
    // and subpixel rendering. Call this once when the component is created; the tie is dropped
    // automatically when the component is collected or manually with UntieFromPlayer.
    public static Action<Component> TieToPlayer;

    public static Action<Component> UntieFromPlayer;
}
