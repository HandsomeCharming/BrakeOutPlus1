using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour {
    public Text m_ControlText;

    private void OnEnable()
    {
        SetControlText();
    }

    public void SetControlText()
    {
        int index = ((int)InputHandler.current.m_ControlScheme) - 1;
        string text = "";
        if (index == 0)
        {
            text = "S";
        }
        else if (index == 1)
        {
            text = "B";
        }
        else
        {
            text = "T";
        }
        m_ControlText.text = text;
    }
}
