using System;

using Colossal.Json;

using Game.Settings;

using TranslateCS2.Consts;

namespace TranslateCS2.Containers.Items;
internal partial class ModSettings {



    public const string ReloadGroup = nameof(ReloadGroup);



    [Exclude]
    [SettingsUIButton]
    [SettingsUIDeveloper]
    [SettingsUIConfirmation]
    [SettingsUISection(TabDevelopers, ReloadGroup)]
    public bool ReloadLanguages {
        set => this.ReloadLangs();
    }

    private void ReloadLangs() {
        try {
            this.languages.ReLoad();
            this.runtimeContainer.LocManager.ReloadActiveLocale();
            if (this.languages.HasErroneous) {
                this.runtimeContainer.ErrorMessages.DisplayErrorMessageForErroneous(this.languages.GetErroneous(), true);
            }
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogCritical(this.GetType(),
                                                     LoggingConstants.FailedTo,
                                                     [nameof(ReloadLangs), ex]);
        }
    }
}
