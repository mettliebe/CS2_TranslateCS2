using TranslateCS2.Models;

namespace TranslateCS2.Interfaces;
internal interface IMyExportTypeCollector : IMySystemCollector {
    MyExportTypeDropDownItems ExportTypeDropDownItems { get; }
}
