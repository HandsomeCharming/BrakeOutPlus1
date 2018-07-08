using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDispenser : MonoBehaviour {

    public static LevelQuestData LevelQuestStorer
    {
        get
        {
            if (m_LevelQuestStorer == null)
            {
                m_LevelQuestStorer = (LevelQuestData)Resources.Load(LevelQuestDataPath);
            }
            return m_LevelQuestStorer;
        }
        set
        { }
    }
    static LevelQuestData m_LevelQuestStorer;

    public static DailyQuestDataObject DailyQuestStorer
    {
        get
        {
            if (m_DailyQuestStorer == null)
            {
                m_DailyQuestStorer = (DailyQuestDataObject)Resources.Load(DailyQuestDataPath);
            }
            return m_DailyQuestStorer;
        }
        set
        { }
    }
    static DailyQuestDataObject m_DailyQuestStorer;

    const string LevelQuestDataPath = "ScriptableObjects/LevelQuestDataObject";
    const string DailyQuestDataPath = "ScriptableObjects/DailyQuestDataObject";

    public static Quest GetLevelQuest(int level)
    {
        if (level < LevelQuestStorer.questPerLevel.Length)
        {
            Quest res = DeepCopyer.DeepClone(LevelQuestStorer.questPerLevel[level]);
            return res;
        }
        else
            return null;
    }

    public static Quest GetRandomDailyQuest()
    {
        int index = Random.Range(0, DailyQuestStorer.randomQuests.Length);
        DailyQuestSingleData dailyQuestData = DailyQuestStorer.randomQuests[index];
        Quest quest = GetQuestFromDailyQuestSingleData(dailyQuestData);

        return quest;
    }

    public static Quest GetQuestFromDailyQuestSingleData(DailyQuestSingleData dailyQuestData)
    {
        Quest quest = new Quest();
        quest.action = dailyQuestData.action;
        quest.currency = Random.value < 0.5f ? Currency.Coin : Currency.Star;

        float lerpAmount = Random.value;
        float roundedLerpAmount = dailyQuestData.actionCount.GetRoundedLerpAmount(lerpAmount, dailyQuestData.actionGap);
        quest.targetCount = dailyQuestData.actionCount.GetBetweenRangeWithGap(lerpAmount, dailyQuestData.actionGap);

        quest.currentCount = 0;
        quest.rewardCount = (quest.currency == Currency.Star) ?
            dailyQuestData.rewardStar.GetBetweenRange(roundedLerpAmount) :
            dailyQuestData.rewardCoin.GetBetweenRange(roundedLerpAmount);

        return quest;
    }

}
