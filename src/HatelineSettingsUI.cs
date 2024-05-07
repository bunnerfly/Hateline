using System.Collections.Generic;
using System.Linq;
using Monocle;

namespace Celeste.Mod.Hateline
{
    public class HatelineSettingsUI
    {
        public string SelectedHat;

        public List<string> uiHats = new List<string>();

        public void CreateMenu(TextMenu menu, bool inGame)
        {
            foreach (KeyValuePair<string, SpriteData> kvp in GFX.SpriteBank.SpriteData)
            {
                if (!kvp.Key.StartsWith("hateline_"))
                {
                    continue;
                }
                if (kvp.Key.Replace("hateline_", "") != "none")
                {
                    uiHats.Add(kvp.Key.Replace("hateline_", ""));
                    HatelineModule.hats = uiHats.Distinct().ToList();
                }
            }

            HatSelector(menu, inGame);
        }

        public void HatSelector(TextMenu menu, bool inGame)
        {
            var hatSelectionMenu = new TextMenu.Option<string>(Dialog.Clean("HATELINE_SETTINGS_CURHAT"));

            hatSelectionMenu.Add(Dialog.Clean("HATELINE_SETTINGS_DEFAULT"), HatelineModule.DEFAULT, true);

            foreach (string hat in HatelineModule.hats)
            {
                bool selected = (hat == HatelineModule.Settings.SelectedHat);
                string name = Dialog.Clean("hateline_hat_" + hat);
                name = (name == "") ? hat : name;
                hatSelectionMenu.Add(name, hat, selected);
            }

            hatSelectionMenu.Change(SelectedHat => HatComponent.ReloadHat(SelectedHat, inGame, HatelineModule.Settings.CrownX, HatelineModule.Settings.CrownY));

            if (inGame)
            {

                // hatSelectionMenu.AddDescription(menu, "You may have to die to reload hat");
                // completely redundant now that i fixed the reload system!

                Player player = (Engine.Scene)?.Tracker.GetEntity<Player>();
                if (player != null && player.StateMachine.State == Player.StIntroWakeUp)
                {
                    hatSelectionMenu.Disabled = true;
                }
            }

            menu.Add(hatSelectionMenu);
        }
    }
}
