using System;
using System.Collections.Generic;
using System.Linq;

using Colossal;
using Colossal.IO.AssetDatabase;
using Colossal.Serialization.Entities;

using Game;
using Game.UI.Widgets;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items.Unitys;
using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Interfaces;
using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod.Services.Exports.Strategys;
internal class ExportServiceStrategy : AExportServiceStrategy, IExportServiceStrategy {
    private readonly IModRuntimeContainer runtimeContainer;
    private readonly LocaleAssetProvider localeAssetProvider;
    private readonly LocManagerProvider locManagerProvider;


    public ExportServiceStrategy(IModRuntimeContainer runtimeContainer) {
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
        this.AppendExportTypeDropDownItems(items);
        return items.ToArray();
    }

    private void AppendExportTypeDropDownItems(List<DropdownItem<string>> items) {
        //this.Refresh();
        items.AddRange(this.runtimeContainer.ExportTypeDropDownItems.Items);
    }

    public override void Export(string localeId,
                                string type,
                                string directory) {
        try {
            ISet<string> builtInLocaleIds = this.localeAssetProvider.GetBuiltInLocaleIds().ToHashSet();
            MyExportTypeDropDownItems? exportTypeDropDownItems = this.runtimeContainer?.ExportTypeDropDownItems;
            if (exportTypeDropDownItems is null) {
                return;
            }
            foreach (MyExportTypeDropDownItem dropDownItem in exportTypeDropDownItems.Items) {
                if (!StringConstants.All.Equals(type)
                    && !type.Equals(dropDownItem.Value)) {
                    continue;
                }
                IDictionary<string, MyLocaleInfo> localeInfos = dropDownItem.LocaleInfos;
                foreach (MyLocaleInfo localeInfo in localeInfos.Values) {
                    if (!StringConstants.All.Equals(localeId)
                        && !localeId.Equals(localeInfo.Id)) {
                        continue;
                    }
                    if (!builtInLocaleIds.Contains(localeInfo.Id)) {
                        continue;
                    }
                    IDictionary<string, string> exportEntries = this.GetExportEntries(localeInfo);
                    // localeid-param has to be localeInfo.id
                    // type-param has to be dropdownitem.displayname
                    // => one json per localeid and type/mod/asset
                    this.WriteEntries(exportEntries,
                                      localeInfo.Id,
                                      dropDownItem.DisplayName,
                                      directory);
                }
            }
        } catch (Exception ex) {
            this.runtimeContainer.ErrorMessages.DisplayErrorMessageFailedExportBuiltIn(directory);
            this.runtimeContainer.Logger.LogError(this.GetType(),
                                                  LoggingConstants.FailedTo,
                                                  [nameof(this.Export), ex]);
        }
    }

    public override void Refresh() {
        IList<IMySystemCollector>? collectors = this.runtimeContainer?.SystemCollectors;
        if (collectors is not null) {
            this.runtimeContainer?.ExportTypeDropDownItems.ClearForRefresh();
            foreach (IMySystemCollector collector in collectors) {
                collector.TryToCollect(Purpose.Cleanup, GameMode.MainMenu, true);
            }
        }
    }

    private IDictionary<string, string> GetExportEntries(MyLocaleInfo localeInfo) {
        IEnumerable<IDictionarySource> sources = localeInfo.Sources;
        Dictionary<string, string> exportEntries = [];
        foreach (IDictionarySource source in sources) {
            IEnumerable<KeyValuePair<string, string>> entries = source.ReadEntries([], []);
            foreach (KeyValuePair<string, string> entry in entries) {
                if (exportEntries.ContainsKey(entry.Key)) {
                    continue;
                }
                exportEntries[entry.Key] = entry.Value;
            }
        }
        IList<LocaleData> localeDatas = localeInfo.LocaleDatas;
        foreach (LocaleData localeData in localeDatas) {
            foreach (KeyValuePair<string, string> entry in localeData.entries) {
                if (exportEntries.ContainsKey(entry.Key)) {
                    continue;
                }
                exportEntries[entry.Key] = entry.Value;
            }
        }
        return exportEntries;
    }
}
