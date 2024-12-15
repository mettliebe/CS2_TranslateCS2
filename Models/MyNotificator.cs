using Colossal.PSI.Common;

using Game.UI.Menu;

using TranslateCS2.Consts;

namespace TranslateCS2.Models;
internal class MyNotificator {
    private readonly NotificationUISystem notificator;
    private readonly string id;
    private readonly string title;
    private readonly float stopMessageDelay;

    private int max;

    public MyNotificator(string id,
                         string title,
                         float stopMessageDelay,
                         NotificationUISystem notificator) {
        this.id = id;
        this.title = title;
        this.notificator = notificator;
    }

    public void Start(string text, int max) {
        this.max = max;
        this.notificator.AddOrUpdateNotification(
            identifier: this.id,
            title: this.title,
            text: text,
            thumbnail: null,
            progressState: ProgressState.Indeterminate,
            progress: 0,
            onClicked: null
        );
    }

    public void Update(string text, int current) {
        this.notificator.AddOrUpdateNotification(
            identifier: this.id,
            title: this.title,
            text: text,
            thumbnail: null,
            progressState: ProgressState.Progressing,
            progress: current * IntConstants.Hundred / this.max,
            onClicked: null
        );
    }
    public void Stop(string text) {
        this.notificator.RemoveNotification(
            identifier: this.id,
            delay: this.stopMessageDelay,
            title: this.title,
            text: text,
            thumbnail: null,
            progressState: ProgressState.Complete,
            progress: IntConstants.Hundred,
            onClicked: null
        );
    }
}
