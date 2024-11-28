using System;
using System.Collections.Generic;
using System.Linq;

using Colossal;
using Colossal.IO.AssetDatabase;

using Game.UI.Widgets;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items.Unitys;
using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Models;

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
        // TODO:
        IList<MyLocaleInfo> localeInfos = this.locManagerProvider.GetLocaleInfos();
        foreach (MyLocaleInfo localeInfo in localeInfos) {
            IList<IDictionarySource> sources = localeInfo.Sources;



            IList<IDictionarySource> localeAssets =
                sources
                    .Where(item => typeof(LocaleAsset).IsAssignableFrom(item.GetType()))
                    .ToList();



            {
                // database.name = Game
                IList<IDictionarySource> baseGameLocaleAssets =
                localeAssets
                    .Where(item => LocaleAssetProvider.BuiltInBaseGamePredicate(item as LocaleAsset))
                    .ToList();
            }



            {
                // database.name = ParadoxMods
                IList<IDictionarySource> onlineLocaleAssets =
                localeAssets
                    .Where(item => LocaleAssetProvider.ParadoxModsPredicate(item as LocaleAsset))
                    .ToList();
                // id can be extracted from subpath
                // try to get name from modmanager?
                // in case of .cok-files the name can be extracted from path (for region packs for example)
            }



            {
                // database.name = User
                IList<IDictionarySource> localLocaleAssets =
                localeAssets
                    .Where(item => LocaleAssetProvider.UserModsPredicate(item as LocaleAsset))
                    .ToList();
                // do not have an id
                // see ExportServiceAssetStrategy.HandleExportTypeDropDownItemsForUserMods
            }



            {
                // all dictionary sources other than localeAsset
                // belong to code-mods
                IList<IDictionarySource> mods =
                sources
                    .Except(localeAssets)
                    .ToList();
                // online: should have an id, name can be gathered from modmanager
                // local: do not have an id, extract name somehow
                // or just work with the name as follows?



                IList<string> modNamesTest =
                    mods
                        .Select(s => s.GetType().Assembly.ManifestModule.ScopeName.Replace(".dll", String.Empty))
                        .Distinct()
                        .ToList();
            }

            // breakpoint
            int i = 0;
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
