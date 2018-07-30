using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageButton : MonoBehaviour {
    public GameObject selected;
    public SystemLanguage language;

    public void SetSelected(bool selected)
    {
        this.selected.SetActive(selected);
    }

    public void Clicked()
    {
        if (selected.activeSelf) return;
        transform.parent.parent.GetComponent<LanguagePanel>().ChooseLanguage(language);
    }
}
