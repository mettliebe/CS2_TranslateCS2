using Colossal.Json;

using Game.Settings;

namespace TranslateCS2.Containers.Items;
internal partial class ModSettings {



    public const string SettingsGroup = nameof(SettingsGroup);



    [Include]
    [SettingsUISection(TabSettings, SettingsGroup)]
    public bool LoadFromOtherMods { get; set; }
}
