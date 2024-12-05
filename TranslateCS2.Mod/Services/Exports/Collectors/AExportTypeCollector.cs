using System;
using System.Threading;

using Colossal.IO.AssetDatabase;
using Colossal.Serialization.Entities;

using Game;
using Game.SceneFlow;

using TranslateCS2.Inf.Attributes;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items.Unitys;
using TranslateCS2.Mod.Interfaces;
using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod.Services.Exports.Collectors;
/// <summary>
///     <see cref="Mod.OnLoad(UpdateSystem)"/>
///     subscribes
///     <see cref="GameManager.onGameLoadingComplete"/>
///     with
///     <see cref="TryToCollect(Purpose, GameMode)"/>
///     <br/>
///     <br/>
///     that was the intention at least...
///     <br/>
///     see the description over there
///     <br/>
///     <see cref="Mod.OnLoad(UpdateSystem)"/>
///     <br/>
///     <br/>
///     for both realizations
///     <br/>
///     <see cref="ExportTypeAssetCollector"/>
///     <br/>
///     <see cref="ExportTypeDictionarySourceCollector"/>
///     <br/>
///     <br/>
///     <see cref="AssetDatabase"/>.global.databases
///     <br/>
///     returns all databases
///     <br/>
///     <br/>
///     but
///     <br/>
///     <see cref="AssetDatabase.onAssetDatabaseChanged"/>.Subscribe&lt;<see cref="LocaleAsset"/>>&gt;(aHandler)
///     <br/>
///     does not work
///     <br/>
///     <br/>
///     perhaps its due to the fact,
///     <br/>
///     <see cref="AssetDatabase.CacheAssets(Boolean, CancellationToken)"/>
///     <br/>
///     disables notifications via
///     <br/>
///     <see cref="AssetDatabase.EnableNotifications(Boolean)"/>
/// </summary>
[MyExcludeFromCoverage]
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


    public void TryToCollect(Purpose purpose, GameMode mode) {
        this.TryToCollect(purpose, mode, false);
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
