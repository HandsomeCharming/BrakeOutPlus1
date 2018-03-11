using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorColorController : MonoBehaviour {
    
    public FloorColorDataObject m_Storer;
    public static FloorColorController current;

    public FloorColorObjectData m_CurrentData;
    public int m_CurrentColorIndex;
    public int m_CurrentDataIndex;
    public int m_FromDataIndex;
    public int m_ToDataIndex;
    
    int lerpGap;

    private void Awake()
    {
        current = this;
        //m_Storer = (FloorColorDataObject) Resources.Load("DataObjects/FloorColorDataObject");
        m_CurrentColorIndex = 0;
        m_CurrentDataIndex = 0;
        print(m_Storer.data.Length);
        m_CurrentData = m_Storer.data[0];
        lerpGap = m_Storer.m_LerpGap;
        m_FromDataIndex = Random.Range(0, m_CurrentData.colors.Length);
        m_ToDataIndex = (m_FromDataIndex + 1) % m_CurrentData.colors.Length;
    }

    public void NextColorPalette()
    {
        if(m_CurrentDataIndex < m_Storer.data.Length - 1)
            m_CurrentData = m_Storer.data[++m_CurrentDataIndex];

        m_CurrentColorIndex = 0;
        m_FromDataIndex = Random.Range(0, m_CurrentData.colors.Length);
        m_ToDataIndex = (m_FromDataIndex + 1) % m_CurrentData.colors.Length;
    }

    public Color NextColor()
    {
        Color col = new Color();

        col = Color.Lerp(m_CurrentData.colors[m_FromDataIndex], m_CurrentData.colors[m_ToDataIndex], 
            (float)m_CurrentColorIndex / (float)lerpGap);
        ++m_CurrentColorIndex;
        if(m_CurrentColorIndex > lerpGap)
        {
            m_CurrentColorIndex = 0;
            m_FromDataIndex = (m_FromDataIndex + 1) % m_CurrentData.colors.Length;
            m_ToDataIndex = (m_ToDataIndex  + 1) % m_CurrentData.colors.Length;
        }

        return col;
    }
}
