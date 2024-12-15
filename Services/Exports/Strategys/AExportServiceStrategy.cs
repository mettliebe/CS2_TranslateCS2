using System.Collections.Generic;
using System.IO;

using Game.UI.Widgets;

using TranslateCS2.Consts;
using TranslateCS2.Helpers;

namespace TranslateCS2.Services.Exports.Strategys;
/// <summary>
///     <see langword="abstract"/> implementation and realization of <see cref="IExportServiceStrategy"/>
///     <br/>
///     for exchangable strategies to use with <see cref="ExportService"/>
/// </summary>
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
