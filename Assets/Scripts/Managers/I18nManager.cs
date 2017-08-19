using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mgl;
using System;

public enum Locale {
    enUS,
    ptBR
}

public class I18nManager : Singleton<I18nManager> {

    public I18nText[] i18nTexts;
    public Locale locale = Locale.ptBR;

    private I18n i18n = I18n.Instance;

    public void Init() {
        SwitchLanguage(locale);
    }

    public void UpdateText(I18nText i18nText) {
        if (i18nText != null) {
            i18nText.Text = i18n.__(i18nText.key);
        }
    }

    public void UpdateAllTexts() {
        foreach (I18nText i18nText in i18nTexts) {
            if (i18nText != null) {
                UpdateText(i18nText);
            }
        }
    }

    public void SwitchLanguage(Locale locale, bool updateTexts = true) {
        this.locale = locale;
        string _locale = "";
        switch (locale) {
            case Locale.ptBR:
                _locale = "pt-BR";
                break;
            case Locale.enUS:
            default:
                _locale = "en-US";
                break;
        }
        I18n.SetLocale(_locale);

        if (updateTexts) {
            UpdateAllTexts();
        }
    }

    public string getText(string key, params object[] args) {
        return args != null ? i18n.__(key, args) : i18n.__(key);
    }

}
