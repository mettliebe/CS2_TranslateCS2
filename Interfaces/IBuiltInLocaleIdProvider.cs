using System.Collections.Generic;

namespace TranslateCS2.Interfaces;
internal interface IBuiltInLocaleIdProvider {
    IReadOnlyList<string> GetBuiltInLocaleIds();
}
