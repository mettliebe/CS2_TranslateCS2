using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        IEnumerable<LocaleAsset> localeAssets = localeAssetProvider.GetBuiltInBaseGameLocaleAssets();
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
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetExportTypeDropDownItems))]
    public string ExportTypeDropDown { get; set; } = StringConstants.All;

    [MyExcludeFromCoverage]
    private DropdownItem<string>[] GetExportTypeDropDownItems() {
        List<DropdownItem<string>> items = [];
        items.Add(new DropdownItem<string>() {
            value = StringConstants.All,
            displayName = StringConstants.All
        });
        items.Add(new DropdownItem<string>() {
            value = StringConstants.Game,
            displayName = StringConstants.Game
        });
        // TODO: local mods id!!!
        LocaleAssetProvider? localeAssetProvider = this.runtimeContainer.BuiltInLocaleIdProvider as LocaleAssetProvider;
        if (localeAssetProvider is not null) {
            try {
                IEnumerable<LocaleAsset>? modAssets = localeAssetProvider?.GetParadoxModsLocaleAssets();
                if (modAssets is not null) {
                    List<Colossal.PSI.Common.Mod> mods = [];
                    IEnumerable<string> modIds = modAssets.Select(OtherModsLocFilesHelper.GetIdFromAssetSubPath).Distinct();
                    foreach (string modId in modIds) {
                        Colossal.PSI.Common.Mod? mod = OtherModsLocFilesHelper.GetModViaId(this.runtimeContainer, Int32.Parse(modId));
                        if (mod is null) {
                            continue;
                        }
                        mods.Add((Colossal.PSI.Common.Mod) mod);
                    }
                    IOrderedEnumerable<Colossal.PSI.Common.Mod> orderedMods = mods.OrderBy(mod => mod.displayName);
                    foreach (Colossal.PSI.Common.Mod orderedMod in orderedMods) {
                        items.Add(new DropdownItem<string>() {
                            value = orderedMod.id.ToString(),
                            displayName = orderedMod.displayName
                        });
                    }

                }
            } catch (Exception ex) {
                int xxx = 0;
            }
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
                    IReadOnlyList<string>? builtInLocaleIds = localeAssetProvider?.GetBuiltInLocaleIds();
                    foreach (string builtInLocaleId in builtInLocaleIds) {
                        IEnumerable<LocaleAsset>? localeAssetsToExport = localeAssetProvider?.Get(builtInLocaleId);
                        IEnumerable<LocaleAsset>? filteredLocaleAssetsToExport = this.FilterLocaleAssetsToExport(localeAssetsToExport);
                        this.ExportEntries(filteredLocaleAssetsToExport,
                                           builtInLocaleId);
                    }
                } else {
                    IEnumerable<LocaleAsset>? localeAssetsToExport = localeAssetProvider?.Get(this.ExportDropDown);
                    IEnumerable<LocaleAsset>? filteredLocaleAssetsToExport = this.FilterLocaleAssetsToExport(localeAssetsToExport);
                    this.ExportEntries(filteredLocaleAssetsToExport,
                                       this.ExportDropDown);
                }
            } catch (Exception ex) {
                this.runtimeContainer.ErrorMessages.DisplayErrorMessageFailedExportBuiltIn(this.ExportDirectory);
                this.runtimeContainer.Logger.LogError(this.GetType(),
                                                      LoggingConstants.FailedTo,
                                                      [nameof(this.ExportButton), ex]);
            }
        }
    }

    private IEnumerable<LocaleAsset>? FilterLocaleAssetsToExport(IEnumerable<LocaleAsset>? localeAssetsToExport) {
        if (StringConstants.All.Equals(this.ExportTypeDropDown)) {
            return localeAssetsToExport;
        } else if (StringConstants.Game.Equals(this.ExportTypeDropDown)) {
            return localeAssetsToExport.Where(LocaleAssetProvider.BuiltInBaseGamePredicate);
        }
        // TODO: local mods id!!!
        return localeAssetsToExport.Where(asset => {
            string id = OtherModsLocFilesHelper.GetIdFromAssetSubPath(asset);
            return this.ExportTypeDropDown.Equals(id);
        });
    }

    [MyExcludeFromCoverage]
    private void ExportEntries(IEnumerable<LocaleAsset>? assets,
                               string localeId) {
        Dictionary<string, string> exportEntries = [];
        foreach (LocaleAsset asset in assets) {
            Dictionary<string, string> entries = asset.data.entries;
            foreach (KeyValuePair<string, string> entry in entries) {
                if (exportEntries.ContainsKey(entry.Key)) {
                    continue;
                }
                exportEntries[entry.Key] = entry.Value;
            }
        }
        string path = Path.Combine(this.ExportDirectory,
                                   $"{localeId}{ModConstants.JsonExtension}");
        JsonHelper.Write(exportEntries, path);

    }
}
