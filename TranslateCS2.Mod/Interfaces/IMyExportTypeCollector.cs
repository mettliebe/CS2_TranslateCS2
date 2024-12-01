using System.Collections.Generic;

using Colossal.Serialization.Entities;

using Game;

using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod.Interfaces;
internal interface IMyExportTypeCollector {
    IEnumerable<MyExportTypeDropDownItem> ExportTypeDropDownItems { get; }
    void TryToCollect(Purpose purpose, GameMode mode);
}
