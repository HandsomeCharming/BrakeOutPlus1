using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FloorMesh), typeof(MeshRenderer))]
public class FloorColor : MonoBehaviour {
    
    public FloorMesh mesh;
    public MeshRenderer m_Renderer;

	void Awake () {
        mesh = GetComponent<FloorMesh>();
        m_Renderer = GetComponent<MeshRenderer>();
    }

    public void ResetColor()
    {
        DrawColor();
    }

    public void DrawColor(Color col)
    {
        m_Renderer.sharedMaterial.color = col;
        m_Renderer.sharedMaterial.SetColor("_EmissionColor", col);
    }

    // Default, get color from background
    public void DrawColor()
    {
        /*if (FloorColorData.current && FloorBuilder.current)
        {
            int next = (m_Level+1) % FloorColorData.current.floorColorDataArray.Length;
            float p = (float)m_Index / (float)FloorBuilder.current.floorMeshCount;
            m_Renderer.material.color = Color.Lerp(FloorColorData.current.floorColorDataArray[m_Level], FloorColorData.current.floorColorDataArray[next], p);
            m_Level = next;
        }*/

        /*if (FloorColorController.current)
        {
            m_Renderer.material.color = FloorColorController.current.NextColor();
        }*/
        if(BackgroundManager.GetBackgroundState() == BackgroundEnum.Color)
        {
            if (BackgroundMaterial.current && BackgroundMaterial.current.gameObject.activeSelf)
            {
                Color col = BackgroundMaterial.current.GetCurrentFloorColor();
                m_Renderer.sharedMaterial.color = col;
                m_Renderer.sharedMaterial.SetColor("_EmissionColor", col / 3.0f);
            }
        }
    }
}
