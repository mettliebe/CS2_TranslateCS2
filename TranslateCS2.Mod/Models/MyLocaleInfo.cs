using System.Collections.Generic;

using Colossal;

namespace TranslateCS2.Mod.Models;
internal class MyLocaleInfo {
    public string Id { get; }
    public IList<IDictionarySource> Sources { get; }
    public MyLocaleInfo(string id,
                        IList<IDictionarySource> sources) {
        this.Id = id;
        this.Sources = sources;
    }
}
