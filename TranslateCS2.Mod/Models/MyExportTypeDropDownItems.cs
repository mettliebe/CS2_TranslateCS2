using System.Collections.Generic;
using System.Linq;

using Colossal;
using Colossal.IO.AssetDatabase;

namespace TranslateCS2.Mod.Models;
internal class MyExportTypeDropDownItems {

    private Dictionary<string, MyExportTypeDropDownItem> _Items { get; } = [];

    public IEnumerable<MyExportTypeDropDownItem> Items =>
        this._Items
            .Values
            .Where(item => item.HasLocaleInfos())
            .OrderByDescending(item => item.IsBaseGame)
            .ThenByDescending(item => item.IsColossalOrdersOne)
            .ThenBy(item => item.DisplayName);

    public MyExportTypeDropDownItems() { }

    public void AddDropDownItem(string localeId,
                                MyExportTypeDropDownItem item,
                                IDictionarySource source) {
        if (!this._Items.ContainsKey(item.Value)) {
            this._Items[item.Value] = item;
        }
        this._Items[item.Value].AddSource(localeId, source);
    }

    public void AddDropDownItem(MyExportTypeDropDownItem item,
                                LocaleData localeData) {
        if (!this._Items.ContainsKey(item.Value)) {
            this._Items[item.Value] = item;
        }
        this._Items[item.Value].AddLocaleData(localeData);
    }

    public void AddDropDownItem(MyExportTypeDropDownItem item,
                                IEnumerable<LocaleData> localeDatas) {
        if (!this._Items.ContainsKey(item.Value)) {
            this._Items[item.Value] = item;
        }
        this._Items[item.Value].AddLocaleDatas(localeDatas);
    }

    public void ClearForRefresh() {
        this._Items.Clear();
    }
}
