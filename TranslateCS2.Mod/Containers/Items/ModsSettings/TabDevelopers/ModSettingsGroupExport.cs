using Colossal.Json;

using Game.Settings;
using Game.UI.Widgets;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;

namespace TranslateCS2.Mod.Containers.Items;
internal partial class ModSettings {



    public const string ExportGroup = nameof(ExportGroup);



    [Exclude]
    [SettingsUIDeveloper]
    [SettingsUISection(TabDevelopers, ExportGroup)]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetExportDropDownItems))]
    public string ExportDropDown { get; set; }

    [MyExcludeFromCoverage]
    private DropdownItem<string>[] GetExportDropDownItems() {
        return this.exportService.GetExportDropDownItems();
    }



    [Exclude]
    [SettingsUIDeveloper]
    [SettingsUISection(TabDevelopers, ExportGroup)]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetExportTypeDropDownItems))]
    public string ExportTypeDropDown { get; set; } = StringConstants.All;

    [MyExcludeFromCoverage]
    private DropdownItem<string>[] GetExportTypeDropDownItems() {
        return this.exportService.GetExportTypeDropDownItems();
    }



    [Exclude]
    [SettingsUIDeveloper]
    [SettingsUISection(TabDevelopers, ExportGroup)]
    [SettingsUIDirectoryPicker]
    public string ExportDirectory { get; set; }



    [Exclude]
    [SettingsUIDeveloper]
    [SettingsUISection(TabDevelopers, ExportGroup)]
    [SettingsUIButton]
    [SettingsUIConfirmation]
    [MyExcludeFromCoverage]
    public bool ExportButton {
        set => this.exportService.Export(this.ExportDropDown,
                                         this.ExportTypeDropDown,
                                         this.ExportDirectory);
    }
}
