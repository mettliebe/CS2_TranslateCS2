using Colossal.IO.AssetDatabase;

using TranslateCS2.Mod.Interfaces;

namespace TranslateCS2.Mod.Containers.Items.Unitys;
internal class SettingsSaver : ISettingsSaver {
    private readonly AssetDatabase assetDatabase;
    public SettingsSaver(AssetDatabase assetDatabase) {
        this.assetDatabase = assetDatabase;
    }
    public void SaveSettings() {
        this.assetDatabase.SaveSettings();
    }
}
