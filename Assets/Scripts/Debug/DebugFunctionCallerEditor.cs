#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DebugFunctionCaller))]
public class DebugFunctionCallerEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DebugFunctionCaller myScript = (DebugFunctionCaller)target;
        if (GUILayout.Button("Call Function"))
        {
            myScript.CallFunc();
        }
    }
}
#endif