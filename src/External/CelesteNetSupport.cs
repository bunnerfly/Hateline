using System.Linq;

namespace Celeste.Mod.Hateline.CelesteNet
{
    public static class CelesteNetSupport
    {
        public const int PROTOCOL_VERSION = 1;

        public static bool loaded;

        private static CelesteNetHatComponent _hatComponent;

        public static void Load()
        {
            if (loaded) return;
            if (Everest.Modules.Any((EverestModule m) => m.Metadata.Name == "CelesteNet.Client"))
            {
                Celeste.Instance.Components.Add(_hatComponent = new CelesteNetHatComponent(Celeste.Instance));
                loaded = true;
                On.Celeste.Player.Update += Player_Update;
            }
        }

        public static void Unload()
        {
            if (!loaded) return;
            if (Everest.Modules.Any((EverestModule m) => m.Metadata.Name == "CelesteNet.Client"))
            {
                Celeste.Instance.Components.Remove(_hatComponent);
                _hatComponent.Dispose();
                _hatComponent = null;
                loaded = false;
                On.Celeste.Player.Update -= Player_Update;
            }
        }

        private static void Player_Update(On.Celeste.Player.orig_Update orig, Player self)
        {
            orig(self);
            _hatComponent?.SendPlayerHat(HatelineModule.Settings.CrownX, HatelineModule.Settings.CrownY, HatelineModule.Settings.SelectedHat);
            // Console.WriteLine("Sending hat with info" + HatelineModule.Settings.CrownX + ", " + HatelineModule.Settings.CrownY + ", " + HatelineModule.Settings.SelectedHat);
        }
    }
}
