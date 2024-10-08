using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using Prism.Mvvm;
using Prism.Navigation.Regions;

using TranslateCS2.ExImport.Models;
using TranslateCS2.ExImport.Properties.I18N;
using TranslateCS2.TextSearch.Models;
using TranslateCS2.TextSearch.ViewModels;

namespace TranslateCS2.ExImport.Controls.Imports;
internal class ComparisonDataGridContext : BindableBase, INavigationAware {
    public TextSearchControlContext<CompareExistingReadTranslation> TextSearchContext { get; }


    private ImportModes _ImportMode;
    public ImportModes ImportMode {
        get => this._ImportMode;
        set => this.SetProperty(ref this._ImportMode, value);
    }


    private bool _HideEqual = false;
    public bool HideEqual {
        get => this._HideEqual;
        set => this.SetProperty(ref this._HideEqual, value, this.OnSearch);
    }


    private readonly List<CompareExistingReadTranslation> Backing = [];

    public ObservableCollection<CompareExistingReadTranslation> Preview { get; } = [];
    public bool IsPreviewAvailable => this.Preview.Any();


    public ComparisonDataGridContext() {
        this.TextSearchContext = new TextSearchControlContext<CompareExistingReadTranslation>();
        this.InitTextSearchContext();
    }

    private void InitTextSearchContext() {
        this.TextSearchContext.OnSearch += this.OnSearch;
        this.TextSearchContext.Columns.Add(
        new ColumnSearchAble<CompareExistingReadTranslation>(I18NImport.ColumnKey, I18NImport.ColumnKeyTip) {
            IsChecked = true,
            Matcher = this.MatchesKeyColumn
        });
        this.TextSearchContext.Columns.Add(
        new ColumnSearchAble<CompareExistingReadTranslation>(I18NImport.ColumnExisting, I18NImport.ColumnExistingTip) {
            IsChecked = true,
            Matcher = this.MatchesExistingColumn
        });
        this.TextSearchContext.Columns.Add(
        new ColumnSearchAble<CompareExistingReadTranslation>(I18NImport.ColumnRead, I18NImport.ColumnReadTip) {
            IsChecked = true,
            Matcher = this.MatchesImportedColumn
        });
        foreach (ColumnSearchAble<CompareExistingReadTranslation> column in this.TextSearchContext.Columns) {
            column.OnIsCheckedChange += this.OnSearch;
        }
    }


    private bool MatchesKeyColumn(CompareExistingReadTranslation parameter) {
        return
            this.TextSearchContext.SearchString is not null
            && parameter.Key is not null
            && parameter.Key.Contains(this.TextSearchContext.SearchString,
                                      StringComparison.OrdinalIgnoreCase);
    }
    private bool MatchesExistingColumn(CompareExistingReadTranslation parameter) {
        return
            this.TextSearchContext.SearchString is not null
            && parameter.TranslationExisting is not null
            && parameter.TranslationExisting.Contains(this.TextSearchContext.SearchString,
                                                      StringComparison.OrdinalIgnoreCase);
    }
    private bool MatchesImportedColumn(CompareExistingReadTranslation parameter) {
        return
            this.TextSearchContext.SearchString is not null
            && parameter.TranslationRead is not null
            && parameter.TranslationRead.Contains(this.TextSearchContext.SearchString,
                                                  StringComparison.OrdinalIgnoreCase);
    }


    private void OnSearch() {
        Application.Current.Dispatcher.Invoke(() => {
            this.Preview.Clear();
            foreach (CompareExistingReadTranslation item in this.Backing) {
                if (this.HideEqual
                    && item.IsEqual()) {
                    continue;
                }
                if (this.TextSearchContext.IsTextSearchMatch(item)) {
                    this.Preview.Add(item);
                }
            }
        });
    }


    public bool IsNavigationTarget(NavigationContext navigationContext) {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext) {
        this.Preview.Clear();
        this.RaisePropertyChanged(nameof(this.IsPreviewAvailable));
    }

    public void OnNavigatedTo(NavigationContext navigationContext) {
        this.Preview.Clear();
        this.RaisePropertyChanged(nameof(this.IsPreviewAvailable));
    }

    public void Raiser() {
        this.RaisePropertyChanged(nameof(this.Preview));
        this.RaisePropertyChanged(nameof(this.IsPreviewAvailable));
        this.RaisePropertyChanged(nameof(this.TextSearchContext));
    }

    public void Clear() {
        this.Backing.Clear();
        Application.Current.Dispatcher.Invoke(this.Preview.Clear);
        this.Raiser();
    }

    public void SetItems(IList<CompareExistingReadTranslation> preview) {
        this.Backing.AddRange(preview);
        this.OnSearch();
    }

    public IList<CompareExistingReadTranslation> GetItems() {
        return this.Backing;
    }
}
