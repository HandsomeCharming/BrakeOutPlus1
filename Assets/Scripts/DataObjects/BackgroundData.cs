using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundData", menuName = "Custom/Background", order = 1)]
public class BackgroundData : ScriptableObject {
    public Material floorColorMat;
    public Material floorSkyMat;

    public List<GameObject> m_SkyPrefabs;
    public MinMaxData m_SkyNewObjectTime;
    public MinMaxData m_SkyNewObjectDistance;
    public MinMaxData m_SkyNewObjectHeight;
    public MinMaxData m_SkyNewObjectScale;
}
