using System;
using System.Collections.Generic;
using System.Linq;

using Colossal;
using Colossal.IO.AssetDatabase;
using Colossal.Serialization.Entities;

using Game;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Containers.Items.Unitys;
using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod.Services.Exports.Collectors;
/// <summary>
///     uses the <see cref="Game"/>s
///     <br/>
///     <see cref="Colossal.Localization.LocalizationManager"/>
///     <br/>
///     through
///     <br/>
///     <see cref="LocManagerProvider"/>
///     <br/>
///     and its <see cref="Game.Modding.ModManager"/>
///     <br/>
///     through
///     <br/>
///     <see cref="OtherModsLocFilesHelper"/>
///     <br/>
///     to gather information and localizations for already loaded
///     <br/>
///     <see cref="Game.Modding.IMod"/>s
///     <br/>
///     and
///     <br/>
///     <see cref="LocaleAsset"/>s
/// </summary>
[MyExcludeFromCoverage]
internal class ExportTypeDictionarySourceCollector : AExportTypeCollector {

    public ExportTypeDictionarySourceCollector(IModRuntimeContainer runtimeContainer) : base(runtimeContainer) { }

    public override void TryToCollect(Purpose purpose, GameMode mode, bool bypassExecutionChecks) {
        if (this.HasToBeExecutedNot(purpose, mode)
            && !bypassExecutionChecks) {
            return;
        }
        this.collected = true;
        IList<MyLocaleInfo> localeInfos = this.locManagerProvider.GetLocaleInfos();
        foreach (MyLocaleInfo localeInfo in localeInfos) {
            IList<IDictionarySource> sources = localeInfo.Sources;
            {
                // TODO: flavor-specific export?
                sources =
                    sources
                        //
                        // INFO: filter out flavors, for now; cause it is possible to add an en-US.json
                        //       dictionary sources are 'only' used for code-mods
                        //       base game and co's packs (at least region packs) are exported via localeasset
                        //       if someone loads a translation for a code-mod, for now it has to be filtered out
                        //       otherwise, the person would export the provided translations, instead of the 'mod-owners' ones
                        //
                        // item that cannot be assigned TO Flavor!
                        .Where(item => !typeof(Flavor).IsAssignableFrom(item.GetType()))
                        //
                        // INFO: Export Uncategorized Mods - see other occurances of this Tag
                        // INFO: some use use the MemorySource-class to provide their localizations
                        //       in those cases, its 'impossible' to assign the localizations to a speccific Mod
                        //
                        // item that cannot be assigned TO MemorySource!
                        //.Where(item => !typeof(MemorySource).IsAssignableFrom(item.GetType()))
                        .ToList();
            }


            IList<IDictionarySource> localeAssets = GetLocaleAssetsFromDictionarySources(sources);
            this.CollectBaseGame(localeInfo.Id,
                                 localeAssets);
            this.CollectParadoxAssetMods(localeInfo.Id,
                                         localeAssets);
            this.CollectLocalAssetMods(localeInfo.Id,
                                       localeAssets);
            this.CollectCodeMods(localeInfo.Id,
                                 sources,
                                 localeAssets);
        }
    }

    private void CollectBaseGame(string localeId,
                                 IList<IDictionarySource> localeAssets) {
        IList<IDictionarySource> sources = GetBaseGameLocaleAssetsFromDictionarySources(localeAssets);
        foreach (IDictionarySource source in sources) {
            MyExportTypeDropDownItem item = MyExportTypeDropDownItem.Create(StringConstants.Game,
                                                                            StringConstants.Game,
                                                                            true,
                                                                            true);
            this.ExportTypeDropDownItems.AddDropDownItem(localeId,
                                                         item,
                                                         source);
        }
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

    private void CollectCodeMods(string localeId,
                                 IList<IDictionarySource> sources,
                                 IList<IDictionarySource> localeAssets) {
        // all dictionary sources other than localeAsset
        // belong to code-mods
        // online: should have an id, name can be gathered from modmanager
        // local: do not have an id, extract name somehow
        // just work with the name as follows,
        // its more convenient to filter within the export-method,
        // instead of a to and fro
        IList<IDictionarySource> modSources =
            sources
                .Except(localeAssets)
                .ToList();
        foreach (IDictionarySource modSource in modSources) {
            string modName = modSource.GetType().Assembly.ManifestModule.ScopeName.Replace(ModConstants.DllExtension, String.Empty);
            MyExportTypeDropDownItem item = MyExportTypeDropDownItem.Create(modName,
                                                                            modName);
            // this is the dictionary source collector!!!
            this.ExportTypeDropDownItems.AddDropDownItem(localeId,
                                                     item,
                                                     modSource);
        }
    }

    private void CollectLocalAssetMods(string localeId,
                                       IList<IDictionarySource> localeAssets) {
        // database.name = User
        // do not have an id
        // see ExportServiceAssetStrategy.HandleExportTypeDropDownItemsForUserMods
        IList<IDictionarySource> sources =
            localeAssets
                .Where(item => LocaleAssetProvider.UserModsPredicate(item as LocaleAsset))
                .ToList();
        foreach (IDictionarySource source in sources) {
            if (sources is not LocaleAsset asset) {
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
            // this is the dictionary source collector!!!
            this.ExportTypeDropDownItems.AddDropDownItem(localeId,
                                                     item,
                                                     source);
        }
    }

    private void CollectParadoxAssetMods(string localeId,
                                         IList<IDictionarySource> localeAssets) {
        // database.name = ParadoxMods
        // id can be extracted from subpath
        // try to get name from modmanager?
        // in case of .cok-files the name can be extracted from path (for region packs for example)
        IList<IDictionarySource> sources =
            localeAssets
                .Where(item => LocaleAssetProvider.ParadoxModsPredicate(item as LocaleAsset))
                .ToList();
        foreach (IDictionarySource source in sources) {
            if (source is not LocaleAsset asset) {
                continue;
            }
            string? id = OtherModsLocFilesHelper.GetIdFromAssetSubPath(asset);
            if (id is null) {
                continue;
            }
            string name = id;
            bool isColossalOrdersOne = false;
            if (asset.path.EndsWith(ModConstants.CokExtension)) {
                // TODO: cok-extension
                // INFO: for now, i, the author of this mod, assume, that only colossal order is 'able' to pack cok-files; an i keep an eye on that
                name =
                    asset
                        .path
                        .Substring(asset.path.LastIndexOf(StringConstants.ForwardSlashChar) + 1)
                        .Replace(ModConstants.CokExtension, String.Empty)
                        .Replace(StringConstants.Space, String.Empty);
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
                                                                            false,
                                                                            isColossalOrdersOne);
            // this is the dictionary source collector!!!
            this.ExportTypeDropDownItems.AddDropDownItem(localeId,
                                                     item,
                                                     source);
        }
    }
}
