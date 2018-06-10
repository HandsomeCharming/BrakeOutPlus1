using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxManager : MonoBehaviour {

    public static LootBoxManager instance;

    public LootCardUI m_LootCardUI;
    public LootBoxPoolObject m_LootBoxPoolObject;
    public LootBoxPoolObject m_BigBoxLootBoxPoolObject;
    public CarLootBoxData m_NormalCarPoolObject;
    public CarLootBoxData m_BigBoxCarPoolObject;
    public ReceiveItemUI m_ReceiveItem;
    public GameObject m_Buttons;
    public GameObject m_ExitButton;
	public LootBoxAdButton m_AdButton;
    public CanvasGroup m_CanvasGroup;

    [Header("Car loot debug Settings")]
    public int ReceivedCarIndex;
    public int ReceivedSceneIndex;
    public bool showReceiveCar = false;

    int m_CurrentPrizeIndex;
    int m_ChancesLeft;
    bool m_Looting = false;

    const int oneLootPrice = 50;
    const int fiveLootPrice = 220;

    // Use this for initialization
    void Start () {
        instance = this;

        m_CurrentPrizeIndex = -1;
        m_Looting = false;
    }

    void OnEnable()
    {
        StartCoroutine(FadeIn(UIManager.UIFadeInTime));

        UnityEngine.Analytics.AnalyticsEvent.ScreenVisit("LootBox");
    }

    IEnumerator FadeIn(float totalTime)
    {
        float time = 0;

        while (time < totalTime)
        {
            m_CanvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, time / totalTime);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        m_CanvasGroup.alpha = 1.0f;
    }

    private void Update()
    {
        if(showReceiveCar)
        {
            showReceiveCar = false;
            //var carData = CarSelectDataReader.Instance.GetCarData(ReceivedCarName);
            CarShortData data = new CarShortData();
            data.carIndex = ReceivedCarIndex;
            data.sceneIndex = ReceivedSceneIndex;
            m_ReceiveItem.ReceiveCar(data);
        }
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
                ResetButtonsOrLootAgain();
                break;
            case LootBoxPrizeType.Star:
                int stars = Random.Range(prize.m_Count.min, prize.m_Count.max) * prize.m_Multiplier;
                GameManager.current.AddStar(stars);
                ResetButtonsOrLootAgain();
                break;
            case LootBoxPrizeType.Car:
                var carData = GetCarDataFromCarLootBoxRandom(m_NormalCarPoolObject);
                StartCoroutine(ReceiveCar(carData, 0.7f));
                break;
            default:
                ResetButtonsOrLootAgain();
                break;
        }
    }

    IEnumerator ReceiveCar(CarShortData data, float time)
    {
        yield return new WaitForSeconds(time);
        m_ReceiveItem.ReceiveCar(data);
        m_ReceiveItem.m_CloseAction += CloseReceiveCarPanel;
    }

    public void CloseReceiveCarPanel()
    {
        ResetButtonsOrLootAgain();
    }

    void ResetButtonsOrLootAgain()
    {
        m_ChancesLeft--;
        m_Looting = false;
        if (m_ChancesLeft <= 0)
        {
            m_Buttons.SetActive(true);
            m_ExitButton.SetActive(true);
        }
        else
        {
            StartLoot();
        }
    }

	public void StartLootAdRewarded()
	{
		StartLoot ();
		m_AdButton.ResetTimerWhenLootSuccess ();
	}

    public bool StartLootAd()
    {
		return AdManager.Instance.ShowLootboxVideo ();
    }

    public void StartOneLootWithStar()
    {
        if(GameManager.current.gameStars > oneLootPrice)
        {
            GameManager.current.AddStar(-oneLootPrice);
            m_ChancesLeft = 1;
            StartLoot();

            UnityEngine.Analytics.AnalyticsEvent.ScreenVisit("OneLoot");
        }
    }

    public void StartFiveLootWithStar()
    {
        if (GameManager.current.gameStars > fiveLootPrice)
        {
            GameManager.current.AddStar(-fiveLootPrice);
            m_ChancesLeft = 5;
            StartLoot();

            UnityEngine.Analytics.AnalyticsEvent.ScreenVisit("FiveLoot");
        }
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
