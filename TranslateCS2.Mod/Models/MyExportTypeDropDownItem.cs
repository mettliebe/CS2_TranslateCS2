using System;
using System.Collections.Generic;
using System.Linq;

using Colossal;
using Colossal.IO.AssetDatabase;

using Game.UI.Widgets;

using TranslateCS2.Inf;

namespace TranslateCS2.Mod.Models;
internal class MyExportTypeDropDownItem : DropdownItem<string>, IEquatable<MyExportTypeDropDownItem?> {
    public string Value => this.value;
    public string DisplayName => this.displayName.id;
    public bool IsBaseGame { get; }
    public bool IsPack { get; }
    public bool IsExpansion { get; }
    public IDictionary<string, MyLocaleInfo> LocaleInfos { get; } = new Dictionary<string, MyLocaleInfo>();
    private MyExportTypeDropDownItem(string value,
                                     string displayName,
                                     bool isBaseGame,
                                     bool isPack,
                                     bool isExpansion) {
        this.value = value;
        this.displayName = displayName;
        this.IsBaseGame = isBaseGame;
        this.IsPack = isPack;
        this.IsExpansion = isExpansion;
    }

    public static MyExportTypeDropDownItem Create(string value,
                                                  string displayName,
                                                  bool isBaseGame,
                                                  bool isPack,
                                                  bool isExpansion) {
        string localDisplayName = displayName;
        if (StringConstants.Colossal_Localization.Equals(localDisplayName)) {
            // INFO: Export Uncategorized Mods - see other occurances of this Tag
            localDisplayName = StringConstants.ZZZ_Uncategorized;
        }
        return new MyExportTypeDropDownItem(value,
                                            localDisplayName,
                                            isBaseGame,
                                            isPack,
                                            isExpansion);
    }

    public void AddSource(string localeId,
                          IDictionarySource dictionarySource) {
        if (!this.LocaleInfos.ContainsKey(localeId)) {
            this.LocaleInfos.Add(localeId, new MyLocaleInfo(localeId));
        }
        this.LocaleInfos[localeId].Sources.Add(dictionarySource);
    }

    public void AddLocaleData(LocaleData localeData) {
        string localeId = localeData.localeId;
        if (!this.LocaleInfos.ContainsKey(localeId)) {
            this.LocaleInfos.Add(localeId, new MyLocaleInfo(localeId));
        }
        this.LocaleInfos[localeId].LocaleDatas.Add(localeData);
    }

    public void AddLocaleDatas(IEnumerable<LocaleData> localeDatas) {
        foreach (LocaleData localeData in localeDatas) {
            string localeId = localeData.localeId;
            if (!this.LocaleInfos.ContainsKey(localeId)) {
                this.LocaleInfos.Add(localeId, new MyLocaleInfo(localeId));
            }
            this.LocaleInfos[localeId].LocaleDatas.Add(localeData);
        }
    }

    public bool HasLocaleInfos() {
        return this.LocaleInfos.Values.Where(item => item.HasDatasOrSources()).Any();
    }

    public override bool Equals(object? obj) {
        return this.Equals(obj as MyExportTypeDropDownItem);
    }

    public bool Equals(MyExportTypeDropDownItem? other) {
        return other is not null &&
               this.Value == other.Value;
    }

    public override int GetHashCode() {
        return -1937169414 + EqualityComparer<string>.Default.GetHashCode(this.Value);
    }
}
