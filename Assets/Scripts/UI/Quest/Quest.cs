using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestSave
{
    public Quest levelQuest;
    public List<Quest> m_Quests;
    public int currentLevel = 0;

    public QuestSave()
    {
        m_Quests = new List<Quest>();
    }
}

public enum QuestAction
{
    Play, // done
    ReachScore, // done
    LeapGap, // done
    UpgradeCar, // done
    UpgradeCarToMax, // done
    OpenChest, // done
    UpgradeItem, // done
    Glide, // done
    CrushCube, // done
    UpgradeItemToMax // done
}

[System.Serializable]
public class Quest {
    public QuestAction action;
    public int targetCount;
    public int currentCount = 0;
    public Currency currency;
    public int rewardCount;

    public bool IsFinished()
    {
        return currentCount >= targetCount;
    }
}
