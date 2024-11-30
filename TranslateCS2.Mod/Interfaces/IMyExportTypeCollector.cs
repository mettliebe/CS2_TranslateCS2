using System.Linq;

using Colossal.Serialization.Entities;

using Game;

using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod.Interfaces;
internal interface IMyExportTypeCollector {
    IOrderedEnumerable<MyExportTypeDropDownItem> ExportTypeDropDownItems { get; }
    void TryToCollect(Purpose purpose, GameMode mode);
}
