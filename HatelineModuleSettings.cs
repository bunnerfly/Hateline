namespace Celeste.Mod.Hateline
{
	[SettingName("Hateline Settings")]
	public class HatelineModuleSettings : EverestModuleSettings
	{
		public bool AllowMapChanges { get; set; } = true;

		public bool Enabled { get; set; } = false;

		[SettingRange(-100, 100, false)]
		public int CrownX { get; set; } = 0;

		[SettingRange(-100, 100, false)]
		public int CrownY { get; set; } = -8;

		[SettingIgnore]
		public string SelectedHat { get; set; } = "none";
	}
}