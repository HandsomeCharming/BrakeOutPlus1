using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuickStartUI : MonoBehaviour {


    public Text m_OnOff;
    public Image m_BaseImage;

    Color initBaseColor;

    private void Awake()
    {
        initBaseColor = m_BaseImage.color;
    }

    // Use this for initialization
    void OnEnable () {
        RefreshText();
    }

    void RefreshText()
    {
        if(!GameManager.current.AdRemoved())
        {
            m_BaseImage.color = Color.gray;
        }
        else
        {
            m_BaseImage.color = initBaseColor;
        }

        if (GameManager.current.isQuickstart())
        {
            m_OnOff.text = "On";
        }
        else
        {
            m_OnOff.text = "Off";
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleQuickStart()
    {
        GameManager.current.SetQuickStart(!GameManager.current.isQuickstart());
        RefreshText();
    }
}
