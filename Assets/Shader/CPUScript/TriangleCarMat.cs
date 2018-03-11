using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleCarMat : MonoBehaviour {

    public Material mat;

    public int lerpTime;

    [HideInInspector]
    float time;
    bool m_Increasing;

    private void Awake()
    {
        time = 0;
        m_Increasing = true;
        mat = GetComponent<MeshRenderer>().material;
        mat.SetFloat("_ScreenSizeX", Screen.width);
        mat.SetFloat("_ScreenSizeY", Screen.height);
    }

    void Update()
    {
        if (m_Increasing)
        {
            time += Time.deltaTime;
            if (time >= lerpTime)
            {
                m_Increasing = false;
            }
        }
        else
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                m_Increasing = true;
            }
        }
        mat.SetFloat("_ColorLerp", time / lerpTime);
    }
}
