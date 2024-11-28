using System.Collections.Generic;

using Game.UI.Widgets;

using TranslateCS2.Inf;

namespace TranslateCS2.Mod.Helpers;
internal static class DropDownItemsHelper {
    public static string None { get; } = StringConstants.NoneLower;
    public static List<DropdownItem<string>> GetDefault(bool addNone) {
        List<DropdownItem<string>> flavors = [];
        if (addNone) {
            flavors.Add(new DropdownItem<string>() {
                value = None,
                displayName = None
            });
        }
        return flavors;
    }

    public static void AppendAllEntry(List<DropdownItem<string>> items) {
        items.Add(new DropdownItem<string>() {
            value = StringConstants.All,
            displayName = StringConstants.All
        });
    }
    public static void AppendGameEntry(List<DropdownItem<string>> items) {
        items.Add(new DropdownItem<string>() {
            value = StringConstants.Game,
            displayName = StringConstants.Game
        });
    }
}
