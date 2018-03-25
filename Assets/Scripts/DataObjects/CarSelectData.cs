﻿using System.Collections;
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
}

[System.Serializable]
public class MinMaxData
{
    public float min;
    public float max;
}

[System.Serializable]
public class SingleCarSelectData
{
    public string name;

    [Header("Prefabs")]
    public GameObject CarInGamePrefab;
    public GameObject CarForViewPrefab;

    [Header("Class")]
    public CarClass carClass;
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

public class CarData
{
    public string m_Name;
    public int m_AccLevel;
    public int m_HandlingLevel;
    public int m_BoostLevel;
}

[CreateAssetMenu(fileName = "CarSelectData", menuName = "Custom/CarSelect", order = 1)]
public class CarSelectData : ScriptableObject
{
    public List<SceneCars> sceneData;
    public List<CarClassData> classData;
}
