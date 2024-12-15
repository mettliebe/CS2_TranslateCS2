using System;

namespace TranslateCS2.Keyz;
public interface IMyKey : IEquatable<IMyKey?> {
    string Key { get; set; }
    bool IsIndexed { get; }
    string CountKey { get; }
    int Index { get; }
}
