using System;
using System.Collections.Generic;
using System.Linq;
using Celeste.Mod.Hateline.CelesteNet;
using FMOD.Studio;
using Monocle;
using MonoMod.ModInterop;

namespace Celeste.Mod.Hateline
{
    public class HatelineModule : EverestModule
    {
        public static HatelineModule Instance { get; private set; }

        public override Type SettingsType => typeof(HatelineModuleSettings);
        public static HatelineModuleSettings Settings => (HatelineModuleSettings)Instance._Settings;

        public override Type SessionType => typeof(HatelineModuleSession);
        public static HatelineModuleSession Session => (HatelineModuleSession)Instance._Session;

        public string? SessionForcedHat => Settings.AllowMapChanges ? Session.MapForcedHat : null;

        public static List<string> hats = new List<string>();

        public static string currentHat = "none";

        public static readonly string DEFAULT = "none";

        public HatelineModule()
        {
            Instance = this;
        }

        public override void Load()
        {
            typeof(GravityHelperImports).ModInterop();

            On.Celeste.Player.Added += hookPlayerAdded;
            On.Celeste.GameLoader.LoadThread += LateLoader;

            currentHat = Settings.SelectedHat;

            if (Everest.Modules.Any(m => m.Metadata.Name == "CelesteNet.Client"))
            {
                CelesteNetSupport.Load();
            }
        }

        private void hookPlayerAdded(On.Celeste.Player.orig_Added orig, Player self, Scene scene)
        {
            orig.Invoke(self, scene);
            self.Add(new HatComponent(currentHat, Settings.CrownX, Settings.CrownY));
        }

        public override void CreateModMenuSection(TextMenu menu, bool inGame, EventInstance snapshot)
        {
            base.CreateModMenuSection(menu, inGame, snapshot);
            HatelineSettingsUI.CreateMenu(menu, inGame);
        }

        private void LateLoader(On.Celeste.GameLoader.orig_LoadThread orig, GameLoader self)
        {
            orig(self);
        }

        public override void Unload()
        {
            On.Celeste.Player.Added -= hookPlayerAdded;
            On.Celeste.GameLoader.LoadThread -= LateLoader;

            if (Everest.Modules.Any(m => m.Metadata.Name == "CelesteNet.Client"))
            {
                CelesteNetSupport.Unload();
            }
        }
    }
}