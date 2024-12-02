using System;
using System.Collections.Generic;
using System.Linq;

using Colossal.IO.AssetDatabase;
using Colossal.Serialization.Entities;

using Game;

using TranslateCS2.Inf.Attributes;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod.Services.Exports.Collectors;
/// <summary>
///     uses the <see cref="Game"/>s
///     <br/>
///     <see cref="AssetDatabase"/>
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
internal class ExportTypeAssetCollector : AExportTypeCollector {

    public ExportTypeAssetCollector(IModRuntimeContainer runtimeContainer) : base(runtimeContainer) { }

    public override void TryToCollect(Purpose purpose, GameMode mode, bool bypassExecutionChecks) {
        if (this.HasToBeExecutedNot(purpose, mode)
            && !bypassExecutionChecks) {
            return;
        }
        this.HandleExportTypeDropDownItemsForExpansions();
        this.HandleExportTypeDropDownItemsForOnlineMods();
        this.HandleExportTypeDropDownItemsForUserMods();
    }

    private void HandleExportTypeDropDownItemsForExpansions() {
        IEnumerable<LocaleAsset>? assets = this.localeAssetProvider.GetExpansionAssets();
        if (assets is null) {
            return;
        }
        IEnumerable<string> expansionNames = assets.Select(asset => asset.database.name).Distinct();
        foreach (string expansionName in expansionNames) {
            MyExportTypeDropDownItem item = MyExportTypeDropDownItem.Create(expansionName,
                                                                            expansionName,
                                                                            false,
                                                                            false,
                                                                            true);
            IEnumerable<LocaleData> assetDatas =
                assets
                    .Where(asset => asset.database.name.Equals(expansionName))
                    .Select(asset => asset.data);
            this.ExportTypeDropDownItems.AddDropDownItem(item,
                                                         assetDatas);
        }
    }

    private void HandleExportTypeDropDownItemsForUserMods() {
        IEnumerable<LocaleAsset>? assets = this.localeAssetProvider.GetUserModsLocaleAssets();
        if (assets is null) {
            return;
        }
        IEnumerable<string?> modNames = assets.Select(OtherModsLocFilesHelper.GetNameFromAssetSubPath).Distinct();
        foreach (string? modName in modNames) {
            if (modName is null) {
                continue;
            }
            Colossal.PSI.Common.Mod? mod = OtherModsLocFilesHelper.GetModViaName(this.runtimeContainer, modName);
            if (mod is null) {
                continue;
            }
            Colossal.PSI.Common.Mod m = (Colossal.PSI.Common.Mod) mod;
            MyExportTypeDropDownItem item = MyExportTypeDropDownItem.Create(m.displayName,
                                                                            m.displayName,
                                                                            false,
                                                                            false,
                                                                            false);
            IEnumerable<LocaleData> assetDatas =
                assets
                    .Where(asset => modName.Equals(OtherModsLocFilesHelper.GetNameFromAssetSubPath(asset)))
                    .Select(asset => asset.data);
            this.ExportTypeDropDownItems.AddDropDownItem(item,
                                                         assetDatas);
        }
    }

    private void HandleExportTypeDropDownItemsForOnlineMods() {
        IEnumerable<LocaleAsset>? assets = this.localeAssetProvider.GetParadoxModsLocaleAssets();
        if (assets is null) {
            return;
        }
        IEnumerable<string?> modIds = assets.Select(OtherModsLocFilesHelper.GetIdFromAssetSubPath).Distinct();
        foreach (string? modId in modIds) {
            if (modId is null) {
                continue;
            }
            Colossal.PSI.Common.Mod? mod = OtherModsLocFilesHelper.GetModViaId(this.runtimeContainer, Int32.Parse(modId));
            if (mod is null) {
                continue;
            }
            Colossal.PSI.Common.Mod m = (Colossal.PSI.Common.Mod) mod;
            MyExportTypeDropDownItem item = MyExportTypeDropDownItem.Create(modId,
                                                                            m.displayName,
                                                                            false,
                                                                            false,
                                                                            false);
            IEnumerable<LocaleData> assetDatas =
                assets
                    .Where(asset => modId.Equals(OtherModsLocFilesHelper.GetIdFromAssetSubPath(asset)))
                    .Select(asset => asset.data);
            this.ExportTypeDropDownItems.AddDropDownItem(item,
                                                         assetDatas);
        }
    }
}
