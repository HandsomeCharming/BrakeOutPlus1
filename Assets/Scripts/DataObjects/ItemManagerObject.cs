using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSpawnData
{
    public float ActivateScore;
    public int ItemFloorGap;
    public ItemType[] Items;
}

[System.Serializable]
public class ItemData
{
    public float[] m_AutoPilotTimePerLevel;
    public float[] m_MagnetTimePerLevel;
    public float[] m_ShieldTimePerLevel;
    public float[] m_TimeSlowTimePerLevel;
    public float[] m_DoubleScoreTimePerLevel;
}


[System.Serializable]
public class ItemPrefabData
{
    public ItemType m_Type;
    public GameObject m_Prefab;
}

[System.Serializable]
public class ItemPriceData
{
    public ItemType m_Type;
    public int[] m_Prices;
    public int m_DefaultLevel;
}


[CreateAssetMenu(fileName = "ItemManagerData", menuName = "Custom/ItemData", order = 1)]
public class ItemManagerObject : ScriptableObject {
    public ItemSpawnData[] m_SpawnData;
    public ItemData m_ItemData;
    public ItemPrefabData[] m_ItemPrefabs;
    public ItemPriceData[] m_ItemPrices;

    public ItemPriceData GetPriceDataByType(ItemType type)
    {
        for(int i=0; i<m_ItemPrices.Length; ++i)
        {
            if (type == m_ItemPrices[i].m_Type)
                return m_ItemPrices[i];
        }
        return null;
    }

    public int GetPriceByTypeAndLevel(ItemType type, int level)
    {
        if (level == 5) return 0;
        ItemPriceData data = GetPriceDataByType(type);
        return data.m_Prices[level];
    }
}
