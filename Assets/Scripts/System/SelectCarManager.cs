using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCarManager : MonoBehaviour {
    public SelectMenuUI m_Menu;

    public string[] m_CarNames;
    public string m_CurrentCarName;
    int m_CarIndex;

	// Use this for initialization
	void Awake () {
        m_Menu.enabled = true;
        m_Menu.m_Manager = this;
        m_CarIndex = 0;
        m_CurrentCarName = m_CarNames[m_CarIndex];
        m_Menu.SetText(m_CurrentCarName);
	}
    
    public void NextCar()
    {
        m_CarIndex++;
        if(m_CarIndex >= m_CarNames.Length)
        {
            m_CarIndex = 0;
        }
        m_CurrentCarName = m_CarNames[m_CarIndex];
        AppManager.instance.SetCarName(m_CurrentCarName);
        GameManager.current.ReloadCar();
    }

    public void PrevCar()
    {
        m_CarIndex--;
        if (m_CarIndex < 0)
        {
            m_CarIndex = m_CarNames.Length -1;
        }
        m_CurrentCarName = m_CarNames[m_CarIndex];
        AppManager.instance.SetCarName(m_CurrentCarName);
        GameManager.current.ReloadCar();
    }
}
