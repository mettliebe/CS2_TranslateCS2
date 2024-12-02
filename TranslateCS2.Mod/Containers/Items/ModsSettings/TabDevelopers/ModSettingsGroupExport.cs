using Colossal.Json;
using Colossal.UI;

using Game.Settings;
using Game.UI.Widgets;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;

namespace TranslateCS2.Mod.Containers.Items;
internal partial class ModSettings {



    public const string ExportGroup = nameof(ExportGroup);



    /// <inheritdoc cref="GetExportTypeValueVersion"/>
    private int ExportTypeValueVersion { get; set; } = 0;
    /// <summary>
    ///     is used to trigger an update of values
    /// </summary>
    public int GetExportTypeValueVersion() {
        return this.ExportTypeValueVersion;
    }



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
    [SettingsUIValueVersion(typeof(ModSettings), nameof(GetExportTypeValueVersion))]
    public string ExportTypeDropDown { get; set; } = StringConstants.All;

    [MyExcludeFromCoverage]
    private DropdownItem<string>[] GetExportTypeDropDownItems() {
        return this.exportService.GetExportTypeDropDownItems();
    }



    [Exclude]
    [SettingsUIDeveloper]
    [SettingsUISection(TabDevelopers, ExportGroup)]
    [SettingsUIButton]
    [SettingsUIConfirmation]
    [MyExcludeFromCoverage]
    public bool ExportTypeRefreshButton {
        set {
            this.exportService.Refresh();
            this.ExportTypeValueVersion++;
            UIManager.instance.Update();
        }
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
