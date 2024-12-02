using System.Collections.Generic;
using System.Linq;

using Colossal;
using Colossal.IO.AssetDatabase;

namespace TranslateCS2.Mod.Models;
internal class MyExportTypeDropDownItems {

    private Dictionary<string, MyExportTypeDropDownItem> items { get; } = [];

    public IEnumerable<MyExportTypeDropDownItem> Items =>
        this.items
            .Values
            .OrderByDescending(item => item.IsBaseGame)
            .ThenByDescending(item => item.IsColossalOrdersOne)
            .ThenBy(item => item.DisplayName);

    public MyExportTypeDropDownItems() { }

    public void AddDropDownItem(string localeId,
                                MyExportTypeDropDownItem item,
                                IDictionarySource source) {
        if (!this.items.ContainsKey(item.Value)) {
            this.items[item.Value] = item;
        }
        this.items[item.Value].AddSource(localeId, source);
    }

    public void AddDropDownItem(MyExportTypeDropDownItem item,
                                LocaleData localeData) {
        if (!this.items.ContainsKey(item.Value)) {
            this.items[item.Value] = item;
        }
        this.items[item.Value].AddLocaleData(localeData);
    }
}
