using Game.UI.Widgets;

namespace TranslateCS2.Mod.Services.Exports.Strategys;
/// <summary>
///     exchangable strategy to use with <see cref="ExportService"/>
/// </summary>
internal interface IExportServiceStrategy {
    DropdownItem<string>[] GetExportDropDownItems();
    DropdownItem<string>[] GetExportTypeDropDownItems();
    void Export(string localeId,
                string type,
                string directory);
    void Refresh();
}
