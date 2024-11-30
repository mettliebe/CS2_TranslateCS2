using Colossal.Serialization.Entities;

using Game;

using TranslateCS2.Mod.Containers;

using Unity.Entities;

namespace TranslateCS2.Mod.Systems;
internal partial class MyAfterModificationEndSystem : GameSystemBase {


    public static MyAfterModificationEndSystem? INSTANCE { get; private set; }


    private IModRuntimeContainer? runtimeContainer;


    protected override void OnCreate() {
        base.OnCreate();
        MyAfterModificationEndSystem.INSTANCE = this;
        this.runtimeContainer = Mod.RuntimeContainer;
    }

    protected override void OnUpdate() {
        // that happens too often
    }

    protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode) {
        base.OnGameLoadingComplete(purpose, mode);
        if (false) {
            // would be used/needed for Notifications
            World world = this.World;
        }
        this.runtimeContainer?.ExportTypeCollector?.CollectIfPossible(purpose, mode);
    }



}
