using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRemovePlayerPref : MonoBehaviour {

    public string ClearPlayerKey = "";

    public void ClearPlayerPrefUsingKey()
    {
        PlayerPrefs.DeleteKey(ClearPlayerKey);
    }

    public void ClearAllPlayerPref()
    {
        PlayerPrefs.DeleteAll();
    }
}
