using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuBarButton : MonoBehaviour {
    public Image m_BaseImage;
	public Image m_Icon;
	public Text m_Text;
    public GameObject m_Menu;
    public UnityEvent m_EventWhenPressedAgain;
    public UnityEvent m_EventWhenFirstActivate;
    bool m_Active;

    static Color flatDesignSelectedColor = new Color(100.0f / 255.0f, 120.0f / 255.0f, 135.0f / 255.0f);
	static Color glassDesignSelectedColor = new Color(235.0f / 245.0f, 255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
	static Color glassDesignDeselectedColor = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, 100.0f / 255.0f);
	static Color glassDesignContainSelectedColor = new Color(37.0f / 255.0f, 170.0f / 255.0f, 225.0f / 255.0f);

    public void Activate()
    {
        SetActivate(true);
    }

    public void SetActivate(bool active)
    {
        if (active != m_Active && active)
            m_EventWhenFirstActivate.Invoke();
        m_Active = active;
        if(active == true)
        {
            GetComponentInParent<MainMenuUI>().ActivateMenu(this);
        }
    }

    public void SetActivateVisual(bool active)
    {
        m_Active = active;
        if (active == true)
        {
			m_BaseImage.color = glassDesignSelectedColor;
            if(m_Icon != null) {
                m_Icon.color = glassDesignContainSelectedColor;
            }
            if (m_Text != null) {
                m_Text.color = glassDesignContainSelectedColor;
            }
            if (m_Menu)
            {
                m_Menu.SetActive(true);
            }
        }
        else
        {
			m_BaseImage.color = glassDesignDeselectedColor;
            if (m_Icon != null) {
                m_Icon.color = Color.white;
            }
            if (m_Text != null) {
                m_Text.color = Color.white;
            }
            if (m_Menu)
            {
                m_Menu.SetActive(false);
            }
        }
    }
}
