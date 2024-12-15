using System;
using System.Text;

using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.PSI.Environment;

using Game;
using Game.Modding;
using Game.SceneFlow;

using TranslateCS2.Consts;
using TranslateCS2.Containers;
using TranslateCS2.Containers.Items;
using TranslateCS2.Containers.Items.ModsSettings;
using TranslateCS2.Containers.Items.Unitys;
using TranslateCS2.Interfaces;
using TranslateCS2.Loggers;
using TranslateCS2.Models;
using TranslateCS2.Models.Localizations;
using TranslateCS2.Services.Exports.Collectors;

namespace TranslateCS2;
/// <summary>
///     dont move this <see langword="class" /> somewhere else
///     <br/>
///     dont rename this <see langword="class" />
///     <br/>
///     <br/>
///     its <see langword="namespace" /> and name are used to build an id for the <see cref="ModSettingsLocales"/>
///     <br/>
///     <seealso cref="Game.Modding.ModSetting.ModSetting(IMod)"/>
/// </summary>
public class Mod : IMod {
    private static readonly ILog Logger = LogManager.GetLogger(ModConstants.NameSimple).SetShowsErrorsInUI(false);

    /// <summary>
    ///     never ever turn to <see langword="true"/>!!!
    ///     <br/>
    ///     <br/>
    ///     take a look at the description within <see cref="OnLoad(UpdateSystem)"/>!!!
    /// </summary>
    private bool UseOnGameLoadingComplete => false;

    private ModRuntimeContainer? RuntimeContainer { get; set; }
    public Mod() {
        // ctor. is never called/used by unity/co
    }
    public void OnLoad(UpdateSystem updateSystem) {
        try {
            GameManager gameManager = GameManager.instance;
            ModManager modManager = gameManager.modManager;
            if (modManager.TryGetExecutableAsset(this,
                                                 out ExecutableAsset asset)) {
                this.Init(gameManager,
                          modManager,
                          asset);
                if (this.UseOnGameLoadingComplete) {
                    // causes very serious issues if the mod is updated from within the game
                    // with other words, for those who do not use skyve
                    //
                    // first at all, i forgot to unsubscribe on dispose
                    //
                    // second
                    /// <see cref="AssetDatabase.CacheAssets(Boolean, CancellationToken)"/>
                    // works with a database lock
                    // the get-methods seem to not work with a/the database lock
                    // but i dont know if there was/is an interaction elsewhere...
                    this.SubscribeOnGameLoadingComplete();
                }
            }
        } catch (Exception ex) {
            // user LogManagers Logger
            // runtimeContainerHandler might not be initialized
            DisplayError(ex);
        }
    }

    private void Init(GameManager gameManager,
                      ModManager modManager,
                      ExecutableAsset asset) {
        this.RuntimeContainer = this.CreateRuntimeContainer(gameManager,
                                                            modManager,
                                                            asset);
        this.RuntimeContainer.Init(AssetDatabase.global.LoadSettings, true);
        if (this.RuntimeContainer.Languages.HasErroneous) {
            this.RuntimeContainer.ErrorMessages.DisplayErrorMessageForErroneous(this.RuntimeContainer.Languages.GetErroneous(),
                                                                                false);
        }
    }

    private void SubscribeOnGameLoadingComplete() {
        foreach (IMySystemCollector collector in this.RuntimeContainer.SystemCollectors) {
            this.RuntimeContainer.GameManager.onGameLoadingComplete += collector.TryToCollect;
        }
    }
    private void UnSubscribeOnGameLoadingComplete() {
        foreach (IMySystemCollector collector in this.RuntimeContainer.SystemCollectors) {
            this.RuntimeContainer.GameManager.onGameLoadingComplete -= collector.TryToCollect;
        }
    }

    public void OnDispose() {
        try {
            if (this.UseOnGameLoadingComplete) {
                this.UnSubscribeOnGameLoadingComplete();
            }
            this.RuntimeContainer?.Dispose(true);
        } catch (Exception ex) {
            // user LogManagers Logger
            // runtimeContainerHandler might not be initialized
            Logger.Critical(ex, nameof(OnDispose));
        }
    }


    private ModRuntimeContainer CreateRuntimeContainer(GameManager gameManager,
                                                        ModManager modManager,
                                                        ExecutableAsset asset) {
        IMyLogProvider logProvider = new ModLogProvider(Logger);
        Paths paths = new Paths(true,
                                EnvPath.kUserDataPath);
        ILocManagerProvider locManagerProvider = new LocManagerProvider(gameManager.localizationManager);
        IIntSettingsProvider intSettingsProvider = new IntSettingsProvider(gameManager.settings.userInterface);
        LocaleAssetProvider localeAssetProvider = new LocaleAssetProvider(AssetDatabase.global);
        IBuiltInLocaleIdProvider builtInLocaleIdProvider = localeAssetProvider;
        IIndexCountsProvider indexCountsProvider = new IndexCountsProvider(localeAssetProvider);
        ISettingsSaver settingsSaver = new SettingsSaver(AssetDatabase.global);
        ModRuntimeContainer runtimeContainer = new ModRuntimeContainer(logProvider,
                                                                       this,
                                                                       locManagerProvider,
                                                                       intSettingsProvider,
                                                                       indexCountsProvider,
                                                                       builtInLocaleIdProvider,
                                                                       paths) {
            GameManager = gameManager,
            ModManager = modManager,
            ModAsset = asset,
            SettingsSaver = settingsSaver
        };
        IMySystemCollector exportTypeDictionarySourceCollector = new ExportTypeDictionarySourceCollector(runtimeContainer);
        runtimeContainer.SystemCollectors.Add(exportTypeDictionarySourceCollector);
        IMySystemCollector exportTypeAssetCollector = new ExportTypeAssetCollector(runtimeContainer);
        runtimeContainer.SystemCollectors.Add(exportTypeAssetCollector);
        return runtimeContainer;

    }

    /// <summary>
    ///     has to be done here
    ///     <br/>
    ///     <br/>
    ///     its used within <see cref="OnLoad(UpdateSystem)"/>
    ///     <br/>
    ///     <br/>
    ///     <see cref="IModRuntimeContainer"/> might not be initialized
    ///     <br/>
    ///     <br/>
    ///     but <see cref="Logger"/> is needed...
    /// </summary>
    private static void DisplayError(Exception exception) {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(ErrorMessages.Intro);
        builder.AppendLine($"The entire Mod failed to load.");
        Mod.Logger.showsErrorsInUI = true;
        Mod.Logger.Critical(exception, builder);
        Mod.Logger.showsErrorsInUI = false;
    }
}
