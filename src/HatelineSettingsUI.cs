using System.Collections.Generic;
using System.Linq;
using Monocle;

namespace Celeste.Mod.Hateline
{
    public static class HatelineSettingsUI
    {
        public static void CreateMenu(TextMenu menu, bool inGame)
        {
            List<string> uiHats = new List<string>();

            foreach (string sprite in GFX.SpriteBank.SpriteData.Select(kvp => kvp.Key))
            {
                if (!sprite.StartsWith("hateline_"))
                    continue;

                string hatName = sprite.Replace("hateline_", "");
                if (hatName != HatelineModule.DEFAULT)
                    uiHats.Add(hatName);
            }
            HatelineModule.hats = uiHats.Distinct().ToList();

            HatSelector(menu, inGame);
        }

        public static void HatSelector(TextMenu menu, bool inGame)
        {
            var hatSelectionMenu = new TextMenu.Option<string>(Dialog.Clean("HATELINE_SETTINGS_CURHAT"));

            hatSelectionMenu.Add(Dialog.Clean("HATELINE_SETTINGS_DEFAULT"), HatelineModule.DEFAULT, true);

            foreach (string hat in HatelineModule.hats)
            {
                bool selected = hat == HatelineModule.Settings.SelectedHat;
                string name = Dialog.Clean("hateline_hat_" + hat);
                name = (name == "") ? hat : name;
                hatSelectionMenu.Add(name, hat, selected);
            }

            hatSelectionMenu.Change(SelectedHat => {
                HatelineModule.Settings.SelectedHat = SelectedHat;
                HatelineModule.ReloadHat(inGame, HatelineModule.Settings.CrownX, HatelineModule.Settings.CrownY);
            });

            if (inGame)
            {
                Player player = Engine.Scene?.Tracker.GetEntity<Player>();
                if (player != null && player.StateMachine.State == Player.StIntroWakeUp)
                {
                    hatSelectionMenu.Disabled = true;
                }
            }

            menu.Add(hatSelectionMenu);
        }
    }
}
