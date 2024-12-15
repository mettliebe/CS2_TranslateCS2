using System;
using System.Collections.Generic;

using Colossal.IO.AssetDatabase;

using Game.Modding;

using TranslateCS2.Containers.Items;
using TranslateCS2.Interfaces;
using TranslateCS2.Loggers;
using TranslateCS2.Models;
using TranslateCS2.Models.Localizations;

namespace TranslateCS2.Containers;
internal interface IModRuntimeContainer {
    LocManager LocManager { get; }
    IntSettings IntSettings { get; }
    IIndexCountsProvider IndexCountsProvider { get; }
    IMyLogger Logger { get; }
    Paths Paths { get; }
    Locales Locales { get; }
    ErrorMessages ErrorMessages { get; }
    MyLanguages Languages { get; }
    IMod Mod { get; }
    ModSettings Settings { get; }
    ModManager? ModManager { get; }
    ExecutableAsset? ModAsset { get; }
    ISettingsSaver? SettingsSaver { get; }
    IBuiltInLocaleIdProvider BuiltInLocaleIdProvider { get; }
    IList<IMySystemCollector> SystemCollectors { get; }
    MyExportTypeDropDownItems ExportTypeDropDownItems { get; }
    void Init(Action<string, object, object?>? loadSettings = null, bool register = false);
    void Dispose(bool unregister = false);
}
