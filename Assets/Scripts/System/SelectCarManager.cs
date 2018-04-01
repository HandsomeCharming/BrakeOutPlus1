using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCarManager : MonoBehaviour {

    public SelectMenuUI m_Menu;
    public SelectTrailUI m_TrailMenu;

    public int m_CurrentSceneIndex;

    int m_CarIndex;
    string m_CurrentTrailName;

    public CarSelectData m_Storer;

    SceneCars m_CurrentScene;
    List<SingleCarSelectData> m_CurrentCars;

	// Use this for initialization
	void Awake ()
    {
        m_Menu.m_Manager = this;
        m_CarIndex = GameManager.current.m_DefaultCarIndex;
        m_CurrentSceneIndex = GameManager.current.m_DefaultSceneIndex;
        m_CurrentScene = m_Storer.sceneData[m_CurrentSceneIndex];
        m_CurrentCars = m_CurrentScene.carData;

        SelectScene(m_CurrentSceneIndex);
        //m_CurrentCarName = m_CarNames[m_CarIndex];
        //m_Menu.SetText(m_CurrentCarName);
        m_Menu.enabled = true;
    }

    public SingleCarSelectData GetCurrentCarData()
    {
        return m_CurrentCars[m_CarIndex];
    }
    
    public bool isCurrentCarAvailable()
    {
        return SaveManager.instance.HasCar(m_CurrentCars[m_CarIndex].name);
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
        if(isCurrentCarAvailable())
        {
            GameManager.current.ReloadCar(m_CarIndex, m_CurrentSceneIndex);
            //GameManager.current.SetDefaultCar(m_CarIndex, m_CurrentSceneIndex);
        }
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
        if (isCurrentCarAvailable())
        {
            GameManager.current.ReloadCar(m_CarIndex, m_CurrentSceneIndex);
            //GameManager.current.SetDefaultCar(m_CarIndex, m_CurrentSceneIndex);
        }
    }

    public void SelectScene(int index)
    {
        if (m_CurrentSceneIndex == index) return;
        m_CurrentSceneIndex = index;
        m_CurrentScene = m_Storer.sceneData[m_CurrentSceneIndex];
        m_CurrentCars = m_CurrentScene.carData;
        m_CarIndex = 0;
        GameManager.current.ReloadCar(m_CurrentCars[m_CarIndex].CarInGamePrefab);
        BackgroundEnum back = (BackgroundEnum)m_CurrentSceneIndex;
        GameManager.current.ChangeBackground(back);
        m_Menu.RefreshForManager();
        m_Menu.ChangeSceneGO(m_CurrentSceneIndex);
    }

    public void BuyCurrentCar()
    {
        if(SaveManager.instance.BuyCar(m_CarIndex, m_CurrentSceneIndex))
        {
            SaveManager.instance.Save();
            m_Menu.RefreshForManager();
        }
    }

    public string GetCurrentCarName()
    {
        return m_CurrentCars[m_CarIndex].name;
    }

    public void OpenTrail()
    {
        m_TrailMenu.gameObject.SetActive(true);
        m_TrailMenu.RefreshUI(m_CurrentScene.trailData);
    }

    public void SelectTrail(string name)
    {
        GameManager.current.ReloadTrail(name);
    }

    private void OnDisable()
    {
        m_TrailMenu.gameObject.SetActive(false);

    }
}
