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
using TranslateCS2.Mod.Packs;

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
        LocaleAssetProvider localeAssetProvider = this.runtimeContainer.BuiltInLocaleIdProvider as LocaleAssetProvider;
        if (localeAssetProvider is not null) {
            this.HandleExportTypeDropDownItemsForOnlineMods(items,
                                                            localeAssetProvider);
            this.HandleExportTypeDropDownItemsForUserMods(items,
                                                          localeAssetProvider);
        }
        return items.ToArray();
    }

    private void HandleExportTypeDropDownItemsForUserMods(List<DropdownItem<string>> items,
                                                          LocaleAssetProvider localeAssetProvider) {
        IEnumerable<LocaleAsset>? modAssets = localeAssetProvider.GetUserModsLocaleAssets();
        if (modAssets is not null) {
            List<Colossal.PSI.Common.Mod> mods = [];
            IEnumerable<string> modNames = modAssets.Select(OtherModsLocFilesHelper.GetNameFromAssetSubPath).Distinct();
            foreach (string modName in modNames) {
                Colossal.PSI.Common.Mod? mod = OtherModsLocFilesHelper.GetModViaName(this.runtimeContainer, modName);
                if (mod is null) {
                    continue;
                }
                mods.Add((Colossal.PSI.Common.Mod) mod);
            }
            IEnumerable<Colossal.PSI.Common.Mod> orderedMods = mods.OrderBy(mod => mod.displayName);
            foreach (Colossal.PSI.Common.Mod orderedMod in orderedMods) {
                // Local-/User-Mods do not have an ID, so their display name (technical name) is used
                items.Add(new DropdownItem<string>() {
                    value = orderedMod.displayName,
                    displayName = orderedMod.displayName
                });
            }
        }
    }

    private void HandleExportTypeDropDownItemsForOnlineMods(List<DropdownItem<string>> items,
                                                            LocaleAssetProvider localeAssetProvider) {
        IEnumerable<LocaleAsset>? modAssets = localeAssetProvider.GetParadoxModsLocaleAssets();
        if (localeAssetProvider.HasFrenchPack(modAssets)) {
            items.Add(RegionPack.FrenchDropDownItem());
        }
        if (localeAssetProvider.HasGermanPack(modAssets)) {
            items.Add(RegionPack.GermanDropDownItem());
        }
        if (localeAssetProvider.HasUKPack(modAssets)) {
            items.Add(RegionPack.UKDropDownItem());
        }
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
        } else if (!Int32.TryParse(this.ExportTypeDropDown, out _)) {
            // export drop down could not be parsed to an integer
            // since All and Game are handled before
            // it has to be a local mod with its technical name as dropdownitems value
            return
                localeAssetsToExport
                    .Where(LocaleAssetProvider.UserModsPredicate)
                    .Where(asset => {
                        string? name = OtherModsLocFilesHelper.GetNameFromAssetSubPath(asset);
                        return this.ExportTypeDropDown.Equals(name);
                    });
        }
        return
            localeAssetsToExport
                .Where(LocaleAssetProvider.ParadoxModsPredicate)
                .Where(asset => {
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
                                   $"{localeId}_{this.ExportTypeDropDown}{ModConstants.JsonExtension}");
        JsonHelper.Write(exportEntries, path);

    }
}
