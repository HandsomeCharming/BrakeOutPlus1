using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdPlacementRecorder {
    int phase = 0;
    int remain;

    static AdPlacementRecorder Instance
    {
        get
        {
            if (instance == null)
                instance = new AdPlacementRecorder();
            return instance;
        }
        set { }
    }
    static AdPlacementRecorder instance;

    readonly int[] phases = { 5, 4, 4, 3, 3, 2, 2 };

    const string PhaseKey = "AdPhase";
    const string RemainKey = "GameRemain";


    AdPlacementRecorder()
    {
        phase = PlayerPrefs.GetInt(PhaseKey, 0);
        if (PlayerPrefs.HasKey(RemainKey))
            remain = PlayerPrefs.GetInt(RemainKey);
        else
            remain = phases[0];
    }

    public static void GamePlayed()
    {
        Instance.GamePlayedPriv();
    }
    
    public static bool ShouldPlayAd()
    {
        return Instance.remain <= 0;
    }

    public static void AdPlayed()
    {
        Instance.ADPlayedPriv();
    }

    void GamePlayedPriv()
    {
        remain--;
        Save();
    }
    void ADPlayedPriv()
    {
        if (remain < 0)
        {
            phase++;
            if (phase < phases.Length)
                remain = phases[phase];
            else
                remain = Random.value < 0.5f ? 1 : 2;
        }
        Save();
    }

    void Save()
    {
        PlayerPrefs.SetInt(RemainKey, remain);
        PlayerPrefs.SetInt(PhaseKey, phase);
        PlayerPrefs.Save();
    }

}
