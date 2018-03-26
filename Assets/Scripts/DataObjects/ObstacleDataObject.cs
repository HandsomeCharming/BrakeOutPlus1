using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleDataObject", menuName = "Custom/ObstacleData", order = 1)]
public class ObstacleDataObject : ScriptableObject {

    public float m_BlackHoleDistance;
    public float m_MovingCubeMoveDistance;

    public float m_JumpDistance;
    public float m_JumpHeight;

    public float m_GlideDistance;
    public float m_GlideHeight;
    public float m_GlideCoinGap;

    public GameObject BoostSignPrefab;
    public GameObject[] m_GliderCoins;

    public GameObject m_RimPrefab;

    public GameObject m_BoostSignPrefab;
    public GameObject m_GlideSignPrefab;
    public GameObject m_StopSignPrefab;
}
