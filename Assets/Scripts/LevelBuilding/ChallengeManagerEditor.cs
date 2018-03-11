#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChallengeManager))]

public class ChallengeManagerEditor : Editor {
    
 /*   public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Write Floor data To Xml")) {
            ((ChallengeManager)serializedObject.targetObject).WriteToXML();
        }
        if (GUILayout.Button("Read Floor data from Xml"))
        {
            ((ChallengeManager)serializedObject.targetObject).ReadFromXML();
        }
        DrawDefaultInspector();
        //This draws the default screen.  You don't need this if you want
}
*/
}
#endif