using Colossal.Json;
using Colossal.UI;

using Game.Settings;
using Game.UI.Widgets;

using TranslateCS2.Consts;

namespace TranslateCS2.Containers.Items;
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

    private bool IsExportDisabeld => this.GetExportTypeValueVersion() == 0;


    [Exclude]
    [SettingsUIDeveloper]
    [SettingsUISection(TabDevelopers, ExportGroup)]
    [SettingsUIButton]
    [SettingsUIConfirmation]
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
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetExportDropDownItems))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsExportDisabeld))]
    public string ExportDropDown { get; set; }

    private DropdownItem<string>[] GetExportDropDownItems() {
        return this.exportService.GetExportDropDownItems();
    }



    [Exclude]
    [SettingsUIDeveloper]
    [SettingsUISection(TabDevelopers, ExportGroup)]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetExportTypeDropDownItems))]
    [SettingsUIValueVersion(typeof(ModSettings), nameof(GetExportTypeValueVersion))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsExportDisabeld))]
    public string ExportTypeDropDown { get; set; } = StringConstants.All;

    private DropdownItem<string>[] GetExportTypeDropDownItems() {
        return this.exportService.GetExportTypeDropDownItems();
    }



    [Exclude]
    [SettingsUIDeveloper]
    [SettingsUISection(TabDevelopers, ExportGroup)]
    [SettingsUIDirectoryPicker]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsExportDisabeld))]
    public string ExportDirectory { get; set; }



    [Exclude]
    [SettingsUIDeveloper]
    [SettingsUISection(TabDevelopers, ExportGroup)]
    [SettingsUIButton]
    [SettingsUIConfirmation]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsExportDisabeld))]
    public bool ExportButton {
        set => this.exportService.Export(this.ExportDropDown,
                                         this.ExportTypeDropDown,
                                         this.ExportDirectory);
    }
}
