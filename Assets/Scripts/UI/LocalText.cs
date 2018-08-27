using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalText : MonoBehaviour {

    string original;

    private void Awake()
    {
        original = GetComponent<Text>().text;

        if(LocalizationManager.current)
        {
            LocalizationManager.current.languageChanged += Refresh;
        }
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (LocalizationManager.current && GetComponent<Text>())
        {
            Text text = GetComponent<Text>();
            text.text = LocalizationManager.current.GetLocalString(original);

            Font font = LocalizationManager.current.GetFont();
            if (font != null && text.font != font)
            {
                text.font = font;
            }
        }
    }

    private void OnDestroy()
    {
        if (LocalizationManager.current)
        {
            LocalizationManager.current.languageChanged -= Refresh;
        }
    }
}
