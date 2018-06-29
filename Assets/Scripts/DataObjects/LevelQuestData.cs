using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "LevelQuestDataObject", menuName = "Custom/LevelQuestData", order = 1)]
public class LevelQuestData : ScriptableObject {
    public Quest[] questPerLevel;

    public int GetMaxLevel()
    {
        return questPerLevel.Length + 1;
    }
}
