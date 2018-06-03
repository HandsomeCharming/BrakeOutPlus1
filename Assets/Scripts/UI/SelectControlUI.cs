using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectControlUI : MonoBehaviour {
    public GameObject[] m_SelectBases;

    private void OnEnable()
    {
        int index =( (int)InputHandler.current.m_ControlScheme)-1;
        foreach (var go in m_SelectBases)
        {
            go.SetActive(false);
        }
        m_SelectBases[index].SetActive(true);
    }

    public void SelectControl(int type)
    {
        if (Enum.IsDefined(typeof(ControlSchemes), type))
        {
            ControlSchemes control = (ControlSchemes)type;

            int index = type - 1;
            InputHandler.current.SetControlScheme(control);

            foreach(var go in m_SelectBases)
            {
                go.SetActive(false);
            }
            m_SelectBases[index].SetActive(true);

            UnityEngine.Analytics.AnalyticsEvent.ScreenVisit(control.ToString());
        }

    }
}
