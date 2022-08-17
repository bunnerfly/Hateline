using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monocle;
using Microsoft.Xna.Framework;
using Celeste.Mod.Entities;

namespace Celeste.Mod.Hateline.Triggers
{
    [CustomEntity("Hateline/HatVisibilityTrigger")]
    public class HatVisibilityTrigger : Trigger
    {
        private bool persistent = true;

        private bool hide = true;

        public HatVisibilityTrigger(EntityData data, Vector2 offset) : base(data, offset)
        {
            persistent = data.Bool("true");
            hide = data.Bool("true");
        }

        public override void OnEnter(Player player)
        {
            base.OnEnter(player);

            if (HatelineModule.Settings.AllowMapChanges &&
                HatelineModule.Settings.Enabled)
            {
                player.Components.Get<HatComponent>().Visible = !hide;
            }
        }

        public override void OnLeave(Player player)
        {
            base.OnEnter(player);

            if (!persistent)
            {
                player.Components.Get<HatComponent>().Visible = hide;
            }
        }
    }
}
