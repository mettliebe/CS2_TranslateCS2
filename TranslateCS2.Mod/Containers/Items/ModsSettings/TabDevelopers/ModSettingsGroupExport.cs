using System;
using System.Collections.Generic;
using System.IO;

using Colossal.IO.AssetDatabase;
using Colossal.Json;

using Game.Settings;
using Game.UI.Widgets;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;
using TranslateCS2.Mod.Containers.Items.Unitys;

using TranslateCS2.Mod.Helpers;

namespace TranslateCS2.Mod.Containers.Items;
internal partial class ModSettings {



    public const string ExportGroup = nameof(ExportGroup);



    [Exclude]
    [SettingsUIDeveloper]
    [SettingsUISection(TabDevelopers, ExportGroup)]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetExportDropDownItems))]
    public string ExportDropDown { get; set; }

    [MyExcludeFromCoverage]
    private DropdownItem<string>[] GetExportDropDownItems() {
        List<DropdownItem<string>> items = [];
        items.Add(new DropdownItem<string>() {
            value = StringConstants.All,
            displayName = StringConstants.All
        });
        LocaleAssetProvider? localeAssetProvider = this.runtimeContainer.BuiltInLocaleIdProvider as LocaleAssetProvider;
        IEnumerable<LocaleAsset> localeAssets = localeAssetProvider.GetLocaleAssets();
        foreach (LocaleAsset localeAsset in localeAssets) {
            items.Add(new DropdownItem<string>() {
                value = localeAsset.localeId,
                displayName = localeAsset.localizedName
            });
        }
        return items.ToArray();
    }

    [Exclude]
    [SettingsUIDeveloper]
    [SettingsUISection(TabDevelopers, ExportGroup)]
    [SettingsUIDirectoryPicker]
    public string ExportDirectory { get; set; }



    [Exclude]
    [SettingsUIDeveloper]
    [SettingsUISection(TabDevelopers, ExportGroup)]
    [SettingsUIButton]
    [SettingsUIConfirmation]
    [MyExcludeFromCoverage]
    public bool ExportButton {
        set {
            try {
                LocaleAssetProvider? localeAssetProvider = this.runtimeContainer.BuiltInLocaleIdProvider as LocaleAssetProvider;
                if (StringConstants.All.Equals(this.ExportDropDown, StringComparison.OrdinalIgnoreCase)) {
                    IEnumerable<LocaleAsset> localeAssets = localeAssetProvider.GetLocaleAssets();
                    foreach (LocaleAsset localeAsset in localeAssets) {
                        this.ExportEntries(localeAsset);
                    }
                } else {
                    LocaleAsset localeAsset = localeAssetProvider.Get(this.ExportDropDown);
                    this.ExportEntries(localeAsset);
                }
            } catch (Exception ex) {
                this.runtimeContainer.ErrorMessages.DisplayErrorMessageFailedExportBuiltIn(this.ExportDirectory);
                this.runtimeContainer.Logger.LogError(this.GetType(),
                                                      LoggingConstants.FailedTo,
                                                      [nameof(this.ExportButton), ex]);
            }
        }
    }
    [MyExcludeFromCoverage]
    private void ExportEntries(LocaleAsset asset) {
        Dictionary<string, string> entries = asset.data.entries;
        string path = Path.Combine(this.ExportDirectory,
                                   $"{asset.localeId}{ModConstants.JsonExtension}");
        JsonHelper.Write(entries, path);
    }
}
