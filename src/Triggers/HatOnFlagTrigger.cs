using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.Hateline.Triggers
{
    [CustomEntity("Hateline/HatOnFlagTrigger")]
    public class HatOnFlagTrigger : Trigger
    {
        public string hat;
        public string flag;

        public bool inverted;

        public int hatX;
        public int hatY;

        public HatOnFlagTrigger(EntityData data, Vector2 offset)
            : base(data, offset)
        {
            flag = data.Attr("flag", "");
            inverted = data.Bool("inverted", false);
            hat = data.Attr("hat", "flower");
            hatX = data.Int("hatX", 0);
            hatY = data.Int("hatY", -8);
        }

        public override void OnEnter(Player player)
        {
            bool flagVal = SceneAs<Level>().Session.GetFlag(flag);
            if (inverted)
                flagVal = !flagVal;

            base.OnEnter(player);
            if (flagVal && HatelineModule.Settings.AllowMapChanges && HatelineModule.Settings.Enabled)
            {
                HatelineModule.Session.MapForcedHat = hat;

                // FIXME: Should this be using HatelineModule.currentHat? Or this trigger's hat...
                HatComponent.ReloadHat(HatelineModule.currentHat, true, hatX, hatY);
            }
        }

        public override void OnLeave(Player player)
        {
            base.OnEnter(player);
        }
    }
}