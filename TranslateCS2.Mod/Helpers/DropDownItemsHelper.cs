using System.Collections.Generic;

using Game.UI.Widgets;

using TranslateCS2.Inf;

namespace TranslateCS2.Mod.Helpers;
internal static class DropDownItemsHelper {
    public static string None { get; } = StringConstants.NoneLower;
    public static List<DropdownItem<string>> GetDefault(bool addNone) {
        List<DropdownItem<string>> flavors = [];
        if (addNone) {
            flavors.Add(Create(None, None));
        }
        return flavors;
    }

    public static void AppendAllEntry(List<DropdownItem<string>> items) {
        items.Add(Create(StringConstants.All, StringConstants.All));
    }

    public static void AppendGameEntry(List<DropdownItem<string>> items) {
        items.Add(Create(StringConstants.Game, StringConstants.Game));
    }

    public static DropdownItem<string> Create(string value, string displayName) {
        return new DropdownItem<string>() {
            value = value,
            displayName = displayName
        };
    }
}
