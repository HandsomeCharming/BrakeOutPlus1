using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : UIBase
{
    public Text raceScore;
    public Text highScore;
    public Text coinNumbers;
    public Button pause;
    public Image BoostImage;
    public Text BoostNumber;
    public Text DoubleScore;

    public GameObject[] m_Controls;

    float m_MultiUpdatedTime = 1.5f;
    float m_MultiUpdatedTimeRemain = 0;
    float m_BigShakeGap;

    Color transWhite = new Color(1.0f, 1.0f, 1.0f, 0.85f);
    Color transWhite2 = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    Color transYellow = new Color(1.0f, 0.92f, 0.016f, 0.85f);

    // Use this for initialization
    void Awake() {
        //DoubleScore.gameObject.SetActive(false);
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
				highScore.text = ((int)GameManager.current.gameHighScore).ToString();
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

        // hardcoded for 1.7 max mult here, change if max mult change
        float displayMult = Mathf.Lerp(1.0f, 2.0f, (multiplier- 1.0f) / 0.7f);
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

    void Pause()
    {
        GameManager.current.Pause(true);
    }
}
