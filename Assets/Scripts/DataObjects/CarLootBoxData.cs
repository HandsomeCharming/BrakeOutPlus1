using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarLootBoxPrizeData
{
    public int m_CarIndex;
    public int m_SceneIndex;
    [Range(0.0f, 1.0f)]
    public float m_Probability;
}

[CreateAssetMenu(fileName = "CarLootBoxData", menuName = "Custom/CarLootBoxData", order = 1)]
public class CarLootBoxData : ScriptableObject
{
    public CarLootBoxPrizeData[] prizes;
}
