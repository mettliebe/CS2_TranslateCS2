namespace TranslateCS2.Consts;
public static class ModConstants {

    /// <seealso href="https://cs2.paradoxwikis.com/Naming_Folder_And_Files"/>
    /// <seealso cref="Helpers.FileSystemHelper"/>
    public const string ModsData = nameof(ModsData);

    /// <seealso href="https://cs2.paradoxwikis.com/Naming_Folder_And_Files"/>
    /// <seealso cref="Helpers.FileSystemHelper"/>
    public const string ModsSettings = nameof(ModsSettings);
    //
    //
    //
    public const string NameSimple = nameof(TranslateCS2);
    public const string Name = $"{NameSimple}.Mod";
    //
    //
    //
    public static string LocaleNameLocalizedKey => $"{nameof(TranslateCS2)}.{nameof(LocaleNameLocalizedKey)}";
    public static string JsonExtension => ".json";
    public static string JsonSearchPattern => $"*{JsonExtension}";
    public static string CokExtension => ".cok";
    public static string DllExtension => ".dll";
    public static string DllSearchPattern => $"*{DllExtension}";
    public static int MaxDisplayNameLength => 31;
    public static int MaxErroneous => 5;
    public static string ModExportKeyValueJsonName { get; } = $"_{NameSimple}{JsonExtension}";
    public static string DataPathRawGeneral { get; } = $"{ModsData}{StringConstants.ForwardSlash}";
    public static string DataPathRawSpecific { get; } = $"{DataPathRawGeneral}{Name}{StringConstants.ForwardSlash}";
    public static string OtherModsLocFilePath { get; } = StringConstants.UnofficialLocales;
}
