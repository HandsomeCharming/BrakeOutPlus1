using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuBarButton : MonoBehaviour {
    public Image m_BaseImage;
    public GameObject m_Menu;
    public UnityEvent m_EventWhenPressedAgain;
    public UnityEvent m_EventWhenFirstActivate;
    bool m_Active;

    static Color selectedColor = new Color(100.0f / 255.0f, 120.0f / 255.0f, 135.0f / 255.0f);

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
            m_BaseImage.color = selectedColor;
            if (m_Menu)
            {
                m_Menu.SetActive(true);
            }
        }
        else
        {
            m_BaseImage.color = Color.white;
            if (m_Menu)
            {
                m_Menu.SetActive(false);
            }
        }
    }
}
