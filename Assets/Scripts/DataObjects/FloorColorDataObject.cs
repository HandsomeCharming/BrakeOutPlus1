using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloorColorObjectData
{
    public Color[] colors;
}

[CreateAssetMenu(fileName = "FloorColorData", menuName = "Custom/FloorColorData", order = 1)]
public class FloorColorDataObject : ScriptableObject {
    public FloorColorObjectData[] data;
    public int m_LerpGap;
}
