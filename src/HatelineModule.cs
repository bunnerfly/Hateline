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

        public bool HasForcedHat => Settings.AllowMapChanges && Session?.MapForcedHat != null;

        public bool ShouldShowHat => HasForcedHat || Settings.Enabled;
        public string? CurrentHat => HasForcedHat ? Session.MapForcedHat : Settings.SelectedHat;

        public int CurrentX => HasForcedHat ? Session.mapsetX : Settings.CrownX;
        public int CurrentY => HasForcedHat ? Session.mapsetY : Settings.CrownY;

        public static List<string> hats = new List<string>();

        public const string DEFAULT = "none";

        public HatelineModule()
        {
            Instance = this;
        }

        public override void Load()
        {
            typeof(GravityHelperImports).ModInterop();

            On.Celeste.Player.Added += hookPlayerAdded;
            On.Celeste.GameLoader.LoadThread += LateLoader;

            if (Everest.Modules.Any(m => m.Metadata.Name == "CelesteNet.Client"))
            {
                CelesteNetSupport.Load();
            }
        }

        private void hookPlayerAdded(On.Celeste.Player.orig_Added orig, Player self, Scene scene)
        {
            orig.Invoke(self, scene);
            self.Add(new HatComponent(Settings.SelectedHat));
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

        public static void ReloadHat(bool inGame, int x, int y)
        {
            string hat = Instance.CurrentHat;

            if (!inGame || !Settings.Enabled)
                return;

            HatComponent playerHatComponent = Engine.Scene.Tracker.GetComponents<HatComponent>().FirstOrDefault(c => c.Entity is Player) as HatComponent;

            if (playerHatComponent == null)
                return;

            if (Instance.HasForcedHat)
            {
                Session.mapsetX = x;
                Session.mapsetY = y;
            }
            else
            {
                x = Settings.CrownX;
                y = Settings.CrownY;
            }

            playerHatComponent.CreateHat(hat, true);
            playerHatComponent.SetPosition(x, y);
            playerHatComponent.UpdatePosition();

            Logger.Log(LogLevel.Verbose, "Hateline", $"ReloadHat: Calling SendPlayerHat of {CelesteNetSupport.CNetComponent}");
            CelesteNetSupport.CNetComponent?.SendPlayerHat();
        }
    }
}