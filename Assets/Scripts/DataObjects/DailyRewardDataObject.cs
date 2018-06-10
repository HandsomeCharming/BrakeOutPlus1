using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DailyLoginPrizeType
{
    Coin,
    Star,
    Car,
    Box
}

[System.Serializable]
public class DailyRewardData
{
    public DailyLoginPrizeType type;
    public int count;
    public int carIndex;
    public int sceneIndex;
}


[CreateAssetMenu(fileName = "DailyRewardDataObject", menuName = "Custom/DailyReward", order = 1)]
public class DailyRewardDataObject : ScriptableObject
{
    public DailyRewardData[] data;
}
