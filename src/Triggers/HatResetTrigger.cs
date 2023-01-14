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
        }
    }
}
