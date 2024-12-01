using System;
using System.Collections.Generic;
using System.Linq;

using Colossal.IO.AssetDatabase;

using Game.UI.Widgets;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items.Unitys;
using TranslateCS2.Mod.Helpers;

namespace TranslateCS2.Mod.Services.Exports;
internal class ExportServiceAssetStrategy : AExportServiceStrategy, IExportServiceStrategy {
    private readonly IModRuntimeContainer runtimeContainer;
    private readonly LocaleAssetProvider localeAssetProvider;


    public ExportServiceAssetStrategy(IModRuntimeContainer runtimeContainer) {
        this.runtimeContainer = runtimeContainer;
        this.localeAssetProvider = this.runtimeContainer.BuiltInLocaleIdProvider as LocaleAssetProvider;
    }


    public override DropdownItem<string>[] GetExportDropDownItems() {
        List<DropdownItem<string>> items = [];
        DropDownItemsHelper.AppendAllEntry(items);
        IEnumerable<LocaleAsset> localeAssets = this.localeAssetProvider.GetBuiltInBaseGameLocaleAssets();
        foreach (LocaleAsset localeAsset in localeAssets) {
            items.Add(DropDownItemsHelper.Create(localeAsset.localeId, localeAsset.localizedName));
        }
        return items.ToArray();
    }

    public override DropdownItem<string>[] GetExportTypeDropDownItems() {
        List<DropdownItem<string>> items = [];
        DropDownItemsHelper.AppendAllEntry(items);
        DropDownItemsHelper.AppendGameEntry(items);
        this.HandleExportTypeDropDownItemsForOnlineMods(items);
        this.HandleExportTypeDropDownItemsForUserMods(items);
        return items.ToArray();
    }

    private void HandleExportTypeDropDownItemsForUserMods(List<DropdownItem<string>> items) {
        IEnumerable<LocaleAsset>? modAssets = this.localeAssetProvider.GetUserModsLocaleAssets();
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
                items.Add(DropDownItemsHelper.Create(orderedMod.displayName, orderedMod.displayName));
            }
        }
    }

    private void HandleExportTypeDropDownItemsForOnlineMods(List<DropdownItem<string>> items) {
        IEnumerable<LocaleAsset>? modAssets = this.localeAssetProvider.GetParadoxModsLocaleAssets();
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
                items.Add(DropDownItemsHelper.Create(orderedMod.id.ToString(), orderedMod.displayName));
            }
        }
    }

    public override void Export(string localeId,
                                string type,
                                string directory) {
        try {
            if (StringConstants.All.Equals(localeId, StringComparison.OrdinalIgnoreCase)) {
                IReadOnlyList<string>? builtInLocaleIds = this.localeAssetProvider?.GetBuiltInLocaleIds();
                foreach (string builtInLocaleId in builtInLocaleIds) {
                    IEnumerable<LocaleAsset>? localeAssetsToExport = this.localeAssetProvider?.Get(builtInLocaleId);
                    IEnumerable<LocaleAsset>? filteredLocaleAssetsToExport = this.FilterLocaleAssetsToExport(localeAssetsToExport,
                                                                                                             type);
                    IDictionary<string, string> exportEntries = this.GetExportEntries(filteredLocaleAssetsToExport,
                                                                                      builtInLocaleId);
                    base.WriteEntries(exportEntries, localeId, type, directory);
                }
            } else {
                IEnumerable<LocaleAsset>? localeAssetsToExport = this.localeAssetProvider?.Get(localeId);
                IEnumerable<LocaleAsset>? filteredLocaleAssetsToExport = this.FilterLocaleAssetsToExport(localeAssetsToExport,
                                                                                                         type);
                IDictionary<string, string> exportEntries = this.GetExportEntries(filteredLocaleAssetsToExport,
                                                                                  localeId);
                base.WriteEntries(exportEntries, localeId, type, directory);
            }
        } catch (Exception ex) {
            this.runtimeContainer.ErrorMessages.DisplayErrorMessageFailedExportBuiltIn(directory);
            this.runtimeContainer.Logger.LogError(this.GetType(),
                                                  LoggingConstants.FailedTo,
                                                  [nameof(this.Export), ex]);
        }
    }

    private IEnumerable<LocaleAsset>? FilterLocaleAssetsToExport(IEnumerable<LocaleAsset>? localeAssetsToExport,
                                                                 string type) {
        if (StringConstants.All.Equals(type)) {
            return localeAssetsToExport;
        } else if (StringConstants.Game.Equals(type)) {
            return localeAssetsToExport.Where(LocaleAssetProvider.BuiltInBaseGamePredicate);
        } else if (!Int32.TryParse(type, out _)) {
            // export drop down could not be parsed to an integer
            // since All and Game are handled before
            // it has to be a local mod with its technical name as dropdownitems value
            return
                localeAssetsToExport
                    .Where(LocaleAssetProvider.UserModsPredicate)
                    .Where(asset => {
                        string? name = OtherModsLocFilesHelper.GetNameFromAssetSubPath(asset);
                        return type.Equals(name);
                    });
        }
        return
            localeAssetsToExport
                .Where(LocaleAssetProvider.ParadoxModsPredicate)
                .Where(asset => {
                    string id = OtherModsLocFilesHelper.GetIdFromAssetSubPath(asset);
                    return type.Equals(id);
                });
    }


    private IDictionary<string, string> GetExportEntries(IEnumerable<LocaleAsset>? assets,
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
        return exportEntries;
    }
}
