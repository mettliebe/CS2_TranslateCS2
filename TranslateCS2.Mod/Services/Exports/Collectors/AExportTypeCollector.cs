using Colossal.Serialization.Entities;

using Game;
using Game.SceneFlow;

using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items.Unitys;
using TranslateCS2.Mod.Interfaces;
using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod.Services.Exports.Collectors;
internal abstract class AExportTypeCollector : IMyExportTypeCollector {

    protected readonly IModRuntimeContainer? runtimeContainer;
    protected readonly LocaleAssetProvider? localeAssetProvider;
    protected readonly LocManagerProvider? locManagerProvider;

    protected readonly GameManager.Configuration? gameConfiguration;

    protected bool collected = false;

    public MyExportTypeDropDownItems ExportTypeDropDownItems { get; }


    protected AExportTypeCollector(IModRuntimeContainer runtimeContainer) {
        this.runtimeContainer = runtimeContainer;
        this.ExportTypeDropDownItems = runtimeContainer.ExportTypeDropDownItems;
        this.localeAssetProvider = this.runtimeContainer?.BuiltInLocaleIdProvider as LocaleAssetProvider;
        this.locManagerProvider = this.runtimeContainer?.LocManager.Provider as LocManagerProvider;
        this.gameConfiguration = GameManager.instance.configuration;
    }


    public abstract void TryToCollect(Purpose purpose, GameMode mode, bool bypassExecutionChecks);

    /// <param name="purpose">
    ///     <see cref="Purpose"/>
    /// </param>
    /// <param name="mode">
    ///     <see cref="GameMode"/>
    /// </param>
    /// <returns>
    ///     <see langword="true"/>, if the collector should NOT be executed
    /// </returns>
    protected bool HasToBeExecutedNot(Purpose purpose, GameMode mode) {
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
