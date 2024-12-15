
namespace TranslateCS2.Enums;
internal enum FlavorSourceTypes : uint {
    /// <summary>
    ///     <see cref="TranslateCS2.Containers.Items.FlavorSource"/> that is provided within this <see cref="TranslateCS2.Mod"/>s data-directory
    /// </summary>
    THIS = 0,
    /// <summary>
    ///     <see cref="TranslateCS2.Containers.Items.FlavorSource"/> that is provided via another <see cref="TranslateCS2.Mod"/>s directory (direct and specific directory within the mod itself)
    /// </summary>
    OTHER = 1
}
