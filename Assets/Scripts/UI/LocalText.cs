using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalText : MonoBehaviour {
    private void OnEnable()
    {
        if(LocalizationManager.current && GetComponent<Text>() )
        {
            Text text = GetComponent<Text>();
            text.text = LocalizationManager.current.GetLocalString(text.text);

            Font font = LocalizationManager.current.GetFont();
            if (font != null && text.font != font)
            {
                text.font = font;
            }
        }
    }
}
