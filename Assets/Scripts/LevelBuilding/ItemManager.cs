using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System;

public enum ItemType {
	Magnet,
	AutoPilot,
	Shield,
	TimeSlow,
    DoubleScore,
    Star
}

[System.Serializable]
public class ItemSingleSaveData
{
    public ItemType m_Type;
    public int m_Level;
}

[System.Serializable]
public class ItemSaveData
{
    public ItemSingleSaveData[] data;

    public void UseDefaultData(ItemManagerObject storer)
    {
        Array types = Enum.GetValues(typeof(ItemType));
        ItemPriceData[] priceData = storer.m_ItemPrices;
        data = new ItemSingleSaveData[priceData.Length];
        for (int i = 0; i < priceData.Length; ++i)
        {
            data[i] = new ItemSingleSaveData();
            data[i].m_Type = priceData[i].m_Type;
            data[i].m_Level = priceData[i].m_DefaultLevel;
        }
    }

    public void UpgradeItem(ItemType type)
    {
        for (int i = 0; i < data.Length; ++i)
        {
            if (data[i].m_Type == type)
            {
                if(data[i].m_Level < 5)
                {
                    data[i].m_Level++;
                    QuestManager.UpdateQuestsStatic(QuestAction.UpgradeItem);
                    if(data[i].m_Level == 5)
                    {
                        QuestManager.UpdateQuestsStatic(QuestAction.UpgradeItemToMax);
                    }
                }
                break;
            }
        }
    }
}


public class ItemManager {

    public ItemManagerObject m_Storer;

	public static ItemManager current;
	public int m_ItemFloorGap = 100;

    //Stores bought items
	public ItemType[] m_AvailableItems = {ItemType.AutoPilot};

	public ItemType m_NextItemType;
	public int m_NextItemFloorCount = 100;

    public ItemSaveData m_Save;

    Dictionary<ItemType, GameObject> m_PrefabDict;
    Dictionary<ItemType, int> m_ItemLevelDict;

    int m_CurrentItemDataIndex = 0;
    int m_LastItemDataIndex;
    ItemSpawnData m_CurrentItemData;

    const string savePath = "/itemSave.save";

	// Use this for initialization
	public ItemManager () {
		current = this;
        
        m_Storer = (ItemManagerObject) Resources.Load("ScriptableObjects/ItemManagerData");
        Debug.Assert(m_Storer != null, "Item Manager data should not be null");
        m_LastItemDataIndex = m_Storer.m_SpawnData.Length - 1;
        m_CurrentItemData = m_Storer.m_SpawnData[0];
        m_NextItemFloorCount = m_CurrentItemData.ItemFloorGap;
        
        m_PrefabDict = new Dictionary<ItemType, GameObject>();
        foreach(ItemPrefabData go in m_Storer.m_ItemPrefabs)
        {
            m_PrefabDict[go.m_Type] = go.m_Prefab;
        }
        LoadOrDefault();
        InitItemLevelDict();
    }

    public void Reset()
    {
        m_CurrentItemDataIndex = 0;
        m_CurrentItemData = m_Storer.m_SpawnData[0];
        m_NextItemFloorCount = m_CurrentItemData.ItemFloorGap;
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + savePath, FileMode.OpenOrCreate);
        bf.Serialize(file, m_Save);
        file.Close();
    }

    public void LoadOrDefault()
    {
        if (File.Exists(Application.persistentDataPath + savePath))
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + savePath, FileMode.Open);
                m_Save = (ItemSaveData)bf.Deserialize(file);
                file.Close();
            }
            catch (SerializationException e)
            {
                Debug.Log(e);
                InitSaveData();
            }
        }
        else
        {
            InitSaveData();
        }
    }

    public void InitSaveData()
    {
        m_Save = new ItemSaveData();
        m_Save.UseDefaultData(m_Storer);
    }

    // Returns if we should build an item
    public bool NextFloorReached()
	{
		m_NextItemFloorCount--;
		return m_NextItemFloorCount <= 0;
	}
    
    public void PutItemOnFloor(FloorMesh fm)
    {
        GameObject nextItemPrefab = NextItemPrefab();
        if (nextItemPrefab != null)
        {
            GameObject item = GameObject.Instantiate(nextItemPrefab);
            item.transform.position = (fm.prevPos1 + fm.prevPos2) / 2.0f;
            RefreshFloorCount();
            int itemRange = 2;
            int maxSlot = fm.GetMaxSlot();

            int rangeMin = maxSlot / 2 - itemRange;
            int rangeMax = maxSlot / 2 + itemRange;
            if (rangeMin < 0) rangeMin = 0;
            if (rangeMax >= maxSlot) rangeMax = maxSlot;

            fm.PutItemOnSlot(item.GetComponent<ItemSuper>(), UnityEngine.Random.Range(rangeMin, rangeMax));
        }
    }


	public GameObject NextItemPrefab()
	{
        if(m_CurrentItemDataIndex < m_LastItemDataIndex && GameManager.current.scoreForDifficulty >= m_Storer.m_SpawnData[m_CurrentItemDataIndex+1].ActivateScore)
        {
            m_CurrentItemDataIndex++;
            m_CurrentItemData = m_Storer.m_SpawnData[m_CurrentItemDataIndex];
        }

        ItemType[] items = m_CurrentItemData.Items;
        if (items.Length > 0)
        {
            //Find first bought item
            int first = 0;
            bool hasItem = false;
            for (; first < items.Length; ++first)
            {
                if (GetItemLevel(items[first]) > 0)
                {
                    hasItem = true;
                    break;
                }
            }
            if (!hasItem) return null; // no bought item
            ItemType type = items[first];

            // Reservoir sampling
            float count = 1.0f;
            for (int i = first + 1; i < items.Length; ++i)
            {
                if (GetItemLevel(items[i]) <= 0)
                    continue;
                float p = count + 1.0f;
                count += 1.0f;
                if (UnityEngine.Random.value <= 1.0f/p)
                {
                    type = items[i];
                    break;
                }
            }

            return m_PrefabDict[type];
        }
        return null;
	}

    public GameObject GetGOByType(ItemType type)
    {
        return m_PrefabDict[type];
    }

	public void RefreshFloorCount()
	{
        m_NextItemFloorCount = m_CurrentItemData.ItemFloorGap;
    }
		
    public static float GetItemDuration(ItemType type)
    {
        ItemData m_Data = current.m_Storer.m_ItemData;
        int level = current.GetItemLevel(type) - 1;
        float res = 1.0f;
        switch (type)
        {
            case ItemType.Shield:
                return m_Data.m_ShieldTimePerLevel[level];
            case ItemType.AutoPilot:
                return m_Data.m_AutoPilotTimePerLevel[level];
            case ItemType.Magnet:
                return m_Data.m_MagnetTimePerLevel[level];
            case ItemType.TimeSlow:
                return m_Data.m_TimeSlowTimePerLevel[level];
            case ItemType.DoubleScore:
                return m_Data.m_DoubleScoreTimePerLevel[level];
        }
        return res;
    }

    void InitItemLevelDict()
    {
        m_ItemLevelDict = new Dictionary<ItemType, int>();
        foreach(ItemSingleSaveData data in m_Save.data)
        {
            m_ItemLevelDict[data.m_Type] = data.m_Level;
        }
    }

    public int GetItemLevel(ItemType type)
    {
        return m_ItemLevelDict != null ? m_ItemLevelDict[type]:0;
    }

    public int GetItemPrice(ItemType type)
    {
        int level = GetItemLevel(type);
        return m_Storer.GetPriceByTypeAndLevel(type, level);
    }
    
    public bool UpgradeItem(ItemType type)
    {
        int price = GetItemPrice(type);
        if(GameManager.current.gameCoins >= price)
        {
            GameManager.current.AddCoin(-price);

            int level = GetItemLevel(type);
            if (level == 5) return false;

            level++;
            m_ItemLevelDict[type] = level;
            m_Save.UpgradeItem(type);
            Save();
            return true;
        }
        else
        {
            return false;
        }

    }
}
