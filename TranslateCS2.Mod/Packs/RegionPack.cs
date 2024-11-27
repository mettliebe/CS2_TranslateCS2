using Game.UI.Widgets;

namespace TranslateCS2.Mod.Packs;
/// <summary>
///     <see cref="RegionPack"/>s are somehow Mods
///     <br/>
///     <br/>
///     but they seem to be unrecognized by the <see cref="Game.Modding.ModManager"/>
///     <br/>
///     <br/>
/// </summary>
internal class RegionPack {
    public string Id { get; }
    public string Name { get; }
    private RegionPack(string id,
                       string name) {
        this.Id = id;
        this.Name = name;
    }

    public static RegionPack French() {
        return new RegionPack("91930", "French Pack");
    }

    public static DropdownItem<string> FrenchDropDownItem() {
        RegionPack pack = French();
        return DropdownItemFromPack(pack);
    }

    public static RegionPack German() {
        return new RegionPack("91931", "German Pack");
    }

    public static DropdownItem<string> GermanDropDownItem() {
        RegionPack pack = German();
        return DropdownItemFromPack(pack);
    }

    public static RegionPack UK() {
        return new RegionPack("92859", "UK Pack");
    }

    public static DropdownItem<string> UKDropDownItem() {
        RegionPack pack = UK();
        return DropdownItemFromPack(pack);
    }

    private static DropdownItem<string> DropdownItemFromPack(RegionPack pack) {
        return new DropdownItem<string>() {
            value = pack.Id,
            displayName = pack.Name,
        };
    }
}
