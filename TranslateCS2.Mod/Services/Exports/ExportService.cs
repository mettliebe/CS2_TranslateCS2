using Game.UI.Widgets;

using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Services.Exports.Strategys;

namespace TranslateCS2.Mod.Services.Exports;
internal class ExportService {
    private readonly IModRuntimeContainer runtimeContainer;
    private readonly IExportServiceStrategy exportServiceStrategy;
    public ExportService(IModRuntimeContainer runtimeContainer) {
        this.runtimeContainer = runtimeContainer;
        this.exportServiceStrategy = new ExportServiceStrategy(this.runtimeContainer);
    }

    public DropdownItem<string>[] GetExportDropDownItems() {
        return this.exportServiceStrategy.GetExportDropDownItems();
    }

    public DropdownItem<string>[] GetExportTypeDropDownItems() {
        return this.exportServiceStrategy.GetExportTypeDropDownItems();
    }

    public void Export(string localeId,
                       string type,
                       string directory) {
        this.exportServiceStrategy.Export(localeId,
                                          type,
                                          directory);
    }

    public void Refresh() {
        this.exportServiceStrategy.Refresh();
    }
}
