using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CarClass
{
    Basic,
    Fast,
    GoodTurn,
    Boost,
    Best,
    Custom
}

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

public enum Currency
{
    Star,
    Coin
}


[System.Serializable]
public class SceneCars
{
    public string name;
    public List<SingleCarSelectData> carData;
    public List<TrailSelectData> trailData;
}

[System.Serializable]
public class MinMaxData
{
    public float min;
    public float max;
}

[System.Serializable]
public class MinMaxDataInt
{
    public int min;
    public int max;

    public MinMaxDataInt(int min, int max)
    {
        this.min = min;
        this.max = max;
    }

    public int GetRandomBetweenRange()
    {
        return Random.Range(min, max+1);
    }

    public int GetBetweenRange(float lerpAmount)
    {
        return Mathf.RoundToInt(Mathf.Lerp((float)min, (float)max, lerpAmount));
    }
    
    public int GetBetweenRangeWithGap(float lerpAmount, int gap)
    {
        int gappedAmount = (max - min) / gap;
        int addedAmount = Mathf.RoundToInt( Mathf.Lerp((float)0, (float)gappedAmount, lerpAmount) );
        addedAmount *= gap;
        return min + addedAmount;
    }
}

[System.Serializable]
public class TrailSelectData
{
    public string name;
    public int price;

    [Header("Prefabs")]
    public GameObject TrailDisplayPrefab;
}

[System.Serializable]
public class CarShortData
{
    public string name;
    public int carIndex;
    public int sceneIndex;

    public CarShortData() { }

    public CarShortData(int carIndex, int sceneIndex)
    {
        this.carIndex = carIndex;
        this.sceneIndex = sceneIndex;
    }
}

[System.Serializable]
public class TrailOnCarData
{
    public string name;

    [Header("Prefabs")]
    public GameObject TrailPrefab;
}

[System.Serializable]
public class SingleCarSelectData
{
    public string name;
    public int coinPrice; // coin price
    public int starPrice;
    //public Currency priceCurrency;
    public int returnedAmountWhenLooted;
    public Currency returnWhenLootedCurrency;

    [Header("Prefabs")]
    public GameObject CarInGamePrefab;
    public GameObject CarForViewPrefab;

    public bool customViewPos;
    public Vector3 ViewPos;
    public bool customViewRot;
    public Vector3 ViewRot;

    public bool lootBoxCustomViewPos;
    public Vector3 LootBoxViewPos;
    public bool lootBoxCustomViewRot;
    public Vector3 lootBoxViewRot;

    [Header("Class")]
    public CarClass carClass;
    public Rarity rarity;
    public int maxUpgradeLevel;

    [Header("Price Settings")]
    public int[] m_AccelerationPrice;
    public int[] m_HandlingPrice;
    public int[] m_BoostPrice;

    [Header("Feature Descriptions")]
    public string firstLine = "a";
    public string secondLine = "b";

    [Header("Trail")]
    public bool CanChangeTrail = true;
    public List<TrailOnCarData> m_Trails;

    public int GetUpgradePrice(int currentLevel, CarUpgradeCatagory type)
    {
        switch (type)
        {
            case CarUpgradeCatagory.Accelerate:
                if(currentLevel < m_AccelerationPrice.Length)
                    return m_AccelerationPrice[currentLevel];
                break;
            case CarUpgradeCatagory.Boost:
                if (currentLevel < m_BoostPrice.Length)
                    return m_BoostPrice[currentLevel];
                break;
            case CarUpgradeCatagory.Handling:
                if (currentLevel < m_HandlingPrice.Length)
                    return m_HandlingPrice[currentLevel];
                break;
        }
        return 0;
    }
}

[System.Serializable]
public class CarClassData
{
    public string name;
    public CarClass carClass;

    [Header("Display Settings")]
    [Range(0, 1.0f)]
    public float m_MinAcceleration;
    [Range(0, 1.0f)]
    public float m_MaxAcceleration;
    [Range(0, 1.0f)]
    public float m_MinHandling;
    [Range(0, 1.0f)]
    public float m_MaxHandling;
    [Range(0, 1.0f)]
    public float m_MinBoost;
    [Range(0, 1.0f)]
    public float m_MaxBoost;

    [Header("Physics Settings")]
    public MinMaxData m_PushForce;
    public MinMaxData m_BoostForce;
    public MinMaxData m_LaunchForce;
    public float m_Gravity;
    public MinMaxData m_MinRotateSpeed;
    public MinMaxData m_MaxRotateSpeed;
    public MinMaxData m_MinBoostRotateSpeed;
    public MinMaxData m_MaxBoostRotateSpeed;
    public MinMaxData timeToReachMaxRotateSpeed;
    public MinMaxData timeToReachMaxBoost;
    public MinMaxData m_CameraGoFarTime;
    public MinMaxData m_CameraGoNearTime;

    public MinMaxData m_MaxBoostMultiplier;
    public MinMaxData m_BoostIncreaseRate;
}

[System.Serializable]
public class CarSaveData
{
    public string m_Name;
    public int m_CarIndex;
    public int m_SceneIndex;
    public int m_AccLevel = 0;
    public int m_HandlingLevel = 0;
    public int m_BoostLevel = 0;
}

[CreateAssetMenu(fileName = "CarSelectData", menuName = "Custom/CarSelect", order = 1)]
public class CarSelectData : ScriptableObject
{
    public List<SceneCars> sceneData;
    public List<CarClassData> classData;

    public SingleCarSelectData GetCarData(int carIndex, int sceneIndex)
    {
        return sceneData[sceneIndex].carData[carIndex];
    }
}

public class CarSelectDataReader
{
    public static CarSelectDataReader Instance
    {
        get
        {
            if(m_Instance == null)
            {
                m_Instance = new CarSelectDataReader();
            }
            return m_Instance;
        }
        set
        {

        }
    }
    static CarSelectDataReader m_Instance;
    public CarSelectData m_CarStorer;
    public Dictionary<string, SingleCarSelectData> m_CarDict;
    public Dictionary<string, TrailSelectData> m_TrailSelectDict;

    CarSelectDataReader()
    {
        m_CarStorer = (CarSelectData)Resources.Load(m_DataPath);
        m_CarDict = new Dictionary<string, SingleCarSelectData>();
        m_TrailSelectDict = new Dictionary<string, TrailSelectData>();

        foreach (var cars in m_CarStorer.sceneData)
        {
            foreach(var car in cars.carData)
            {
                m_CarDict.Add(car.name, car);
            }
            foreach(var trail in cars.trailData)
            {
                m_TrailSelectDict.Add(trail.name, trail);
            }
        }
    }

    const string m_DataPath = "ScriptableObjects/CarSelectData";

    public SingleCarSelectData GetCarData(int carIndex, int sceneIndex)
    {
        return m_CarStorer.sceneData[sceneIndex].carData[carIndex];
    }

    public SingleCarSelectData GetCarData(string name)
    {
        return m_CarDict[name];
    }

    public TrailSelectData GetTrailSelectData(string name)
    {
        return m_TrailSelectDict[name];
    }

    public CarClassData GetCarClassData(string name)
    {
        foreach (var data in m_CarStorer.classData)
        {
            if(data.carClass.ToString() == name)
            {
                return data;
            }
        }
        return null;
    }

    public CarClassData GetCarClassData(CarClass c)
    {
        foreach (var data in m_CarStorer.classData)
        {
            if (data.carClass == c)
            {
                return data;
            }
        }
        return null;
    }
}
