using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguagePanel : MonoBehaviour {

    public LanguageButton[] buttons;

    private void OnEnable()
    {
        RefreshByCurrentLanguage();
    }

    public void ChooseLanguage(SystemLanguage lang)
    {
        LocalizationManager.current.CurrentLanguage = lang;
        RefreshByCurrentLanguage();
    }

    void RefreshByCurrentLanguage()
    {
        SystemLanguage lang = LocalizationManager.current.CurrentLanguage;
        foreach (var button in buttons)
        {
            if (button.language == lang) button.SetSelected(true);
            else button.SetSelected(false);
        }
    }
}
