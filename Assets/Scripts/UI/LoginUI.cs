using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : UIBase
{
    public GameObject m_LoginPage;
    public GameObject m_NamePage;
    public InputField m_Text;
	
    public void PressPlay()
    {
        m_LoginPage.SetActive(false);
        m_NamePage.SetActive(true);
    }

    public void ConfirmName()
    {
        m_NamePage.SetActive(false);
        string text = m_Text.text;
        if (text == "") text = "Driver";
        AppManager.instance.RegisterPlayer(text);
        UIManager.current.ChangeStateByGameState();
    }
}
