#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(QuestSlotUI))]
public class QuestSlotUIEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        QuestSlotUI myScript = (QuestSlotUI)target;
        if (GUILayout.Button("FindChilds"))
        {
            myScript.FindChildsEditor();
        }
    }
}
#endif