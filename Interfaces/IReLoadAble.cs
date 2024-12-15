using TranslateCS2.Services.Localizations;

namespace TranslateCS2.Interfaces;
internal interface IReLoadAble {
    void ReLoad(LocFileService<string> locFileService);
}
