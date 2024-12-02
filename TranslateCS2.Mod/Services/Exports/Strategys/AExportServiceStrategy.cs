using System.Collections.Generic;
using System.IO;

using Game.UI.Widgets;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Helpers;

namespace TranslateCS2.Mod.Services.Exports.Strategys;
internal abstract class AExportServiceStrategy : IExportServiceStrategy {
    public abstract DropdownItem<string>[] GetExportDropDownItems();

    public abstract DropdownItem<string>[] GetExportTypeDropDownItems();

    public abstract void Export(string localeId,
                                string type,
                                string directory);

    public abstract void Refresh();

    public void WriteEntries(IDictionary<string, string> exportEntries,
                             string localeId,
                             string type,
                             string directory) {
        string path = Path.Combine(directory,
                                   $"{localeId}_{type}{ModConstants.JsonExtension}");
        JsonHelper.Write(exportEntries, path);
    }
}
