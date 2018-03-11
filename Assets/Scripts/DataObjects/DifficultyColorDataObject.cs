using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TopDownColor
{
    public Color topColor;
    public Color downColor;
    public Color floorColor;
}

[System.Serializable]
public class ColorsForDiff
{
    public TopDownColor[] colors;
}

[CreateAssetMenu(fileName = "DifficultyColorData", menuName = "Custom/DiffColor", order = 1)]
public class DifficultyColorDataObject : ScriptableObject {
    public ColorsForDiff[] data;
    public float lerpTime;
    public float sharpLerpTime;
    public float floorBeforeSec;
    public int startWithX = 0;
}
