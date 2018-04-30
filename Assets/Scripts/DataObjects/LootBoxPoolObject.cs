using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LootBoxPrizeType
{
    Coin,
    Star,
    Car,
    Trail,
    BigBox
}

[System.Serializable]
public class LootBoxPrizeData
{
    public LootBoxPrizeType m_Type;
    public MinMaxDataInt m_Count; // Only for coin and star
    public int m_Multiplier;
    [Range(0.0f, 1.0f)]
    public float m_Probability;
}

[CreateAssetMenu(fileName = "LootBoxData", menuName = "Custom/LootBoxData", order = 1)]
public class LootBoxPoolObject : ScriptableObject
{
    public LootBoxPrizeData[] prizes;
}

