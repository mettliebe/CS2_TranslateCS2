using System.Collections.Generic;

using Colossal;
using Colossal.IO.AssetDatabase;

namespace TranslateCS2.Mod.Models;
internal class MyLocaleInfo {
    public string Id { get; }
    public IList<IDictionarySource> Sources { get; } = [];
    public IList<LocaleData> LocaleDatas { get; } = [];
    public MyLocaleInfo(string id,
                        IList<IDictionarySource> sources) {
        this.Id = id;
        this.Sources = sources;
    }
    public MyLocaleInfo(string id) {
        this.Id = id;
    }

    public bool HasDatas() {
        return this.LocaleDatas.Count > 0;
    }

    public bool HasSources() {
        return this.Sources.Count > 0;
    }

    public bool HasDatasOrSources() {
        return this.HasDatas() || this.HasSources();
    }

    public bool HasSourcesOrDatas() {
        return this.HasDatasOrSources();
    }
}
