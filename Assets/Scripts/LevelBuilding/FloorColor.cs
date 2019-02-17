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
