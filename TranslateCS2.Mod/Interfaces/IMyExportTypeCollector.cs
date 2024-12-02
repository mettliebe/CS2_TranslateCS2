using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod.Interfaces;
internal interface IMyExportTypeCollector : IMySystemCollector {
    MyExportTypeDropDownItems ExportTypeDropDownItems { get; }
}
