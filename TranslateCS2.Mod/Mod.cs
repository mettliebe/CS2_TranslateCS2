using System;
using System.Text;

using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.PSI.Environment;

using Game;
using Game.Modding;
using Game.SceneFlow;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Containers.Items.ModsSettings;
using TranslateCS2.Mod.Containers.Items.Unitys;
using TranslateCS2.Mod.Interfaces;
using TranslateCS2.Mod.Loggers;
using TranslateCS2.Mod.Systems;

namespace TranslateCS2.Mod;
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
[MyExcludeFromCoverage]
public class Mod : IMod {
    private static readonly ILog Logger = LogManager.GetLogger(ModConstants.Name).SetShowsErrorsInUI(false);
    internal static IModRuntimeContainer? RuntimeContainer { get; set; }
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
                // after modification ended
                // so,
                // all mods
                // and their locales
                // are loaded
                // and can be collected
                updateSystem.UpdateAfter<MyAfterModificationEndSystem>(SystemUpdatePhase.ModificationEnd);
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
        Mod.RuntimeContainer = this.CreateRuntimeContainer(gameManager,
                                                            modManager,
                                                            asset);
        Mod.RuntimeContainer.Init(AssetDatabase.global.LoadSettings, true);
        if (Mod.RuntimeContainer.Languages.HasErroneous) {
            Mod.RuntimeContainer.ErrorMessages.DisplayErrorMessageForErroneous(Mod.RuntimeContainer.Languages.GetErroneous(),
                                                                               false);
        }
    }

    public void OnDispose() {
        try {
            Mod.RuntimeContainer?.Dispose(true);
        } catch (Exception ex) {
            // user LogManagers Logger
            // runtimeContainerHandler might not be initialized
            Logger.Critical(ex, nameof(OnDispose));
        }
    }


    private IModRuntimeContainer CreateRuntimeContainer(GameManager gameManager,
                                                        ModManager modManager,
                                                        ExecutableAsset asset) {
        IMyLogProvider logProvider = new ModLogProvider(Logger);
        Paths paths = new Paths(true,
                                EnvPath.kStreamingDataPath,
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
            ModManager = modManager,
            ModAsset = asset,
            SettingsSaver = settingsSaver
        };
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
