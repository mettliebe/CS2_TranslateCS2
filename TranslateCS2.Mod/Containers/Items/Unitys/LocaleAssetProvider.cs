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

    private static ISet<string> DefaultDatabaseNames { get; } = new HashSet<string>() {
        StringConstants.User,
        StringConstants.ParadoxMods,
        StringConstants.Game,
        StringConstants.SteamCloud,
    };

    public static Func<LocaleAsset, bool> BuiltInBaseGamePredicate => asset => StringConstants.Game.Equals(asset.database.name);
    public static Func<LocaleAsset, bool> ParadoxModsPredicate => asset => StringConstants.ParadoxMods.Equals(asset.database.name);
    public static Func<LocaleAsset, bool> UserModsPredicate => asset => StringConstants.User.Equals(asset.database.name);
    public static Func<LocaleAsset, bool> ExtensionsPredicate => asset => !DefaultDatabaseNames.Contains(asset.database.name);


    public IEnumerable<LocaleAsset>? Get(string localeId) {
        IEnumerable<LocaleAsset> localeAssets =
            this.GetLocaleAssets()
                .Where(item => item.localeId.Equals(localeId, StringComparison.OrdinalIgnoreCase));
        if (localeAssets.Any()) {
            return localeAssets;
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

    /// <summary>
    ///     only Base-Game without any Mods
    ///     <br/>
    ///     <br/>
    ///     neither local/user
    ///     <br/>
    ///     nor online
    /// </summary>
    public IEnumerable<LocaleAsset> GetBuiltInBaseGameLocaleAssets() {
        return
            this.GetLocaleAssets()
                .Where(BuiltInBaseGamePredicate)
                .OrderBy(asset => asset.localeId);
    }

    /// <summary>
    ///     only Online-Mods
    /// </summary>
    public IEnumerable<LocaleAsset> GetParadoxModsLocaleAssets() {
        return
            this.GetLocaleAssets()
                .Where(ParadoxModsPredicate);
    }

    /// <summary>
    ///     only Local-Mods/User-Mods
    /// </summary>
    public IEnumerable<LocaleAsset> GetUserModsLocaleAssets() {
        return
            this.GetLocaleAssets()
                .Where(UserModsPredicate);
    }

    public IEnumerable<LocaleAsset> GetLocaleAssets() {
        // mods are included
        return
            this.global
                .GetAssets(default(SearchFilter<LocaleAsset>));
    }

    public IEnumerable<LocaleAsset> GetExpansionAssets() {
        return
            this.GetLocaleAssets()
                .Where(ExtensionsPredicate);
    }
}
