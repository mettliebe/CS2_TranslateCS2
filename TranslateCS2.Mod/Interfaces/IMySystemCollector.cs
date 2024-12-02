using Colossal.Serialization.Entities;

using Game;

namespace TranslateCS2.Mod.Interfaces;
/// <summary>
///     <see langword="interface"/> to collect information via <see cref="Game.SceneFlow.GameManager.onGameLoadingComplete"/>
/// </summary>
internal interface IMySystemCollector {
    /// <summary>
    ///     <see cref="Game.SceneFlow.GameManager.EventGamePreload"/>
    /// </summary>
    /// <param name="purpose">
    ///     <see cref="Purpose"/>
    /// </param>
    /// <param name="mode">
    ///     <see cref="GameMode"/>
    /// </param>
    void TryToCollect(Purpose purpose, GameMode mode);
    /// <summary>
    ///     primaryly an <see langword="internal"/> method to be able to <paramref name="bypassExecutionChecks"/>
    /// </summary>
    /// <param name="purpose">
    ///     <see cref="Purpose"/>
    /// </param>
    /// <param name="mode">
    ///     <see cref="GameMode"/>
    /// </param>
    /// <param name="bypassExecutionChecks">
    ///     <see langword="true"/>, to bypass execution checks,
    ///     <see langword="false"/> otherwise
    /// </param>
    void TryToCollect(Purpose purpose, GameMode mode, bool bypassExecutionChecks);
}
