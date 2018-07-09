using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : UIBase
{
    public static InGameUI Instance;

    public Text raceScore;
    public Text highScore;
    public Text coinNumbers;
    public Button pause;
    public Image BoostImage;
    public Text BoostNumber;
    public Image[] m_HUDs;
    public Sprite m_ShieldImage;
    public Sprite m_DoubleScoreImage;
    public Sprite m_MagnetImage;
    public Sprite m_AutopilotImage;
    public Sprite m_TimeslowImage;
    public Text LevelText;

    public GameObject[] m_Controls;

    public List<Powerups> m_Powerups;

    float m_MultiUpdatedTime = 1.5f;
    float m_MultiUpdatedTimeRemain = 0;
    float m_BigShakeGap;

    Color transWhite = new Color(1.0f, 1.0f, 1.0f, 0.85f);
    Color transWhite2 = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    Color transYellow = new Color(1.0f, 0.92f, 0.016f, 0.85f);

    // Use this for initialization
    void Awake() {
        //DoubleScore.gameObject.SetActive(false);
        Instance = this;
        m_Powerups = new List<Powerups>();
    }

    private void OnEnable()
    {
        pause.onClick.RemoveAllListeners();
        pause.onClick.AddListener(Pause);
        ShowControlByInputHandler();
    }

    private void OnDisable()
    {
        HideControls();
    }

    void HideControls()
    {
        foreach (GameObject go in m_Controls)
        {
            go.SetActive(false);
        }
    }

    void ShowControlByInputHandler()
    {
        ControlSchemes scheme = InputHandler.current == null ? ControlSchemes.SingleHand : InputHandler.current.m_ControlScheme;

        foreach(GameObject go in m_Controls)
        {
            go.SetActive(false);
        }
        int index = ((int)scheme) - 1;
        if(index < m_Controls.Length)
        {
            m_Controls[index].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update() {
        if (GameManager.current)
        {
            if (GameManager.current.state == GameManager.GameState.Running)
            {
                raceScore.text = ((int)GameManager.current.gameScore).ToString();
				highScore.text = "BEST:" + ((int)GameManager.current.gameHighScore).ToString();
                coinNumbers.text = GameManager.current.GetCoinCount().ToString();

                if (m_MultiUpdatedTimeRemain > 0)
                {
                    m_MultiUpdatedTimeRemain -= Time.deltaTime;
                    /*if (m_MultiUpdatedTimeRemain <= 0 && BoostText.enabled)
                    {
                        //BoostText.enabled = false;
                        //BoostNumber.enabled = false;
                        BoostNumber.GetComponent<Animator>().Play("BoostUIHide");
                        BoostText.GetComponent<Animator>().Play("BoostUIHide");
                    }*/
                }

                if (m_BigShakeGap > 0)
                {
                    m_BigShakeGap -= Time.deltaTime;
                }
            }
        }
    }

    /*public void StartDoubleScoreCountDown(float time)
    {
        DoubleScore.gameObject.SetActive(true);
        StartCoroutine(DoubleScoreCountDown(time));
    }

    IEnumerator DoubleScoreCountDown(float time)
    {
        yield return new WaitForSeconds(time);
        DoubleScore.gameObject.SetActive(false);
    }*/

    public void HideBoostUI()
    {
        if (BoostImage.enabled)
        {
            //BoostText.enabled = false;
            //BoostNumber.enabled = false;
            BoostNumber.GetComponent<Animator>().Play("BoostUIHide");
            BoostImage.GetComponent<Animator>().Play("BoostUIHide");
        }
    }

    public void StartPowerup(Powerups powerup)
    {
        if(!m_Powerups.Contains(powerup))
        {
            m_Powerups.Add(powerup);
        }
        RefreshPowerupHUD();
    }

    public void EndPowerup(Powerups powerup)
    {
        if (m_Powerups.Contains(powerup))
        {
            m_Powerups.Remove(powerup);
        }
        RefreshPowerupHUD();
    }

    public void RefreshPowerupHUD()
    {
        foreach(var image in m_HUDs)
        {
            image.gameObject.SetActive(false);
        }

        for(int i=0; i<m_Powerups.Count; ++i)
        {
            m_HUDs[i].gameObject.SetActive(true);
            m_HUDs[i].sprite = GetImageFromPowerUp(m_Powerups[i]);
        }
    }

	public void ClearAllPowerups()
	{
		m_Powerups.Clear ();
		RefreshPowerupHUD ();
	}

    Sprite GetImageFromPowerUp(Powerups powerup)
    {
        switch(powerup)
        {
            case Powerups.AutoPilot:
                return m_AutopilotImage;
            case Powerups.DoubleScore:
                return m_DoubleScoreImage;
            case Powerups.Magnet:
                return m_MagnetImage;
            case Powerups.Shield:
                return m_ShieldImage;
            case Powerups.Timeslow:
                return m_TimeslowImage;
        }
        return null;
    }

    public void UpdateBoostMultiplierNumber(float multiplier, bool increasing) //false when decreasing
    {
        m_MultiUpdatedTimeRemain = m_MultiUpdatedTime;

        if(BoostNumber.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("BoostUINothing"))
        {
            BoostImage.enabled = true;
            BoostNumber.enabled = true;
            BoostNumber.GetComponent<Animator>().Play("BoostUIShow");
            BoostImage.GetComponent<Animator>().Play("BoostUIShow");
        }
        
        float maxMult = Player.current == null?2.0f: Player.current.m_MaxMult;
		float maxDisplay = 1.0f + (maxMult - 1.0f) * 1.428f;
		float displayMult = Mathf.Lerp(1.0f, maxDisplay, (multiplier- 1.0f) / 0.7f);
		BoostNumber.text = "x" + displayMult.ToString("0.0" );

        float lerp = (multiplier - 1.0f - (increasing ? 0.0f : 1.0f)) / 1.7f;

        BoostNumber.color = Color.Lerp(transWhite2, transWhite2, lerp);
		BoostImage.color = Color.Lerp(transWhite2, transWhite2, lerp);
        if (Mathf.Abs(multiplier - Mathf.Round(multiplier)) < 0.05f && m_BigShakeGap <= 0)
        {
            BoostNumber.GetComponent<Animator>().Play("BoostUIBigShake");
            BoostImage.GetComponent<Animator>().Play("BoostUIBigShake");
            m_BigShakeGap = 0.5f;
        }
    }

    public void ShowLevelText()
    {
        LevelText.text = "Level " + GameManager.current.m_Level.ToString();
        StartCoroutine(Fade(0.5f, 0.0f, 1.0f));
        Invoke("HideLevelText", 3.0f);
    }

    public void HideLevelText()
    {
        StartCoroutine(Fade(0.5f, 1.0f, 0.0f));
    }

    IEnumerator Fade(float duration, float from, float to)
    {
        float time = 0;
        while(time < duration)
        {
            Color col = LevelText.color;
            col.a = Mathf.Lerp(from, to, time / duration);
            LevelText.color = col;
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        Color color = LevelText.color;
        color.a = Mathf.Lerp(from, to, time / duration);
        LevelText.color = color;
        
        if (to == 0.0f)
            LevelText.text = "";
    }

    void Pause()
    {
        GameManager.current.Pause(true);
    }
}
