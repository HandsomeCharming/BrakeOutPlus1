using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadMenuUI : UIBase
{
    public static DeadMenuUI current;

    public Button CollectButton;
    public Button DoubleCollectButton;
    public Text m_CoinEarned;
    public Text m_Score;
    public QuestProgressInDeadMenu m_QuestProgress;

    public RectTransform Group1;
    public RectTransform Group2;

    readonly Vector2[] questGroupYs = { new Vector2(-80,0), new Vector2(-45,-30), new Vector2(-45, -90), new Vector2(10, -90)};

    // Use this for initialization
    void Awake () {
        current = this;

        CollectButton.onClick.RemoveAllListeners();
        CollectButton.onClick.AddListener(ReloadGameAndCollect);
        DoubleCollectButton.onClick.RemoveAllListeners();
        DoubleCollectButton.onClick.AddListener(TryShowDoubleVideo);
    }

    private void OnEnable()
    {
        RefreshUI();
        if(QuestManager.ShouldFinishLevelQuestInDeadMenu())
            QuestManager.current.TryFinishLevelQuest();
        QuestManager.current.TryFinishDailyQuests();
    }

    public void ReloadGameAndCollect()
    {
        GameManager.current.CollectInGameCoins();
        GameManager.current.Reload();
    }

    void RefreshUI()
    {
        m_Score.text = ((int)GameManager.current.gameScore).ToString();
        m_CoinEarned.text = ((int)GameManager.current.singleGameCoins).ToString();

        int questCount = QuestManager.GetActiveQuestCount();
        Vector2 group1Pos = Group1.anchoredPosition;
        Vector2 group2Pos = Group2.anchoredPosition;
        group1Pos.y = questGroupYs[questCount].x;
        group2Pos.y = questGroupYs[questCount].y;
        Group1.anchoredPosition = group1Pos;
        Group2.anchoredPosition = group2Pos;

        m_QuestProgress.ShowQuestProgress();
    }

    void TryShowDoubleVideo()
    {
        if (!AdManager.Instance.ShowDoubleCollectVideo())
        {
            ReloadGameAndCollect();
        }
    }

    public void ReloadGameAndCollectDouble()
    {
        GameManager.current.CollectInGameCoins(true);
        GameManager.current.Reload();
    }
}
