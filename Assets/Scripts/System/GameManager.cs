using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class GameManager : MonoBehaviour {

    public static GameManager current;
    public static int m_NextAdTime = 0;

    public bool m_LoadFromAppManager = true;
    public GameObject m_PlayerPrefab;
    public GameObject player;

    public float gameScore;
    public float scoreForDifficulty;
    public float scoreForDiffScale = 0.2f;
    public bool shouldUseScoreForDiff = true;
    public DiffScoreUsage DiffScoreOption;

	public int gameHighScore;
    public int gameCoins;
    public int gameStars;
    public int colliNum;
	public int cldIndex;
	public int cIndex;
    public float m_BoostMultiplier;
    public float m_DiffMultiplier;
    public float m_ItemMultiplier;
    public float m_GlobalMultiplier;
    public GameObject[] m_InitSequence;

    [HideInInspector]
    public int m_ReviveCount;

    float m_OldTimescale;
    float m_SlowmotionFactor;

    public string highScoreLastDate;
    public int dayHighScore;
    const string LastDateName = "HSLastDate";
    const string DayHighScoreName = "DayHighScore";

    public enum GameState
    {
        Start,
        Menu,
        AssembleTrack,
        CutScene,
        Paused,
        ReviveMenu,
        Dead,
        Running
    }

    public enum DiffScoreUsage
    {
        Normal,
        ScaleOffHighest,
        HAHA
    }



    public GameState state;

    public static GameState GetGameState()
    {
        if (current) return current.state;
        return GameState.Start;
    }
    
    void Awake()
    {
        current = this;
        //Init App Manager
        if(AppManager.instance == null)
        {
            GameObject go = new GameObject();
            go.AddComponent<AppManager>();
            go.name = "AppManager";
        }

        //Init Game save
        LoadGameSave();

        Application.targetFrameRate = 60;
        //QualitySettings.antiAliasing = 0;
        //QualitySettings.shadowCascades = 2;
        //QualitySettings.shadowDistance = 150;
        gameHighScore = PlayerPrefs.GetInt("High Score");
        if (DiffScoreOption == DiffScoreUsage.ScaleOffHighest)
            scoreForDifficulty = gameHighScore * scoreForDiffScale;
        else if(DiffScoreOption == DiffScoreUsage.Normal)
            scoreForDifficulty = 0;
        // else is haha
            
        print(scoreForDifficulty);
        Time.timeScale = 1.0f;
        m_SlowmotionFactor = 1.0f;
        m_OldTimescale = 1.0f;
        state = GameState.AssembleTrack;
        m_BoostMultiplier = 1.0f;
        m_DiffMultiplier = 1.0f;
        m_ItemMultiplier = 1.0f;

        if (m_LoadFromAppManager || m_PlayerPrefab == null)
            m_PlayerPrefab = (GameObject) Resources.Load(AppManager.instance.m_CarPrefabName);
        player = Instantiate(m_PlayerPrefab);
        //ChallengeManager.current.currentFloorData = 0;

        foreach (GameObject obj in m_InitSequence)
        {
            obj.SetActive(true);
        }

        m_ReviveCount = 0;
        //Show ad every 3 games
        /*m_NextAdTime--;
        if(m_NextAdTime < 0)
        {
            m_NextAdTime = 1;
            //AdManager.Instance.ShowBannerAd();
        }*/
    }

    public void ReloadCar()
    {
        if (m_LoadFromAppManager || m_PlayerPrefab == null)
            m_PlayerPrefab = (GameObject)Resources.Load(AppManager.instance.m_CarPrefabName);
        if (player) Destroy(player);
        player = Instantiate(m_PlayerPrefab);
    }

    public void ReloadCar(GameObject carPrefab)
    {
        if (player) Destroy(player);
        player = Instantiate(carPrefab);
    }

    public void ChangeBackground(BackgroundEnum back)
    {
        BackgroundManager.current.ChangeBackground(back);
    }

    public void AddCoin(int coinCount)
    {
        gameCoins += coinCount;
        ConsistantUI.UpdateCoinAndStar();

        if(state != GameState.Running)
        {
            SaveGame();
        }
    }

    public void AddStar(int starCount)
    {
        gameStars += starCount;
        ConsistantUI.UpdateCoinAndStar();

        if (state != GameState.Running)
        {
            SaveGame();
        }
    }

    public int GetCoinCount()
    {
        return gameCoins;
    }

    public void StartGame()
    {
        if (GameManager.current.state == GameManager.GameState.Start)
        {
            Player.current.Launch();
            //Player.current.playerState = Player.PlayerState.Playing;
            print("Running");
            state = GameState.Running;
            Time.timeScale = 1.0f;
            ChallengeManager.current.startTime = Time.time;
            ChallengeManager.current.getHardTimeRemain = 15.0f;
            UIManager.current.ChangeStateByGameState();
        }
    }

    public void ChangeScene()
    {

    }

    public void StartCutScene()
    {
        state = GameState.CutScene;
    }

    public void AddScore(int newCollidedCount)
    {
        float add = m_DiffMultiplier * (m_BoostMultiplier * (float)newCollidedCount) * m_ItemMultiplier * m_GlobalMultiplier;
        gameScore += add;
        scoreForDifficulty += add;

        AudioSystem.current.SetScore(scoreForDifficulty);
    }

	public void SetHighScore()
    {
		if (gameScore > gameHighScore) {
			gameHighScore = (int)gameScore;
			PlayerPrefs.SetInt ("High Score",gameHighScore);
            PlayerPrefs.Save();
		}
        if(gameScore > dayHighScore)
        {
            SetDayHighScore();
        }
	}

    void SetDayHighScore()
    {
        dayHighScore = (int)gameScore;
        PlayerPrefs.SetInt(DayHighScoreName, dayHighScore);
        PlayerPrefs.Save();

        NetworkManager.current.SendDailyHighScore(dayHighScore);
    }

    public void SetItemMultiplier(float mult, float time)
    {
        m_ItemMultiplier = mult;
        Invoke("ResetItemMultiplier", time);
    }

    public void ResetItemMultiplier()
    {
        m_ItemMultiplier = 1.0f;
    }

    public void EndGame()  //When player dies
    {
        SaveGame();
        if(m_ReviveCount < 3)
        {
            state = GameState.ReviveMenu;
            UIManager.current.ChangeStateByGameState();
        }
        else
        {
            state = GameState.Dead;
            UIManager.current.ChangeStateByGameState();
            SetHighScore();
        }
    }

    public void LoadGameSave()
    {
        print(Application.persistentDataPath);
        SaveManager.GetInstance().Load();
        GameSave save = SaveManager.GetInstance().m_Data;

        gameHighScore = save.highScore;
        gameCoins = save.coin;
        gameStars = save.star;

        // Daily score
        RefreshDailyHighScore();
    }

    void RefreshDailyHighScore()
    {
        print(System.DateTime.Now.ToString("dd/MM"));
        highScoreLastDate = PlayerPrefs.GetString(LastDateName, "");
        if (highScoreLastDate == "")
        {
            dayHighScore = 0;
            PlayerPrefs.SetString(LastDateName, System.DateTime.Today.ToString("dd/MM"));
            PlayerPrefs.SetInt(DayHighScoreName, 0);
        }
        else
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            System.DateTime date = System.DateTime.ParseExact(highScoreLastDate, "dd/MM", provider);
            if(System.DateTime.Today > date)
            {
                // refresh
                dayHighScore = 0;
                PlayerPrefs.SetString(LastDateName, System.DateTime.Today.ToString("dd/MM"));
                PlayerPrefs.SetInt(DayHighScoreName, 0);
            }
            else
            {
                dayHighScore = PlayerPrefs.GetInt(DayHighScoreName);
            }
        }
        PlayerPrefs.Save();
    }

    public void SaveGame()
    {
        SaveManager.GetInstance().Save();
    }

    public void SkipRevive()
    {
        state = GameState.Dead;
        UIManager.current.ChangeStateByGameState();
        SetHighScore();
    }

    public void ShowReviveVideo()
    {
        if (Application.isMobilePlatform)
            AdManager.Instance.ShowVideoAd("Revive");
        else
            RevivePlayer();
    }

    public void RevivePlayer()
    {
        state = GameState.Running;
        m_ReviveCount++;
        InputHandler.current.ResetControls();
        //FloorBuilder.current.RebuildFloor();
        Player.current.Revive();
        UIManager.current.ChangeStateByGameState();
        //UIManager.current.m_Ingame.StartReviveCountDown(3.0f);
    }

    public void ReloadAfterDelay(float delay)
    {
        StartCoroutine(ReloadAfterDelayIE(delay));
    }

    IEnumerator ReloadAfterDelayIE(float delay)
    {
        yield return new WaitForSeconds(delay);
        Reload();
    }

    public void Reload()
    {
        IDictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("Score", (int)gameScore);
        AnalyticsResult result = UnityEngine.Analytics.Analytics.CustomEvent("Player Score", dic);
        print(result);

        SaveGame();
        SetHighScore();

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void SetNormalTimeScale(float timeScale)
    {
        m_OldTimescale = timeScale;
        // Change it when it's not slow motion
        if (Time.timeScale >= 1.0f)
        {
            Time.timeScale = m_OldTimescale;
        }
    }

    public void AddNormalTimeScale(float addedTimeScale)
    {
        m_OldTimescale += addedTimeScale;
        // Change it when it's not slow motion
        if(Time.timeScale >= 1.0f)
        {
            Time.timeScale = m_OldTimescale;
        }
    }

    public void TokiyoTomare(float slowdownFactor = 0.5f)
    {
        m_SlowmotionFactor = slowdownFactor;
        Time.timeScale = m_OldTimescale * m_SlowmotionFactor;
    }

    public void Pause(bool pause)
    {
        if(pause == true)
        {
            Time.timeScale = 0;
            state = GameState.Paused;
        }
        else
        {
            Time.timeScale = m_OldTimescale * m_SlowmotionFactor;
            state = GameState.Running;
        }
        print(state);
        UIManager.current.ChangeStateByGameState();
    }
	
	// Update is called once per frame
	void Update () {
	}
}
