using UnityEngine;
using UnityEditor;

public class VCloudShaderGUI : MaterialEditor
{
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Help"))
            Application.OpenURL("https://docs.google.com/document/d/1vUXzQ5Ww9NR7m7F5n7adUWXJnWUH0u47JHmcifKK48w/edit?usp=sharing");

        base.OnInspectorGUI();
        
        serializedObject.Update();
        
        var theShader = serializedObject.FindProperty("m_Shader");

        //string debug = "Drew base inspector, ";
        //int d = 0;
        if (isVisible && theShader.objectReferenceValue != null)
        {
            //debug += "inspector visible, "; d++;

            if (!theShader.hasMultipleDifferentValues)
            {
                //debug += "multi edit disabled, "; d++;
                if (theShader.objectReferenceValue != null)
                {
                    //debug += "reference non-null, "; d++;
                    if (target is Material)
                    {
                        //debug += "target is material, "; d++;
                        Material m = (Material)target;
                        m.renderQueue = m.GetInt("_RenderQueue");
                        //debug += "render queue should be set to " + m.GetInt("_RenderQueue") + " and is now equal to " + m.renderQueue+"."; d++;
                    }
                }                
            }
        }
        /*
        debug += "\nDone debugging, "+d+"/5 correct";
        if (d == 5)
            Debug.Log(debug);
        else
            Debug.LogError(debug);*/
    }
}
