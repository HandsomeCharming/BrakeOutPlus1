using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ObstaclePrefabs
{
    public string obstacleName;

    public GameObject Prefab; //future: change to array
}

[System.Serializable]
public class SceneObstaclePrefabs
{
    public string SceneName;

    public ObstaclePrefabs[] m_Prefabs;

    /*
    public GameObject CubePrefab; //future: change to array
    public GameObject WallPrefab; //future: change to array
    public GameObject BlackholePrefab;
    public GameObject MovingCubePrefab;
    public GameObject MovingWallPrefab;
    public GameObject WallLeftCubePrefab; //future: change to array
    public GameObject WallRightCubePrefab; //future: change to array
    public GameObject WallMidCubePrefab; //future: change to array
    public GameObject GlidingTriggerPrefab;
    public GameObject AutoPilotTriggerPrefab;

    public GameObject m_BoostSignPrefab;
    public GameObject m_GlideSignPrefab;
    public GameObject m_StopSignPrefab;
    */
}

[CreateAssetMenu(fileName = "ObstacleDataObject", menuName = "Custom/ObstacleData", order = 1)]
public class ObstacleDataObject : ScriptableObject {

    [Header("Obstacles")]
    public float m_BlackHoleDistance;
    public float m_MovingCubeMoveDistance;

    [Header("Jump")]
    public float m_JumpDistance;
    public float m_JumpHeight;

    [Header("Glide")]
    public float m_GlideDistance;
    public float m_GlideHeight;
    public float m_GlideCoinGap;
    public MinMaxData m_GlideCoinFreq;
    public MinMaxData m_GlideCoinOffset;

    [Header("Prefabs")]
    public GameObject BoostGroundSignPrefab;
    public GameObject[] m_GliderCoins;

    public GameObject m_RimPrefab;

    public GameObject m_BoostSignPrefab;
    public GameObject m_GlideSignPrefab;
    public GameObject m_StopSignPrefab;

    public SceneObstaclePrefabs[] m_ScenePrefabs;
}

public class ObstacleDataReader
{
    public static ObstacleDataReader Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new ObstacleDataReader();
            }
            return m_Instance;
        }
        set
        {

        }
    }
    static ObstacleDataReader m_Instance;
    public ObstacleDataObject m_Storer;
    public Dictionary<string, ObstaclePrefabs>[] m_PrefabDict;

    const string m_DataPath = "ScriptableObjects/ObstacleDataObject";

    ObstacleDataReader()
    {
        m_Storer = (ObstacleDataObject)Resources.Load(m_DataPath);
        m_PrefabDict = new Dictionary<string, ObstaclePrefabs>[m_Storer.m_ScenePrefabs.Length];

        for(int i=0; i<m_Storer.m_ScenePrefabs.Length; ++i)
        {
            m_PrefabDict[i] = new Dictionary<string, ObstaclePrefabs>();
            var scene = m_Storer.m_ScenePrefabs[i];
            foreach (var prefab in scene.m_Prefabs)
            {
                m_PrefabDict[i].Add(prefab.obstacleName, prefab);
            }
        }
    }


    public GameObject GetObstaclePrefabPrivate(string name)
    {
        int sceneIndex = GameManager.current.m_CurrentSceneIndex;
        if (m_PrefabDict[sceneIndex].ContainsKey(name))
            return m_PrefabDict[sceneIndex][name].Prefab;
        return m_PrefabDict[0][name].Prefab;
    }

    public GameObject GetObstaclePrefabPrivate(string name, int sceneIndex)
    {
        if (m_PrefabDict[sceneIndex].ContainsKey(name))
            return m_PrefabDict[sceneIndex][name].Prefab;
        return m_PrefabDict[0][name].Prefab;
    }
    
    public static GameObject GetObstaclePrefab(string name, int sceneIndex)
    {
        return Instance.GetObstaclePrefabPrivate(name, sceneIndex);
    }

    public static GameObject GetObstaclePrefab(string name)
    {
        return Instance.GetObstaclePrefabPrivate(name);
    }
}
