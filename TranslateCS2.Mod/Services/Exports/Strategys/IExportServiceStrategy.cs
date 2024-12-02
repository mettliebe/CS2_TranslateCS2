using Game.UI.Widgets;

namespace TranslateCS2.Mod.Services.Exports.Strategys;
internal interface IExportServiceStrategy {
    DropdownItem<string>[] GetExportDropDownItems();
    DropdownItem<string>[] GetExportTypeDropDownItems();
    void Export(string localeId,
                string type,
                string directory);
}
