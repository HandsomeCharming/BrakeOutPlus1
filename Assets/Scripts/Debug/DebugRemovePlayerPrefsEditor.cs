﻿#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DebugRemovePlayerPref))]
public class DebugRemovePlayerPrefsEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DebugRemovePlayerPref myScript = (DebugRemovePlayerPref)target;
        if (GUILayout.Button("Remove PlayerPref by Key"))
        {
            myScript.ClearPlayerPrefUsingKey();
        }
        if (GUILayout.Button("Remove All PlayerPref"))
        {
            myScript.ClearAllPlayerPref();
        }
    }
}
#endif