using System.Collections.Generic;

using Colossal.Serialization.Entities;

using Game;

using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Interfaces;

using Unity.Entities;

namespace TranslateCS2.Mod.Systems;
internal partial class MyAfterModificationEndSystem : GameSystemBase {

    private IModRuntimeContainer? runtimeContainer;


    protected override void OnCreate() {
        base.OnCreate();
        this.runtimeContainer = Mod.RuntimeContainer;
    }

    protected override void OnUpdate() {
        // that happens too often
    }

    protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode) {
        if (false) {
            // would be used/needed for Notifications
            World world = this.World;
        }
        IList<IMySystemCollector>? systemCollectors = this.runtimeContainer?.SystemCollectors;
        if (systemCollectors is null) {
            return;
        }
        bool bypassExecutionChecks = false;
        foreach (IMySystemCollector systemCollector in systemCollectors) {
            systemCollector.TryToCollect(purpose, mode, bypassExecutionChecks);
        }
    }



}
