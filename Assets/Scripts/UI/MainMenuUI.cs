using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : UIBase
{
    public MenuBarButton[] m_MenuBarButtons;
    public MenuBarButton m_CurrentMenuBar;

    private void Awake()
    {
        m_MenuBarButtons = GetComponentsInChildren<MenuBarButton>();
        if(m_CurrentMenuBar != null)
		    ActivateMenu(m_CurrentMenuBar, false);
    }

    public void ActivateMenu(MenuBarButton button, bool sendEvent = true)
    {
        print(button.name);
        if (button == m_CurrentMenuBar && sendEvent)
        {
            button.m_EventWhenPressedAgain.Invoke();
            return;
        }

        m_CurrentMenuBar = button;
        foreach (MenuBarButton mb in m_MenuBarButtons)
        {
            if(mb != button)
                mb.SetActivateVisual(false);
        }
        m_CurrentMenuBar.SetActivateVisual(true);
        //button.SetActivate(true);
    }
}
