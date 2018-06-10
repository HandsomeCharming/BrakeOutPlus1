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
    public GameObject m_ButtonAfterClaim;

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

            m_ClaimButtonBase.color = Color.white;
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
    }

    public void ClaimReward()
    {
        m_ClaimButtonBase.color = Color.gray;
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
            var rewardData = dailyRewardData.data[rewardCount];

            ReceiveRewardByData(rewardData);
        }
        else
        {
            ReceiveRewardBybox();
        }
        RecordManager.RecordInt(PastRewardCountKey, rewardCount);
        RecordLatestRewardDate();

        Invoke("EnableButtonToClose", 1.0f);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    void EnableButtonToClose()
    {
        m_ButtonAfterClaim.SetActive(true);
    }

    bool CanReceiveReward()
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

    void ReceiveRewardByData(DailyRewardData data)
    {
        switch(data.type)
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
