using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloorColorMatColor
{
    Color start;
}

public class FloorColorData : MonoBehaviour {

    public Color[] floorColorDataArray;
    public int currentLevel = -1;
    public static FloorColorData current;

    private void Awake()
    {
        current = this;
        currentLevel = 0;
    }

    void Update()
    {
        if (ChallengeManager.current != null && ChallengeManager.current.currentDifficulty % floorColorDataArray.Length != currentLevel)
        {
            currentLevel = ChallengeManager.current.currentDifficulty % floorColorDataArray.Length;
            //DrawFloors();
        }
    }
}
