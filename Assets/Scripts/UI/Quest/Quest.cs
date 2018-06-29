using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestSave
{
    public Quest levelQuest;
    public List<Quest> m_Quests;

    public QuestSave()
    {
        m_Quests = new List<Quest>();
    }
}

public enum QuestAction
{
    Play,
    ReachScore,
    LeapGap,
    UpgradeCar,
    UpgradeCarToMax,
    OpenChest,
    UpgradeItem,
    Glide,
    CrushCube
}

[System.Serializable]
public class Quest {
    public QuestAction action;
    public int targetCount;
    public int currentCount = 0;
    public Currency currency;
    public int rewardCount;
}
