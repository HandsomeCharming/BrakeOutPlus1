using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMaterial : MonoBehaviour {

    public static BackgroundMaterial current;

    public GameObject GlideBackground;
    public GameObject AutoPilotBackground;

    [HideInInspector]
    public Material mat;
    public DifficultyColorDataObject m_Storer;

    public float lerpTime;
    //public float floorBeforeTime;
    public int m_CurrentColorIndex = 0;

    [HideInInspector]
    float time;
    float floorTime;
    bool m_Increasing;
    float m_SharpLerpTime;
    ColorsForDiff m_CurrentColor;
    int ColorAIndex;
    int ColorBIndex;
    Color m_CurrentTop;
    Color m_CurrentDown;
    Color m_CurrentFloor;

    TopDownColor m_SharpLerpTarget;
    TopDownColor blackColor;
    bool m_SharpLerpToBlack = false;
    bool m_Gliding = false;
    bool m_Autopilot = false;
    //int FloorColAIndex;
    //int FloorColBIndex;

    private void Awake()
    {
        current = this;

        time = 0;
        m_Increasing = true;
        m_SharpLerpTime = 0;
        mat = GetComponent<Image>().material;
        mat.SetFloat("_ScreenSizeX", Screen.width);
        mat.SetFloat("_ScreenSizeY", Screen.height);

        blackColor = new TopDownColor();
        blackColor.downColor = Color.black;
        blackColor.topColor = Color.black;
        blackColor.floorColor = Color.black;

        lerpTime = m_Storer.lerpTime;
        //floorBeforeTime = m_Storer.floorBeforeSec;
        m_CurrentColor = m_Storer.data[0];
        if(m_CurrentColor.colors.Length >= 2)
        {
            if (m_Storer.startWithX > 0 && m_Storer.startWithX <= m_CurrentColor.colors.Length)
            {
                ColorAIndex = m_Storer.startWithX - 1;
                ColorBIndex = (ColorAIndex + 1) % m_CurrentColor.colors.Length;
                SetColorA(m_CurrentColor.colors[ColorAIndex]);
                SetColorB(m_CurrentColor.colors[ColorBIndex]);
            }
            else
            {
                ColorAIndex = Random.Range(0, m_CurrentColor.colors.Length);
                ColorBIndex = (ColorAIndex + 1) % m_CurrentColor.colors.Length;
                SetColorA(m_CurrentColor.colors[ColorAIndex]);
                SetColorB(m_CurrentColor.colors[ColorBIndex]);
            }
        }
        else
        {
            SetColorA(m_CurrentColor.colors[0]);
            SetColorB(m_CurrentColor.colors[0]);
            ColorAIndex = 0;
            ColorBIndex = 0;
            print("Shit, All color need to have 2 or more");
        }
        /*FloorColAIndex = ColorAIndex;
        FloorColBIndex = ColorBIndex;
        floorTime = floorBeforeTime;*/
    }

    public void StartGlideIfColor()
    {
        if(BackgroundManager.current.m_Background == BackgroundEnum.Color)
        {
            GlideBackground.SetActive(true);
            m_Gliding = true;
            StartCoroutine(GlideManage());
        }
    }
    
    public void EndGlide()
    {
        m_Gliding = false;
    }

    IEnumerator GlideManage()
    {
        const float fadeInTime = 0.5f;
        const float maxAlpha = 0.5f;
        float time = 0;
        Image image = GlideBackground.GetComponent<Image>();
        while (m_Gliding)
        {
            if(time < fadeInTime)
            {
                Color col = image.color;
                col.a = Mathf.Lerp(0, maxAlpha, time / fadeInTime);
                image.color = col;
            }
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

        time = 0;
        while (time < fadeInTime)
        {
            Color col = image.color;
            col.a = Mathf.Lerp(maxAlpha, 0, time / fadeInTime);
            image.color = col;
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        
        GlideBackground.SetActive(false);
    }


    public void StartAutoPilotIfColor()
    {
        if (BackgroundManager.current.m_Background == BackgroundEnum.Color)
        {
            AutoPilotBackground.SetActive(true);
            m_Autopilot = true;
            StartCoroutine(AutoPilotManage());
        }
    }

    public void EndAutoPilot()
    {
        m_Autopilot = false;
    }

    IEnumerator AutoPilotManage()
    {
        const float fadeInTime = 0.5f;
        const float maxAlpha = 0.5f;
        float time = 0;
        Image image = AutoPilotBackground.GetComponent<Image>();
        while (m_Autopilot)
        {
            if (time < fadeInTime)
            {
                Color col = image.color;
                col.a = Mathf.Lerp(0, maxAlpha, time / fadeInTime);
                image.color = col;
            }
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

        time = 0;
        while (time < fadeInTime)
        {
            Color col = image.color;
            col.a = Mathf.Lerp(maxAlpha, 0, time / fadeInTime);
            image.color = col;
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

        AutoPilotBackground.SetActive(false);
    }


    void SetColorA(TopDownColor colors)
    {
        mat.SetColor("_ColorUpA", colors.topColor);
        mat.SetColor("_ColorDownA", colors.downColor);
    }

    void SetColorB(TopDownColor colors)
    {
        mat.SetColor("_ColorUpB", colors.topColor);
        mat.SetColor("_ColorDownB", colors.downColor);
    }

    public void ChangeColorSudden()
    {
        if(m_CurrentColorIndex < m_Storer.data.Length - 1)
        {
            m_CurrentColorIndex++;
            m_CurrentColor = m_Storer.data[m_CurrentColorIndex];
            ColorAIndex = 0;
            ColorBIndex = 1;

            SetColorA(m_CurrentColor.colors[ColorAIndex]);
            SetColorB(m_CurrentColor.colors[ColorBIndex]);
            time = 0;
            mat.SetFloat("_ColorLerp", time / lerpTime);
        }
        else
        {
            ColorAIndex = (ColorAIndex + 2) % m_CurrentColor.colors.Length;
            SetColorA(m_CurrentColor.colors[ColorAIndex]);
            ColorBIndex = (ColorBIndex + 2) % m_CurrentColor.colors.Length;
            SetColorB(m_CurrentColor.colors[ColorBIndex]);
        }

        /*FloorColAIndex = ColorAIndex;
        FloorColBIndex = ColorBIndex;
        floorTime = floorBeforeTime;*/
    }

    public Color GetCurrentDownColor()
    {
        float lerpAmount = time / lerpTime;
        return Color.Lerp(m_CurrentColor.colors[ColorAIndex].downColor, m_CurrentColor.colors[ColorBIndex].downColor, lerpAmount);
    }

    public Color GetCurrentFloorColor()
    {
        if (m_SharpLerpTime > 0 || m_SharpLerpToBlack)
           return GetSharpLerpFloorColor();
        else
            return Color.Lerp(m_CurrentColor.colors[ColorAIndex].floorColor, m_CurrentColor.colors[ColorBIndex].floorColor,
             time / lerpTime);
    }

    Color GetSharpLerpFloorColor()
    {
        float lerpAmount = 1 - m_SharpLerpTime / m_Storer.sharpLerpTime;
        Color col = Color.Lerp(m_CurrentFloor, m_SharpLerpTarget.floorColor, lerpAmount);
        return col;
    }

    public void StartSharpLerp()
    {
        m_CurrentFloor = GetCurrentFloorColor();
        m_SharpLerpTime = m_Storer.sharpLerpTime;
        float lerpAmount = time / lerpTime;
        m_CurrentTop = Color.Lerp(m_CurrentColor.colors[ColorAIndex].topColor, m_CurrentColor.colors[ColorBIndex].topColor, lerpAmount);
        m_CurrentDown = Color.Lerp(m_CurrentColor.colors[ColorAIndex].downColor, m_CurrentColor.colors[ColorBIndex].downColor, lerpAmount);

        mat.SetColor("_ColorUpA", m_CurrentTop);
        mat.SetColor("_ColorDownA", m_CurrentDown);

        ColorAIndex = (ColorAIndex + 1) % m_CurrentColor.colors.Length;// Random.Range(0, m_CurrentColor.colors.Length);
        ColorBIndex = (ColorAIndex + 1) % m_CurrentColor.colors.Length;
        mat.SetColor("_ColorUpB", m_CurrentColor.colors[ColorAIndex].topColor);
        mat.SetColor("_ColorDownB", m_CurrentColor.colors[ColorAIndex].downColor);
        m_SharpLerpTarget = m_CurrentColor.colors[ColorAIndex];

        mat.SetFloat("_ColorLerp", 0);

        m_SharpLerpToBlack = false;
    }

    public void StartSharpLerpToBlack()
    {
        m_CurrentFloor = GetCurrentFloorColor();
        m_SharpLerpTime = m_Storer.sharpLerpTime;
        float lerpAmount = time / lerpTime;
        m_CurrentTop = Color.Lerp(m_CurrentColor.colors[ColorAIndex].topColor, m_CurrentColor.colors[ColorBIndex].topColor, lerpAmount);
        m_CurrentDown = Color.Lerp(m_CurrentColor.colors[ColorAIndex].downColor, m_CurrentColor.colors[ColorBIndex].downColor, lerpAmount);

        mat.SetColor("_ColorUpA", m_CurrentTop);
        mat.SetColor("_ColorDownA", m_CurrentDown);

        ///ColorAIndex = (ColorAIndex + 1) % m_CurrentColor.colors.Length;// Random.Range(0, m_CurrentColor.colors.Length);
        //ColorBIndex = (ColorAIndex + 1) % m_CurrentColor.colors.Length;
        mat.SetColor("_ColorUpB", Color.black); // m_CurrentColor.colors[ColorAIndex].topColor);
        mat.SetColor("_ColorDownB", Color.black); // m_CurrentColor.colors[ColorAIndex].downColor);
        m_SharpLerpTarget = blackColor;

        mat.SetFloat("_ColorLerp", 0);

        m_SharpLerpToBlack = true;
    }

    void Update () {
        time += Time.deltaTime;
        //floorTime += Time.deltaTime;
        if(m_SharpLerpTime > 0)
        {
            float lerpAmount = 1 - m_SharpLerpTime / m_Storer.sharpLerpTime;
            mat.SetFloat("_ColorLerp", lerpAmount); 
            m_SharpLerpTime -= Time.deltaTime;
            FloorBuilder.current.ChangeFloorColor();
            if (m_SharpLerpTime <= 0)
            {
                if(m_SharpLerpToBlack)
                {
                    SetColorA(blackColor);
                    SetColorB(blackColor);
                }
                else
                {
                    SetColorA(m_CurrentColor.colors[ColorAIndex]);
                    SetColorB(m_CurrentColor.colors[ColorBIndex]);
                }
                mat.SetFloat("_ColorLerp", 0);
                time = 0;
                FloorBuilder.current.ChangeFloorColor();
            }
        }
        else if (time >= lerpTime)
        {
            m_Increasing = false;
            time = 0;

            ColorAIndex = ColorBIndex;
            SetColorA(m_CurrentColor.colors[ColorAIndex]);
            ColorBIndex = (ColorBIndex + 1) % m_CurrentColor.colors.Length;
            SetColorB(m_CurrentColor.colors[ColorBIndex]);
            mat.SetFloat("_ColorLerp", time / lerpTime);
        }
        else
        {
            mat.SetFloat("_ColorLerp", time / lerpTime);
            FloorBuilder.current.ChangeFloorColor();
        }
        /*if(floorTime >= lerpTime)
        {
            FloorColAIndex = FloorColBIndex;
            FloorColBIndex = (FloorColBIndex + 1) % m_CurrentColor.colors.Length;
            floorTime = 0;
        }*/
    }
}
