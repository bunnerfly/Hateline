namespace Celeste.Mod.Hateline
{
	[SettingName("Hateline Settings")]
	public class HatelineModuleSettings : EverestModuleSettings
	{
		public bool Enabled { get; set; } = true;

		public bool AllowMapChanges { get; set; } = true;

		[SettingRange(-100, 100, false)]
		public int CrownX { get; set; } = 0;

		[SettingRange(-100, 100, false)]
		public int CrownY { get; set; } = -8;

		[SettingIgnore]
		public string SelectedHat { get; set; } = "none";
	}
}