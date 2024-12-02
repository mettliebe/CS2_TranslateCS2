using Colossal.Serialization.Entities;

using Game;

namespace TranslateCS2.Mod.Interfaces;
/// <summary>
///     <see langword="interface"/> to collect information within <see cref="GameSystemBase"/>
///     <br/>
///     <br/>
///     <see cref="Systems.MyAfterModificationEndSystem"/>
/// </summary>
internal interface IMySystemCollector {
    void TryToCollect(Purpose purpose, GameMode mode);
    void TryToCollect(Purpose purpose, GameMode mode, bool bypassExecutionChecks);
}
