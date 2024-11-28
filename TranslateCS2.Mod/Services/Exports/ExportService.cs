using Game.UI.Widgets;

using TranslateCS2.Inf.Attributes;
using TranslateCS2.Mod.Containers;

namespace TranslateCS2.Mod.Services.Exports;
[MyExcludeFromCoverage]
internal class ExportService {
    private readonly IModRuntimeContainer runtimeContainer;
    private readonly IExportServiceStrategy exportServiceStrategy;
    public ExportService(IModRuntimeContainer runtimeContainer) {
        this.runtimeContainer = runtimeContainer;
        this.exportServiceStrategy = new ExportServiceAssetStrategy(this.runtimeContainer);
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
}
