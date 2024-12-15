using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Colossal;
using Colossal.Localization;
using Colossal.Reflection;

using TranslateCS2.Interfaces;
using TranslateCS2.Models;

using UnityEngine;

namespace TranslateCS2.Containers.Items.Unitys;
/// <summary>
///     wrapper for <see cref="LocalizationManager"/>
/// </summary>
internal class LocManagerProvider : ILocManagerProvider {
    /// <summary>
    ///     <see cref="LocalizationManager.m_LocaleInfos"/>
    /// </summary>
    private readonly string FieldNameLocaleInfos = "m_LocaleInfos";
    /// <summary>
    ///     <see cref="LocalizationManager.LocaleInfo.m_sources"/>
    /// </summary>
    private readonly string FieldNameSources = "m_Sources";

    private readonly LocalizationManager localizationManager;

    public LocManagerProvider(LocalizationManager localizationManager) {
        this.localizationManager = localizationManager;
    }

    public string FallbackLocaleId => this.localizationManager.fallbackLocaleId;

    public void AddLocale(string localeId,
                          SystemLanguage systemLanguage,
                          string localeName) {
        this.localizationManager.AddLocale(localeId,
                                           systemLanguage,
                                           localeName);
    }

    public void AddSource(string localeId,
                          IDictionarySource source) {
        this.localizationManager.AddSource(localeId,
                                           source);
    }

    public SystemLanguage LocaleIdToSystemLanguage(string localeId) {
        return this.localizationManager.LocaleIdToSystemLanguage(localeId);
    }

    public void ReloadActiveLocale() {
        this.localizationManager.ReloadActiveLocale();
    }

    public void RemoveLocale(string localeId) {
        this.localizationManager.RemoveLocale(localeId);
    }

    public void RemoveSource(string localeId,
                             Flavor source) {
        this.localizationManager.RemoveSource(localeId,
                                              source);
    }

    public void SetActiveLocale(string localeId) {
        this.localizationManager.SetActiveLocale(localeId);
    }

    public bool SupportsLocale(string localeId) {
        return this.localizationManager.SupportsLocale(localeId);
    }

    public IList<MyLocaleInfo> GetLocaleInfos() {
        List<MyLocaleInfo> returnList = [];
        Type type = this.localizationManager.GetType();
        FieldInfo localeInfosField = type.GetFieldIncludingPrivateBase(this.FieldNameLocaleInfos);
        IDictionary? localeInfos = localeInfosField.GetValue(this.localizationManager) as IDictionary;
        if (localeInfos is not null) {
            foreach (DictionaryEntry localeInfo in localeInfos) {
                string? key = localeInfo.Key as string;
                object value = localeInfo.Value;
                FieldInfo sourcesField = value.GetType().GetFieldIncludingPrivateBase(this.FieldNameSources);
                List<IDictionarySource>? sources = sourcesField.GetValue(localeInfo.Value) as List<IDictionarySource>;
                if (key is not null
                    && sources is not null) {
                    returnList.Add(new MyLocaleInfo(key, sources));
                }
            }
        }
        return returnList;
    }
}
