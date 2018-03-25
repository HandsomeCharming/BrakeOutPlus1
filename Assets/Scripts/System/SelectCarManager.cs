using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCarManager : MonoBehaviour {

    public SelectMenuUI m_Menu;
    
    public int m_CurrentSceneIndex;

    int m_CarIndex;

    public CarSelectData m_Storer;

    SceneCars m_CurrentScene;
    List<SingleCarSelectData> m_CurrentCars;

	// Use this for initialization
	void Awake ()
    {
        m_Menu.m_Manager = this;
        m_CarIndex = 0;
        m_CurrentSceneIndex = 0;
        m_CurrentScene = m_Storer.sceneData[m_CurrentSceneIndex];
        m_CurrentCars = m_CurrentScene.carData;
        //m_CurrentCarName = m_CarNames[m_CarIndex];
        //m_Menu.SetText(m_CurrentCarName);
    }
    
    public void NextCar()
    {
        m_CarIndex++;
        if(m_CarIndex >= m_CurrentCars.Count)
        {
            m_CarIndex = 0;
        }
        //m_CurrentCarName = m_CarNames[m_CarIndex];
        //AppManager.instance.SetCarName(m_CurrentCarName);
        GameManager.current.ReloadCar(m_CurrentCars[m_CarIndex].CarInGamePrefab);
    }

    public void PrevCar()
    {
        m_CarIndex--;
        if (m_CarIndex < 0)
        {
            m_CarIndex = m_CurrentCars.Count -1;
        }
        //m_CurrentCarName = m_CarNames[m_CarIndex];
        //AppManager.instance.SetCarName(m_CurrentCarName);
        GameManager.current.ReloadCar(m_CurrentCars[m_CarIndex].CarInGamePrefab);
    }

    public void SelectScene(int index)
    {
        m_CurrentSceneIndex = index;
        m_CurrentScene = m_Storer.sceneData[m_CurrentSceneIndex];
        m_CurrentCars = m_CurrentScene.carData;
        m_CarIndex = 0;
        GameManager.current.ReloadCar(m_CurrentCars[m_CarIndex].CarInGamePrefab);
        BackgroundEnum back = (BackgroundEnum)m_CurrentSceneIndex;
        GameManager.current.ChangeBackground(back);
        m_Menu.RefreshForManager();
    }

    public string GetCurrentCarName()
    {
        return m_CurrentCars[m_CarIndex].name;
    }
}
