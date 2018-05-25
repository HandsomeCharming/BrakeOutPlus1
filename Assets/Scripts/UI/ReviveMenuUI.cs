using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveMenuUI : UIBase {

    public Button starRevive1;
    public Button starRevive2;
    public Button starRevive4;
    public Button adRevive;
    public Button skip;
    public Text countdown;
    public Text tap;

    public float m_Countdown;

    void Awake() {
    }

    private void OnEnable()
    {
        skip.onClick.RemoveAllListeners();
        skip.onClick.AddListener(SkipRevive);
        skip.gameObject.SetActive(false);
        if(GameManager.current.m_ReviveCount == 0)
        {
            adRevive.gameObject.SetActive(true);
            starRevive2.gameObject.SetActive(false);
            starRevive4.gameObject.SetActive(false);
            starRevive1.gameObject.SetActive(true);
            adRevive.onClick.RemoveAllListeners();
            adRevive.onClick.AddListener(AdRevive);
            starRevive1.onClick.RemoveAllListeners();
            starRevive1.onClick.AddListener(StarRevive);
        }
        else if(GameManager.current.m_ReviveCount == 1)
        {
            adRevive.gameObject.SetActive(false);
            starRevive1.gameObject.SetActive(false);
            starRevive4.gameObject.SetActive(false);
            starRevive2.gameObject.SetActive(true);
            starRevive2.onClick.RemoveAllListeners();
            starRevive2.onClick.AddListener(StarRevive);
        }
        else if (GameManager.current.m_ReviveCount == 2)
        {
            adRevive.gameObject.SetActive(false);
            starRevive1.gameObject.SetActive(false);
            starRevive2.gameObject.SetActive(false);
            starRevive4.gameObject.SetActive(true);
            starRevive4.onClick.RemoveAllListeners();
            starRevive4.onClick.AddListener(StarRevive);
        }
        StartCoroutine(FadeInSkip());

        m_Countdown = 5.0f;
    }

    private void Update()
    {
        m_Countdown -= Time.deltaTime;
        countdown.text = ((int)Mathf.Ceil(m_Countdown)).ToString();

        if (m_Countdown < 0.0f)
        {
            SkipRevive();
        }
    }

    IEnumerator FadeInSkip()
    {
        float fadeTime = 0.5f;
        // fade from opaque to transparent
        for (float i = 0; i <= fadeTime; i += Time.deltaTime)
        {
            // set color with i as alpha
            tap.color = new Color(1, 1, 1, i/fadeTime);
            yield return new WaitForEndOfFrame();
        }
        skip.gameObject.SetActive(true);
    }

    void SkipRevive()
    {
        GameManager.current.SkipRevive();
    }

    void StarRevive()
    {
        //To-do: add remove star
        int price = 10;
        switch(GameManager.current.m_ReviveCount)
        {
            case 0:
                price = 10;
                break;
            case 1:
                price = 20;
                break;
            case 2:
                price = 40;
                break;
            default:break;
        }
        if(GameManager.current.gameStars > price)
        {
            GameManager.current.AddStar(-price);
            GameManager.current.RevivePlayer();
        }
    }

    void AdRevive()
    {
        GameManager.current.ShowReviveVideo();
    }
}
