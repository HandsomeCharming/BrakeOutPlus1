using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCarManager : MonoBehaviour {

    public SelectMenuUI m_Menu;
    public SelectTrailUI m_TrailMenu;

    public GameObject m_TutorialCanvasGO;

    public int m_CurrentSceneIndex;

    int m_CarIndex;
    string m_CurrentTrailName;

    public CarSelectData m_Storer;

    public bool removeTutorialKey;

    SceneCars m_CurrentScene;
    List<SingleCarSelectData> m_CurrentCars;

    const string TutorialKey = "CarTutorialWatched";

	// Use this for initialization
	void Awake ()
    {
        m_Menu.m_Manager = this;
        InitCar();

        m_Menu.enabled = true;

        //m_CurrentCarName = m_CarNames[m_CarIndex];
        //m_Menu.SetText(m_CurrentCarName);
    }

    private void Update()
    {
        if(removeTutorialKey)
        {
            removeTutorialKey = false;
            PlayerPrefs.DeleteKey(TutorialKey);
            PlayerPrefs.Save();
        }
    }

    bool HasSawTutorial()
    {
        return PlayerPrefs.HasKey(TutorialKey);
    }

    public void SawTutorial()
    {
        PlayerPrefs.SetInt(TutorialKey, 1);
        PlayerPrefs.Save();
        m_TutorialCanvasGO.SetActive(false);
    }

    private void OnEnable()
    {
        //InitCar();
        UnityEngine.Analytics.AnalyticsEvent.ScreenVisit("CarMenu");

        if (!HasSawTutorial())
        {
            m_TutorialCanvasGO.SetActive(true);
        }
    }

    void InitCar()
    {
        m_CarIndex = GameManager.current.m_DefaultCarIndex;
        m_CurrentSceneIndex = GameManager.current.m_DefaultSceneIndex;
        m_CurrentScene = m_Storer.sceneData[m_CurrentSceneIndex];
        m_CurrentCars = m_CurrentScene.carData;
        SelectScene(m_CurrentSceneIndex, true, m_CarIndex);
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
            GameManager.current.ReloadDefaultTrail();
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
            GameManager.current.ReloadDefaultTrail();
            //GameManager.current.SetDefaultCar(m_CarIndex, m_CurrentSceneIndex);
        }
    }

    public void SelectScene(int index)
    {
        SelectScene(index, false);
    }

    public void SelectScene(int index, bool forced, int carIndex = 0)
    {
        if (m_CurrentSceneIndex == index && !forced) return;
        m_CurrentSceneIndex = index;
        m_CurrentScene = m_Storer.sceneData[m_CurrentSceneIndex];
        m_CurrentCars = m_CurrentScene.carData;
        m_CarIndex = carIndex;
        if (isCurrentCarAvailable())
        {
            GameManager.current.ReloadCar(m_CarIndex, m_CurrentSceneIndex);
            GameManager.current.ReloadTrail(GetCurrentCarData().m_Trails[0].name);
        }
        BackgroundEnum back = (BackgroundEnum)m_CurrentSceneIndex;
        GameManager.current.ChangeBackground(back);
        m_Menu.RefreshForManager();
        m_Menu.ChangeScene(m_CurrentSceneIndex);
    }

    public void BuyCurrentCarWithCoin()
    {
        if(SaveManager.instance.BuyCarWithCoin(m_CarIndex, m_CurrentSceneIndex))
        {
            SaveManager.instance.Save();
            m_Menu.RefreshForManager();
            GameManager.current.ReloadCar(m_CarIndex, m_CurrentSceneIndex);
        }
    }

    public void BuyCurrentCarWithStar()
    {
        if (SaveManager.instance.BuyCarWithStar(m_CarIndex, m_CurrentSceneIndex))
        {
            SaveManager.instance.Save();
            m_Menu.RefreshForManager();
            GameManager.current.ReloadCar(m_CarIndex, m_CurrentSceneIndex);
        }
    }


    public string GetCurrentCarName()
    {
        return m_CurrentCars[m_CarIndex].name;
    }

    public void UpgradeCurrentCar(CarUpgradeCatagory type)
    {
        SaveManager.instance.BuyCarUpgrade(GetCurrentCarData().name, type);
        m_Menu.RefreshUpgradeCards();
    }

    public void OpenTrailMenu()
    {
        m_Menu.OpenTrailMenu();

        m_TrailMenu.gameObject.SetActive(true);
        m_TrailMenu.RefreshUI(m_CurrentScene.trailData);

        UnityEngine.Analytics.AnalyticsEvent.ScreenVisit("TrailMenu");
    }

    public void CloseTrailMenu()
    {
        m_Menu.CloseTrailMenu();
    }

    public void SelectTrail(string name)
    {
        GameManager.current.ReloadTrail(name);
        m_TrailMenu.RefreshUI(m_CurrentScene.trailData);
    }

    private void OnDisable()
    {
        m_TrailMenu.gameObject.SetActive(false);

    }
}
