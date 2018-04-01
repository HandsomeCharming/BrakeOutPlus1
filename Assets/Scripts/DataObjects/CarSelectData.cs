using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CarClass
{
    Basic,
    Fast,
    GoodTurn,
    Boost,
    Slow
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
public class TrailSelectData
{
    public string name;
    public int price;

    [Header("Prefabs")]
    public GameObject TrailDisplayPrefab;
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
    public int price;

    [Header("Prefabs")]
    public GameObject CarInGamePrefab;
    public GameObject CarForViewPrefab;

    public bool customViewPos;
    public Vector3 ViewPos;
    public bool customViewRot;
    public Vector3 ViewRot;

    [Header("Class")]
    public CarClass carClass;

    [Header("Trail")]
    public bool CanChangeTrail = true;
    public List<TrailOnCarData> m_Trails;
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

    [Header("Price Settings")]
    public float[] m_AccelerationPrice;
    public float[] m_HandlingPrice;
    public float[] m_BoostPrice;

    [Header("Physics Settings")]
    public MinMaxData m_BoostForce;
    public MinMaxData m_MinRotateSpeed;
    public MinMaxData m_MaxRotateSpeed;
    public MinMaxData m_MinBoostRotateSpeed;
    public MinMaxData m_MaxBoostRotateSpeed;
    public float timeToReachMaxRotateSpeed;
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
}
