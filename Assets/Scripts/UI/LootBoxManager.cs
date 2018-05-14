using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxManager : MonoBehaviour {

    public LootCardUI m_LootCardUI;
    public LootBoxPoolObject m_LootBoxPoolObject;
    public LootBoxPoolObject m_BigBoxLootBoxPoolObject;
    public CarLootBoxData m_NormalCarPoolObject;
    public CarLootBoxData m_BigBoxCarPoolObject;
    public GameObject m_Buttons;
    public GameObject m_ExitButton;

    int m_CurrentPrizeIndex;
    bool m_Looting = false;
    
    // Use this for initialization
    void Start () {
        m_CurrentPrizeIndex = -1;
        m_Looting = false;
    }

    public void StartLoot()
    {
        if (m_Looting) return;
        m_Looting = true;
        m_Buttons.SetActive(false);
        m_ExitButton.SetActive(false);
        int prizeIndex = GetPrizeIndexFromLootBoxRandom(m_LootBoxPoolObject);
        m_CurrentPrizeIndex = prizeIndex;

        m_LootCardUI.AnimateToLootBox(prizeIndex);
    }
    
    public void FinishAnim()
    {
        var prize = m_LootBoxPoolObject.prizes[m_CurrentPrizeIndex];
        switch(prize.m_Type)
        {
            case LootBoxPrizeType.Coin:
                int coins = Random.Range(prize.m_Count.min, prize.m_Count.max) * prize.m_Multiplier;
                GameManager.current.AddCoin(coins);
                break;
            case LootBoxPrizeType.Star:
                int stars = Random.Range(prize.m_Count.min, prize.m_Count.max) * prize.m_Multiplier;
                GameManager.current.AddStar(stars);
                break;
            default:
                break;
        }

        m_Buttons.SetActive(true);
        m_ExitButton.SetActive(true);
        m_Looting = false;
    }

    public void StartLootAd()
    {
        StartLoot();
    }

    public void StartOneLootWithStar()
    {
        StartLoot();
    }

    public void StartFiveLootWithStar()
    {
        StartLoot();
    }

    int GetPrizeIndexFromLootBoxRandom(LootBoxPoolObject lootBoxPool)
    {
        int prizeIndex = 0;
        float rand = Random.value;
        var prizes = lootBoxPool.prizes;
        for (int i = 0; i < prizes.Length; ++i)
        {
            if (rand <= prizes[i].m_Probability)
            {
                prizeIndex = i;
                break;
            }
        }
        return prizeIndex;
    }
    
    CarShortData GetCarDataFromCarLootBoxRandom(CarLootBoxData carLootBox)
    {
        int prizeIndex = 0;
        float rand = Random.value;
        var prizes = carLootBox.prizes;
        for (int i = 0; i < prizes.Length; ++i)
        {
            if (rand <= prizes[i].m_Probability)
            {
                prizeIndex = i;
                break;
            }
        }

        CarShortData data = new CarShortData();
        data.carIndex = prizes[prizeIndex].m_CarIndex;
        data.sceneIndex = prizes[prizeIndex].m_SceneIndex;
        return data;
    }
}
