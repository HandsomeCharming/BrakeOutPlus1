using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuickStartUI : MonoBehaviour {

    public GameObject UnlockText;
    public Text m_OnOff;
    public Text m_ButtonText;
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
            m_ButtonText.color = Color.gray;
            UnlockText.SetActive(true);
        }
        else
        {
            m_BaseImage.color = initBaseColor;
            m_ButtonText.color = initBaseColor;
            UnlockText.SetActive(false);
        }

        if (GameManager.current.isQuickstart())
        {
            m_OnOff.text = "ON";
        }
        else
        {
            m_OnOff.text = "OFF";
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
