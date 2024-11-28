using System;
using System.Collections.Generic;
using System.Linq;

using Colossal;
using Colossal.IO.AssetDatabase;
using Colossal.Serialization.Entities;

using Game;
using Game.SceneFlow;
using Game.UI.Menu;
using Game.UI.Widgets;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items.Unitys;
using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod.Systems;
internal partial class MyExportTypeCollectorSystem : GameSystemBase {


    public static MyExportTypeCollectorSystem? INSTANCE { get; private set; }


    private bool collected = false;

    private IModRuntimeContainer? runtimeContainer;
    private LocaleAssetProvider? localeAssetProvider;
    private LocManagerProvider? locManagerProvider;

    private GameManager.Configuration? gameConfiguration;

    private MyNotificator? notificator;

    public IOrderedEnumerable<DropdownItem<string>>? ExportTypeDropDownItems { get; private set; }



    protected override void OnCreate() {
        base.OnCreate();
        MyExportTypeCollectorSystem.INSTANCE = this;
        this.runtimeContainer = Mod.RuntimeContainer;
        this.localeAssetProvider = this.runtimeContainer?.BuiltInLocaleIdProvider as LocaleAssetProvider;
        this.locManagerProvider = this.runtimeContainer?.LocManager.Provider as LocManagerProvider;
        this.gameConfiguration = GameManager.instance.configuration;
        if (false) {
            // INFO: at this very moment, i see no need to show notifications
            NotificationUISystem uiNotificator = this.World.GetOrCreateSystemManaged<NotificationUISystem>();
            this.notificator = new MyNotificator($"{ModConstants.Name}.{nameof(MyExportTypeCollectorSystem)}",
                                                 $"{ModConstants.Name}",
                                                 15f,
                                                 uiNotificator);
        }
    }

    protected override void OnUpdate() {
        // that happens too often
    }

    protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode) {
        base.OnGameLoadingComplete(purpose, mode);
        if (this.gameConfiguration is null) {
            // no gameConfiguration = no collecting
            return;
        } else if (!this.gameConfiguration.developerMode) {
            // only needs to be collected for devs
            return;
        } else if (this.runtimeContainer is null) {
            // no runtimeContainer = no collecting
            return;
        } else if (this.localeAssetProvider is null) {
            // no localeAssetProvider = no collecting
            return;
        } else if (this.locManagerProvider is null) {
            // no locManagerProvider = no collecting
            return;
        }

        if (GameMode.MainMenu.Equals(mode)
            && !this.collected) {

            // only on main menu
            // and
            // only if its not collected

            this.collected = true;
            HashSet<string> added = [];
            List<DropdownItem<string>> exportDropDownItems = [];
            IList<MyLocaleInfo> localeInfos = this.locManagerProvider.GetLocaleInfos();
            int i = 0;
            // TODO: Notificator: text
            this.notificator?.Start("fang an un lot jon", localeInfos.Count);
            foreach (MyLocaleInfo localeInfo in localeInfos) {
                IList<IDictionarySource> sources = localeInfo.Sources;



                IList<IDictionarySource> localeAssets =
                    sources
                        .Where(item => typeof(LocaleAsset).IsAssignableFrom(item.GetType()))
                        .ToList();



                // database.name = Game
                IList<IDictionarySource> baseGameLocaleAssets =
                    localeAssets
                        .Where(item => LocaleAssetProvider.BuiltInBaseGamePredicate(item as LocaleAsset))
                        .ToList();



                {
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
                        if (asset.path.EndsWith(ModConstants.CokExtension)) {
                            name =
                                asset
                                    .path
                                    .Substring(asset.path.LastIndexOf(StringConstants.ForwardSlashChar) + 1)
                                    .Replace(ModConstants.CokExtension, String.Empty);
                        } else if (Int32.TryParse(id, out int idInt)) {
                            Colossal.PSI.Common.Mod? mod = OtherModsLocFilesHelper.GetModViaId(this.runtimeContainer, idInt);
                            if (mod is not null) {
                                Colossal.PSI.Common.Mod m = (Colossal.PSI.Common.Mod) mod;
                                name = m.displayName;
                            }
                        }
                        if (added.Contains(name)) {
                            continue;
                        }
                        added.Add(name);
                        exportDropDownItems.Add(DropDownItemsHelper.Create(id, name));
                    }
                }



                {
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
                        if (added.Contains(m.displayName)) {
                            continue;
                        }
                        added.Add(m.displayName);
                        exportDropDownItems.Add(DropDownItemsHelper.Create(m.displayName, m.displayName));
                    }
                }



                {
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
                        if (added.Contains(modName)) {
                            continue;
                        }
                        added.Add(modName);
                        exportDropDownItems.Add(DropDownItemsHelper.Create(modName, modName));
                    }
                }
                i++;
                // TODO: Notificator: text
                this.notificator?.Update("best de immer noch net färdisch? schwad disch net mööd, lot jonn", i);
            }
            // DropDownItem.displayName is a LocalizedString that's id is displayed/used!!!
            // LocalizedString itself does not implement IComparable
            this.ExportTypeDropDownItems = exportDropDownItems.OrderBy(item => item.displayName.id);
            // TODO: Notificator: text
            this.notificator?.Stop("jetz bön isch färdisch");
        }
    }
}
