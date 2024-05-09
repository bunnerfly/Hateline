using System.Linq;
using Monocle;

namespace Celeste.Mod.Hateline.CelesteNet
{
    public static class CelesteNetSupport
    {
        public const int PROTOCOL_VERSION = 1;

        public static bool loaded;

        private static CelesteNetHatComponent cnetComponent;

        public static CelesteNetHatComponent CNetComponent => cnetComponent;

        public static void Load()
        {
            if (loaded) return;
            if (Everest.Modules.Any((EverestModule m) => m.Metadata.Name == "CelesteNet.Client"))
            {
                Celeste.Instance.Components.Add(cnetComponent = new CelesteNetHatComponent(Celeste.Instance));
                loaded = true;
                On.Celeste.Player.Added += OnPlayerAdded;
            }
        }

        public static void Unload()
        {
            if (!loaded) return;
            if (Everest.Modules.Any((EverestModule m) => m.Metadata.Name == "CelesteNet.Client"))
            {
                Celeste.Instance.Components.Remove(cnetComponent);
                cnetComponent.Dispose();
                cnetComponent = null;
                loaded = false;
                On.Celeste.Player.Added -= OnPlayerAdded;
            }
        }

        private static void OnPlayerAdded(On.Celeste.Player.orig_Added orig, Player self, Scene scene)
        {
            orig(self, scene);

            Logger.Log(LogLevel.Verbose, "Hateline", $"OnPlayerAdded: Calling SendPlayerHat of {cnetComponent}");

            cnetComponent?.SendPlayerHat();
        }
    }
}
