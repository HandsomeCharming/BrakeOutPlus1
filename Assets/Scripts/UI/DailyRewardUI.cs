using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardUI : MonoBehaviour {

    public DailyRewardDataObject dailyRewardData;
    public LootBoxPoolObject Day7BoxRewardData;
    public ReceiveItemUI m_ReceiveItemUI;
    public DailyRewardCardUI[] m_RewardCards;
    public CanvasGroup m_CanvasGroup;
    public Image m_ClaimButtonBase;
    public Text m_ClainButtonText;
    public GameObject m_ButtonAfterClaim;
    public DailyRewardCardUI m_RewardCard3B;
    public DailyRewardCardUI m_RewardCard7B;

    public int DebugDateOffset;

    const string DateKey = "DailyRewardDate";
    const string PastRewardCountKey = "DailyRewardCount";


    public void ClearRecord()
    {
        PlayerPrefs.DeleteKey(DateKey);
        PlayerPrefs.DeleteKey(PastRewardCountKey);
        PlayerPrefs.Save();
    }

    private void OnEnable()
    {
    }

    public void ShowIfCanReceiveReward()
    {
        if(CanReceiveReward())
        {
            m_CanvasGroup.alpha = 0.0f;
            gameObject.SetActive(true);

            // day 3 and 7 hard code
            if (SaveManager.instance.HasCar("TRIVEX"))
            {
                m_RewardCards[2].gameObject.SetActive(false);
                m_RewardCards[2] = m_RewardCard3B;
                m_RewardCards[2].gameObject.SetActive(true);
            }
            if (SaveManager.instance.HasCar("HELICOPTER"))
            {
                m_RewardCards[6].gameObject.SetActive(false);
                m_RewardCards[6] = m_RewardCard7B;
                m_RewardCards[6].gameObject.SetActive(true);
            }

            int rewardCount = GetRewardCount();

            foreach (var go in m_RewardCards)
            {
                go.SetSelected(false);
            }

            int maxCount = rewardCount < 7 ? rewardCount : 7;
            for(int i=0; i<maxCount; ++i)
            {
                m_RewardCards[i].SetSelected(true);
            }

            // set buttons
            m_ClaimButtonBase.color = Color.white;
            m_ClainButtonText.text = "CLAIM";
            m_ClaimButtonBase.GetComponent<Button>().enabled = true;
            m_ButtonAfterClaim.SetActive(false);

            StartCoroutine(FadeIn(UIManager.UIFadeInTime));
        }
    }

    IEnumerator FadeIn(float totalTime)
    {
        float time = 0;

        while(time < totalTime)
        {
            m_CanvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, time / totalTime);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        m_CanvasGroup.alpha = 1.0f;
        m_CanvasGroup.interactable = true;
    }
    
    IEnumerator FadeOut(float totalTime)
    {
        float time = 0;
        m_CanvasGroup.interactable = false;

        while (time < totalTime)
        {
            m_CanvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, time / totalTime);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        m_CanvasGroup.alpha = 0.0f;

        gameObject.SetActive(false);
    }

    public void ClaimReward()
    {
        //m_ClaimButtonBase.color = Color.gray;
        m_ClainButtonText.text = "CLOSE";
        m_ClaimButtonBase.GetComponent<Button>().enabled = false;

        int rewardCount = GetRewardCount();
        print(rewardCount);
        
        if (rewardCount < 7)
        {
            m_RewardCards[rewardCount].SetSelected(true);
        }
        else
        {
            m_RewardCards[7].SetSelected(true);
        }

        if (rewardCount < dailyRewardData.data.Length)
        {
            ReceiveRewardByDay(rewardCount);
        }
        else
        {
            ReceiveRewardBybox();
        }
        RecordManager.RecordInt(PastRewardCountKey, rewardCount);
        RecordLatestRewardDate();

        Invoke("EnableButtonToClose", 0.5f);
        AudioSystem.current.PlayEvent("click");
    }

    public void Close()
    {
        StartCoroutine(FadeOut(UIManager.UIFadeInTime));
    }

    void EnableButtonToClose()
    {
        m_ButtonAfterClaim.SetActive(true);
    }

    public bool CanReceiveReward()
    {
        if (RecordManager.HasRecordDate(DateKey))
        {
            DateTime lastDate = RecordManager.GetRecordDate(DateKey);
            DateTime today = DateTime.Today;
            today = today.AddDays(DebugDateOffset);
            return lastDate.Date != today.Date;
        }
        else
        {
            return true;
        }
    }

    void ReceiveRewardByDay(int rewardCount)
    {
        if(rewardCount == 2 && SaveManager.instance.HasCar("TRIVEX"))
        {
            GameManager.current.AddStar(78);
        }
        else if (rewardCount == 6 && SaveManager.instance.HasCar("HELICOPTER"))
        {
            GameManager.current.AddStar(4368);
        }
        else
        {
            var data = dailyRewardData.data[rewardCount];
            switch (data.type)
            {
                case DailyLoginPrizeType.Coin:
                    GameManager.current.AddCoin(data.count);
                    break;
                case DailyLoginPrizeType.Star:
                    GameManager.current.AddStar(data.count);
                    break;
                case DailyLoginPrizeType.Car:
                    m_ReceiveItemUI.ReceiveCar(new CarShortData(data.carIndex, data.sceneIndex));
                    break;
                case DailyLoginPrizeType.Box:
                    m_ReceiveItemUI.ReceiveCar(new CarShortData(data.carIndex, data.sceneIndex));
                    break;
            }
        }
    }

    void ReceiveRewardBybox()
    {
        int prizeIndex = 0;
        float rand = UnityEngine.Random.value;
        var prizes = Day7BoxRewardData.prizes;
        for (int i = 0; i < prizes.Length; ++i)
        {
            if (rand <= prizes[i].m_Probability)
            {
                prizeIndex = i;
                break;
            }
        }
        
        var prize = Day7BoxRewardData.prizes[prizeIndex];
        switch (prize.m_Type)
        {
            case LootBoxPrizeType.Coin:
                int coins = UnityEngine.Random.Range(prize.m_Count.min, prize.m_Count.max) * prize.m_Multiplier;
                GameManager.current.AddCoin(coins);
                break;
            case LootBoxPrizeType.Star:
                int stars = UnityEngine.Random.Range(prize.m_Count.min, prize.m_Count.max) * prize.m_Multiplier;
                GameManager.current.AddStar(stars);
                break;
        }
    }

    void RecordLatestRewardDate()
    {
        DateTime today = DateTime.Today.Date;
        today = today.AddDays(DebugDateOffset);
        RecordManager.RecordDate(DateKey, today);
    }
    
    int GetRewardCount()
    {
        int rewardCount = PlayerPrefs.GetInt(PastRewardCountKey, 0);

        if (RecordManager.HasRecordDate(DateKey))
        {
            DateTime lastDate = RecordManager.GetRecordDate(DateKey);
            DateTime today = DateTime.Today;
            today = today.AddDays(DebugDateOffset);
            if ((today.Date - lastDate.Date).Days == 1)
            {
                rewardCount++;
            }
            else
            {
                rewardCount = 0;
            }
        }

        return rewardCount;
    }
}
