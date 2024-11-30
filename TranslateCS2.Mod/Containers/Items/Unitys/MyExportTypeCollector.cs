using System;
using System.Collections.Generic;
using System.Linq;

using Colossal;
using Colossal.IO.AssetDatabase;
using Colossal.Serialization.Entities;

using Game;
using Game.SceneFlow;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;
using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Interfaces;
using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod.Containers.Items.Unitys;
[MyExcludeFromCoverage]
internal class MyExportTypeCollector : IMyExportTypeCollector {
    private bool collected = false;

    private readonly IModRuntimeContainer? runtimeContainer;
    private readonly LocaleAssetProvider? localeAssetProvider;
    private readonly LocManagerProvider? locManagerProvider;

    private readonly GameManager.Configuration? gameConfiguration;


    private ISet<MyExportTypeDropDownItem> _ExportTypeDropDownItems { get; } = new HashSet<MyExportTypeDropDownItem>();
    public IOrderedEnumerable<MyExportTypeDropDownItem> ExportTypeDropDownItems =>
        this._ExportTypeDropDownItems
            .OrderByDescending(item => item.IsBaseGame)
            .ThenByDescending(item => item.IsColossalOrdersOne)
            .ThenBy(item => item.DisplayName);


    public MyExportTypeCollector(IModRuntimeContainer runtimeContainer) {
        this.runtimeContainer = runtimeContainer;
        this.localeAssetProvider = this.runtimeContainer?.BuiltInLocaleIdProvider as LocaleAssetProvider;
        this.locManagerProvider = this.runtimeContainer?.LocManager.Provider as LocManagerProvider;
        this.gameConfiguration = GameManager.instance.configuration;
    }

    public void CollectIfPossible(Purpose purpose, GameMode mode) {
        if (this.HasToBeExecutedNot(purpose, mode)) {
            return;
        }
        this.collected = true;
        IList<MyLocaleInfo> localeInfos = this.locManagerProvider.GetLocaleInfos();
        foreach (MyLocaleInfo localeInfo in localeInfos) {
            IList<IDictionarySource> sources = localeInfo.Sources;
            int size = sources.Count;
            {
                // TODO:
                // INFO: filter out flavors, for now; cause it is possible to add an en-US.json
                //       dictionary sources are 'only' used for code-mods
                //       base game and co's packs (at least region packs) are exported via localeasset
                //       if someone loads a translation for a code-mod, for now it has to be filtered out
                //       otherwise, the person would export the provided translations, instead of the 'mod-owners' ones
                sources =
                    sources
                        .Where(item => !typeof(Flavor).IsAssignableFrom(item.GetType()))
                        .ToList();
                size = sources.Count;
            }


            IList<IDictionarySource> localeAssets = GetLocaleAssetsFromDictionarySources(sources);
            // TODO: each DropDownItem has to have its localeinfo related sources to not obtain and filter them again for export
            this.CollectBaseGame(localeAssets);
            this.CollectParadoxAssetMods(localeAssets);
            this.CollectLocalAssetMods(localeAssets);
            this.CollectCodeMods(sources,
                                 localeAssets);
        }
    }

    private void CollectBaseGame(IList<IDictionarySource> localeAssets) {
        IList<IDictionarySource> baseGameLocaleAssets = GetBaseGameLocaleAssetsFromDictionarySources(localeAssets);
        MyExportTypeDropDownItem item = MyExportTypeDropDownItem.Create(StringConstants.Game,
                                                                        StringConstants.Game,
                                                                        true,
                                                                        true);
        this._ExportTypeDropDownItems.Add(item);
    }

    private static IList<IDictionarySource> GetBaseGameLocaleAssetsFromDictionarySources(IList<IDictionarySource> localeAssets) {
        // database.name = Game
        return localeAssets
                .Where(item => LocaleAssetProvider.BuiltInBaseGamePredicate(item as LocaleAsset))
                .ToList();
    }

    private static IList<IDictionarySource> GetLocaleAssetsFromDictionarySources(IList<IDictionarySource> sources) {
        return sources
                .Where(item => typeof(LocaleAsset).IsAssignableFrom(item.GetType()))
                .ToList();
    }

    private void CollectCodeMods(IList<IDictionarySource> sources, IList<IDictionarySource> localeAssets) {
        // all dictionary sources other than localeAsset
        // belong to code-mods
        // online: should have an id, name can be gathered from modmanager
        // local: do not have an id, extract name somehow
        // just work with the name as follows,
        // its more convenient to filter within the export-method,
        // instead of a to and fro
        IList<IDictionarySource> mods =
            sources
                .Except(localeAssets)
                .ToList();
        IList<string> modNames =
            mods
                .Select(s =>
                    s
                        .GetType()
                        .Assembly
                        .ManifestModule
                        .ScopeName
                        .Replace(ModConstants.DllExtension, String.Empty))
                .Distinct()
                .ToList();
        foreach (string modName in modNames) {
            MyExportTypeDropDownItem item = MyExportTypeDropDownItem.Create(modName,
                                                                            modName);
            this._ExportTypeDropDownItems.Add(item);
        }
    }

    private void CollectLocalAssetMods(IList<IDictionarySource> localeAssets) {
        // database.name = User
        // do not have an id
        // see ExportServiceAssetStrategy.HandleExportTypeDropDownItemsForUserMods
        IList<IDictionarySource> localLocaleAssets =
            localeAssets
                .Where(item => LocaleAssetProvider.UserModsPredicate(item as LocaleAsset))
                .ToList();
        foreach (IDictionarySource localLocaleAsset in localLocaleAssets) {
            if (localLocaleAssets is not LocaleAsset asset) {
                continue;
            }
            string? name = OtherModsLocFilesHelper.GetNameFromAssetSubPath(asset);
            if (name is null) {
                continue;
            }
            Colossal.PSI.Common.Mod? mod = OtherModsLocFilesHelper.GetModViaName(this.runtimeContainer, name);
            if (mod is null) {
                continue;
            }
            Colossal.PSI.Common.Mod m = (Colossal.PSI.Common.Mod) mod;
            MyExportTypeDropDownItem item = MyExportTypeDropDownItem.Create(m.displayName,
                                                                            m.displayName);
            this._ExportTypeDropDownItems.Add(item);
        }
    }

    private void CollectParadoxAssetMods(IList<IDictionarySource> localeAssets) {
        // database.name = ParadoxMods
        // id can be extracted from subpath
        // try to get name from modmanager?
        // in case of .cok-files the name can be extracted from path (for region packs for example)
        IList<IDictionarySource> onlineLocaleAssets =
            localeAssets
                .Where(item => LocaleAssetProvider.ParadoxModsPredicate(item as LocaleAsset))
                .ToList();
        foreach (IDictionarySource onlineLocaleAsset in onlineLocaleAssets) {
            if (onlineLocaleAsset is not LocaleAsset asset) {
                continue;
            }
            string id = OtherModsLocFilesHelper.GetIdFromAssetSubPath(asset);
            string name = id;
            bool isColossalOrdersOne = false;
            if (asset.path.EndsWith(ModConstants.CokExtension)) {
                name =
                    asset
                        .path
                        .Substring(asset.path.LastIndexOf(StringConstants.ForwardSlashChar) + 1)
                        .Replace(ModConstants.CokExtension, String.Empty);
                isColossalOrdersOne = true;
            } else if (Int32.TryParse(id, out int idInt)) {
                Colossal.PSI.Common.Mod? mod = OtherModsLocFilesHelper.GetModViaId(this.runtimeContainer, idInt);
                if (mod is not null) {
                    Colossal.PSI.Common.Mod m = (Colossal.PSI.Common.Mod) mod;
                    name = m.displayName;
                }
            }
            MyExportTypeDropDownItem item = MyExportTypeDropDownItem.Create(id,
                                                                            name,
                                                                            isColossalOrdersOne);
            this._ExportTypeDropDownItems.Add(item);
        }
    }

    /// <param name="purpose">
    ///     <see cref="Purpose"/>
    /// </param>
    /// <param name="mode">
    ///     <see cref="GameMode"/>
    /// </param>
    /// <returns>
    ///     <see langword="true"/>, if the collector should NOT be executed
    /// </returns>
    private bool HasToBeExecutedNot(Purpose purpose, GameMode mode) {
        if (!GameMode.MainMenu.Equals(mode)) {
            return true;
        } else if (this.collected) {
            return true;
        } else if (this.gameConfiguration is null) {
            // no gameConfiguration = no collecting
            return true;
        } else if (!this.gameConfiguration.developerMode
                   && !this.gameConfiguration.qaDeveloperMode
                   && !this.gameConfiguration.uiDeveloperMode) {
            // only needs to be collected for devs
            return true;
        } else if (this.runtimeContainer is null) {
            // no runtimeContainer = no collecting
            return true;
        } else if (this.localeAssetProvider is null) {
            // no localeAssetProvider = no collecting
            return true;
        } else if (this.locManagerProvider is null) {
            // no locManagerProvider = no collecting
            return true;
        }
        return false;
    }
}
