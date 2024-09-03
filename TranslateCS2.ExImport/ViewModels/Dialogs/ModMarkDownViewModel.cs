using Markdig;
using Markdig.Wpf;

using Prism.Dialogs;
using Prism.Mvvm;

using TranslateCS2.ExImport.Properties.I18N;

namespace TranslateCS2.ExImport.ViewModels.Dialogs;
internal class ModMarkDownViewModel : BindableBase, IDialogAware {
    public static string DocParameterName { get; } = nameof(DocParameterName);
    public static string TitleParameterName { get; } = nameof(TitleParameterName);
    public string? Doc { get; private set; }
    public MarkdownPipeline? Pipeline { get; private set; }

    public string? Title { get; private set; }

    public ModMarkDownViewModel() { }

    public DialogCloseListener RequestClose { get; }

    public bool CanCloseDialog() {
        return true;
    }

    public void OnDialogClosed() {
        // nothing to do
    }

    public void OnDialogOpened(IDialogParameters parameters) {
        this.Title = I18NExport.CaptionAdditionalInformation;
        if (parameters.TryGetValue(TitleParameterName, out string? title)
            && title is not null) {
            this.Title += $" - {title}";
            this.RaisePropertyChanged(nameof(this.Title));
        }
        if (parameters.TryGetValue(DocParameterName, out string? doc)
            && doc is not null) {
            this.Doc = doc;
            this.Pipeline = new MarkdownPipelineBuilder().UseSupportedExtensions().Build();
            this.RaisePropertyChanged(nameof(this.Doc));
            this.RaisePropertyChanged(nameof(this.Pipeline));
        }
    }
}
