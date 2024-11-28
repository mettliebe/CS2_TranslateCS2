using System;
using System.Collections.Generic;
using System.Linq;

using Colossal.IO.AssetDatabase;

using Game.UI.Widgets;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items.Unitys;
using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Systems;

namespace TranslateCS2.Mod.Services.Exports;
internal class ExportServiceDictionarySourceStrategy : AExportServiceStrategy, IExportServiceStrategy {
    private readonly IModRuntimeContainer runtimeContainer;
    private readonly LocaleAssetProvider localeAssetProvider;
    private readonly LocManagerProvider locManagerProvider;


    public ExportServiceDictionarySourceStrategy(IModRuntimeContainer runtimeContainer) {
        this.runtimeContainer = runtimeContainer;
        this.localeAssetProvider = this.runtimeContainer.BuiltInLocaleIdProvider as LocaleAssetProvider;
        this.locManagerProvider = this.runtimeContainer.LocManager.Provider as LocManagerProvider;
    }


    public override DropdownItem<string>[] GetExportDropDownItems() {
        List<DropdownItem<string>> items = [];
        DropDownItemsHelper.AppendAllEntry(items);
        IEnumerable<LocaleAsset> localeAssets = this.localeAssetProvider.GetBuiltInBaseGameLocaleAssets();
        foreach (LocaleAsset localeAsset in localeAssets) {
            items.Add(new DropdownItem<string>() {
                value = localeAsset.localeId,
                displayName = localeAsset.localizedName
            });
        }
        return items.ToArray();
    }

    public override DropdownItem<string>[] GetExportTypeDropDownItems() {
        List<DropdownItem<string>> items = [];
        DropDownItemsHelper.AppendAllEntry(items);
        DropDownItemsHelper.AppendGameEntry(items);
        this.AppendExportTypeDropDownItems(items);
        return items.ToArray();
    }

    private void AppendExportTypeDropDownItems(List<DropdownItem<string>> items) {
        IOrderedEnumerable<DropdownItem<string>>? exportTypeDropDownItems = MyExportTypeCollectorSystem.INSTANCE?.ExportTypeDropDownItems;
        if (exportTypeDropDownItems is not null) {
            items.AddRange(exportTypeDropDownItems);
        }

    }

    public override void Export(string localeId,
                                string type,
                                string directory) {
        try {
            // TODO:
        } catch (Exception ex) {
            this.runtimeContainer.ErrorMessages.DisplayErrorMessageFailedExportBuiltIn(directory);
            this.runtimeContainer.Logger.LogError(this.GetType(),
                                                  LoggingConstants.FailedTo,
                                                  [nameof(this.Export), ex]);
        }
    }


    private IDictionary<string, string> GetExportEntries(IEnumerable<Dictionary<string, string>>? localizations,
                                                         string localeId) {
        Dictionary<string, string> exportEntries = [];
        foreach (Dictionary<string, string> entries in localizations) {
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
