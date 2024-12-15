using Colossal.IO.AssetDatabase;

using TranslateCS2.Interfaces;

namespace TranslateCS2.Containers.Items.Unitys;
internal class SettingsSaver : ISettingsSaver {
    private readonly AssetDatabase assetDatabase;
    public SettingsSaver(AssetDatabase assetDatabase) {
        this.assetDatabase = assetDatabase;
    }
    public void SaveSettings() {
        this.assetDatabase.SaveSettings();
    }
}
