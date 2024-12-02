using System;
using System.Collections.Generic;
using System.Linq;

using Colossal.IO.AssetDatabase;
using Colossal.Serialization.Entities;

using Game;

using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod.Services.Exports.Collectors;
internal class ExportTypeAssetCollector : AExportTypeCollector {

    public ExportTypeAssetCollector(IModRuntimeContainer runtimeContainer) : base(runtimeContainer) {
        // dont execute over there
        // otherwise,
        // asset-mods appear within the dropdown,
        // but do not have localizations
        /// <see cref="Systems.MyAfterModificationEndSystem"/>
        this.collected = true;
    }

    public override void TryToCollect(Purpose purpose, GameMode mode, bool bypassExecutionChecks) {
        if (this.HasToBeExecutedNot(purpose, mode)
            && !bypassExecutionChecks) {
            return;
        }
        this.HandleExportTypeDropDownItemsForOnlineMods();
        this.HandleExportTypeDropDownItemsForUserMods();
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
                                                                            m.displayName);
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
                                                                        m.displayName);
            IEnumerable<LocaleData> assetDatas =
                assets
                    .Where(asset => modId.Equals(OtherModsLocFilesHelper.GetIdFromAssetSubPath(asset)))
                    .Select(asset => asset.data);
            this.ExportTypeDropDownItems.AddDropDownItem(item,
                                                         assetDatas);
        }
    }
}
