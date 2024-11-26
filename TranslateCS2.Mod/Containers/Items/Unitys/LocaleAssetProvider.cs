using System;
using System.Collections.Generic;
using System.Linq;

using Colossal.IO.AssetDatabase;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;
using TranslateCS2.Mod.Interfaces;

namespace TranslateCS2.Mod.Containers.Items.Unitys;
[MyExcludeFromCoverage]
internal class LocaleAssetProvider : IBuiltInLocaleIdProvider {
    private readonly IAssetDatabase global;
    public LocaleAssetProvider(IAssetDatabase global) {
        this.global = global;
    }

    public static Func<LocaleAsset, bool> BuiltInBaseGamePredicate => asset => StringConstants.DataTilde.Equals(asset.subPath) && StringConstants.Game.Equals(asset.database.name);

    public LocaleAsset? Get(string localeId) {
        IEnumerable<LocaleAsset> localeAssets =
            this.GetBuiltInBaseGameLocaleAssets()
                .Where(item => item.localeId.Equals(localeId, StringComparison.OrdinalIgnoreCase));
        if (localeAssets.Any()) {
            return localeAssets.First();
        }
        return null;
    }

    public IReadOnlyList<string> GetBuiltInLocaleIds() {
        IReadOnlyList<string> localeIds =
            this.GetBuiltInBaseGameLocaleAssets()
                .Select(item => item.localeId)
                .Distinct()
                .ToList();
        return localeIds;
    }

    public IEnumerable<LocaleAsset> GetBuiltInBaseGameLocaleAssets() {
        return
            this.global
                .GetAssets(default(SearchFilter<LocaleAsset>))
                .Where(BuiltInBaseGamePredicate);
    }
}
