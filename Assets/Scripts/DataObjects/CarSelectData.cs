using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public MinMaxData m_RotateSpeed;
    public MinMaxData m_BoostRotateSpeed;
    public float timeToReachMaxRotateSpeed;
}

[CreateAssetMenu(fileName = "CarSelectData", menuName = "Custom/CarSelect", order = 1)]
public class CarSelectData : ScriptableObject
{
    public List<SingleCarSelectData> data;
}
