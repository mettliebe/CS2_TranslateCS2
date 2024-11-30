using System;
using System.Collections.Generic;

using Game.UI.Widgets;

namespace TranslateCS2.Mod.Models;
internal class MyExportTypeDropDownItem : DropdownItem<string>, IEquatable<MyExportTypeDropDownItem?> {
    public string Value => this.value;
    public string DisplayName => this.displayName.id;
    public bool IsColossalOrdersOne { get; }
    private MyExportTypeDropDownItem(string value,
                                     string displayName,
                                     bool isColossalOrdersOne) {
        this.value = value;
        this.displayName = displayName;
        this.IsColossalOrdersOne = isColossalOrdersOne;
    }

    public static MyExportTypeDropDownItem Create(string value,
                                                  string displayName,
                                                  bool isColossalOrdersOne = false) {
        return new MyExportTypeDropDownItem(value,
                                            displayName,
                                            isColossalOrdersOne);
    }

    public override bool Equals(object? obj) {
        return this.Equals(obj as MyExportTypeDropDownItem);
    }

    public bool Equals(MyExportTypeDropDownItem? other) {
        return other is not null &&
               this.Value == other.Value;
    }

    public override int GetHashCode() {
        return -1937169414 + EqualityComparer<string>.Default.GetHashCode(this.Value);
    }

    public static bool operator ==(MyExportTypeDropDownItem? left, MyExportTypeDropDownItem? right) {
        return EqualityComparer<MyExportTypeDropDownItem>.Default.Equals(left, right);
    }

    public static bool operator !=(MyExportTypeDropDownItem? left, MyExportTypeDropDownItem? right) {
        return !(left == right);
    }
}
