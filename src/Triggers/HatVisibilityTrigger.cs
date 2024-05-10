using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.Hateline.Triggers
{
    [CustomEntity("Hateline/HatForceTrigger")]
    public class HatForceTrigger : Trigger
    {
        public string hat;

        public int hatX;
        public int hatY;

        public HatForceTrigger(EntityData data, Vector2 offset)
            : base(data, offset)
        {
            hat = data.Attr("hat", "flower");
            hatX = data.Int("hatX", 0);
            hatY = data.Int("hatY", -8);
        }

        public override void OnEnter(Player player)
        {
            base.OnEnter(player);

            if (HatelineModule.Settings.AllowMapChanges &&
                HatelineModule.Settings.Enabled)
            {
                HatelineModule.Session.MapForcedHat = hat;
                HatelineModule.ReloadHat(true, hatX, hatY);
            }
        }

        public override void OnLeave(Player player)
        {
            base.OnEnter(player);
        }
    }
}