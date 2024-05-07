using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.Hateline.Triggers
{
    [CustomEntity("Hateline/HatResetTrigger")]
    public class HatResetTrigger : Trigger
    {
        public HatResetTrigger(EntityData data, Vector2 offset)
            : base(data, offset)
        {
        }

        public override void OnEnter(Player player)
        {
            base.OnEnter(player);

            HatelineModule.Session.MapForcedHat = null;
            HatelineModule.Session.mapsetX = 0;
            HatelineModule.Session.mapsetY = 0;

            HatComponent.ReloadHat(HatelineModule.Settings.SelectedHat, true, HatelineModule.Settings.CrownX, HatelineModule.Settings.CrownY);
        }
    }
}
