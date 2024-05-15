using Celeste.Mod.Hateline.CelesteNet;

namespace Celeste.Mod.Hateline
{
    [SettingName("Hateline Settings")]
    public class HatelineModuleSettings : EverestModuleSettings
    {
        private bool _enabled = true;
        public bool Enabled {
            get {
                return _enabled;
            }
            set {
                if(_enabled != value && CelesteNetSupport.loaded)
                {
                    // as a notification for other clients of either current hat, or force none when disabled
                    CelesteNetSupport.CNetComponent?.SendPlayerHat(value ? null : HatelineModule.DEFAULT);
                }
                _enabled = value;
            }
        }

        public bool AllowMapChanges { get; set; } = true;

        [SettingRange(-100, 100, false)]
        public int CrownX { get; set; } = 0;

        [SettingRange(-100, 100, false)]
        public int CrownY { get; set; } = -8;

        [SettingIgnore]
        public string SelectedHat { get; set; } = HatelineModule.DEFAULT;
    }
}