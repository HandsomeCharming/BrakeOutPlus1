using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMenuUI : MonoBehaviour {
    
    public SelectCarManager m_Manager;
    public Button m_NextButton;
    public Button m_PrevButton;
    public Text m_CarText;
    public GameObject m_BuyButton;
    public GameObject m_TrailButton;
    public GameObject[] m_SceneGOs;
    public RectTransform m_CarRotBotLeft;
    public RectTransform m_CarRotTopRight;
    public float m_RotSpeed;

    [HideInInspector]
    public GameObject m_CurrentCarPreview;

    bool m_CurrentLocked;
    Vector2 m_BotLeft;
    Vector2 m_TopRight;

    bool m_RotatingPreview;
    Vector2 m_LastTouchPos;
    float m_PreviewRotSpeed;

    // Use this for initialization
    void Start ()
    {
        m_NextButton.onClick.RemoveAllListeners();
        m_NextButton.onClick.AddListener(NextCar);
        m_PrevButton.onClick.RemoveAllListeners();
        m_PrevButton.onClick.AddListener(PrevCar);

        RefreshCarUI();

        m_BotLeft = RectTransformUtility.WorldToScreenPoint(Camera.main, m_CarRotBotLeft.position);
        m_TopRight = RectTransformUtility.WorldToScreenPoint(Camera.main, m_CarRotTopRight.position);
        print(m_BotLeft);
        print(m_TopRight);
        m_RotatingPreview = false;
    }
	
    public void ChangeSceneGO(int index)
    {
        for(int i=0; i<m_SceneGOs.Length;++i)
        {
            if(i != index)
                m_SceneGOs[i].SetActive(false);
            else
                m_SceneGOs[i].SetActive(true);
        }
    }

    public void RefreshCarUI()
    {
        m_CarText.text = m_Manager.GetCurrentCarName();

        if (!m_Manager.isCurrentCarAvailable())
        {
            m_CurrentLocked = true;
            m_BuyButton.SetActive(true);
            m_TrailButton.SetActive(false);
        }
        else
        {
            m_CurrentLocked = false;
            m_BuyButton.SetActive(false);
            if(m_Manager.GetCurrentCarData().CanChangeTrail)
                m_TrailButton.SetActive(true);
        }

        LoadCurrentCarPreview();
    }

    void NextCar()
    {
        m_Manager.NextCar();

        RefreshCarUI();
    }

    void PrevCar()
    {
        m_Manager.PrevCar();

        RefreshCarUI();
    }

    public void LoadCurrentCarPreview()
    {
        if(m_CurrentCarPreview != null)
        {
            Destroy(m_CurrentCarPreview);
        }

        SingleCarSelectData data = m_Manager.GetCurrentCarData();
        if (data.CarForViewPrefab != null)
        {
            m_CurrentCarPreview = Instantiate(data.CarForViewPrefab, Camera.main.transform);

            if (!data.customViewPos)
                m_CurrentCarPreview.transform.localPosition = new Vector3(1.0f, 0.8f, 5.0f);
            else
                m_CurrentCarPreview.transform.localPosition = data.ViewPos;


            if (!data.customViewRot)
                m_CurrentCarPreview.transform.localRotation = Quaternion.Euler(0, 180.0f, 0);
            else
                m_CurrentCarPreview.transform.localRotation = Quaternion.Euler(data.ViewRot);
        }
    }

    public void SetText(string str)
    {
        m_CarText.text = str;
    }

    public void RefreshForManager()
    {
        m_CarText.text = m_Manager.GetCurrentCarName();
        RefreshCarUI();
    }

    private void OnDisable()
    {
        Destroy(m_CurrentCarPreview);
    }

    private void OnEnable()
    {
        RefreshCarUI();
    }

    bool isInBox(Vector2 botLeft, Vector2 topRight, Vector2 target)
    {
        return target.x > botLeft.x && target.y > botLeft.y && target.x < topRight.x && target.y < topRight.y;
    }

    Touch GetTouchByMouse()
    {
        Touch touch = new Touch();
        touch = new Touch();
        if(Input.GetMouseButtonDown(0))
        {
            touch.phase = TouchPhase.Began;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            touch.phase = TouchPhase.Ended;
        }
        else
        {
            touch.phase = TouchPhase.Moved;
        }
        touch.position = Input.mousePosition;
        return touch;
    }

    // Update is called once per frame
    void Update () {
		if(!m_CurrentLocked)
        {
            if(Input.touchCount == 1 || Input.GetMouseButton(0))
            {
                Touch touch;
                if (Input.touchCount == 1)
                    touch = Input.GetTouch(0);
                else// if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
                {
                    touch = GetTouchByMouse();
                }
                Vector2 pos = touch.position;
                if (touch.phase == TouchPhase.Began && isInBox(m_BotLeft, m_TopRight, pos))
                {
                    print(pos);
                    m_RotatingPreview = true;
                    m_LastTouchPos = Input.mousePosition;
                }
                else if(m_RotatingPreview)
                {
                    if (/*isInBox(m_BotLeft, m_TopRight, pos) &&*/ touch.phase != TouchPhase.Ended)
                    {
                        m_CurrentCarPreview.transform.Rotate(0, (m_LastTouchPos.x - pos.x) * m_RotSpeed, 0);
                        m_LastTouchPos = pos;
                    }
                    else
                    {
                        EndTouchRot(pos);
                    }
                }
                //print(Input.touches[0].position);
            }
            
            /*if(Input.GetMouseButtonDown(0))
            {
            }
            if(Input.GetMouseButton(0))
            {
                m_CurrentCarPreview.transform.Rotate(0, (m_LastTouchPos.x - Input.mousePosition.x) * m_RotSpeed, 0);
                m_LastTouchPos = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
            }*/
            if(!m_RotatingPreview && m_PreviewRotSpeed != 0)
            {
                m_CurrentCarPreview.transform.Rotate(0, m_PreviewRotSpeed, 0);
                m_PreviewRotSpeed = Mathf.Lerp(m_PreviewRotSpeed, 0, 0.2f);
                if(Mathf.Abs(m_PreviewRotSpeed) < 0.01f)
                {
                    m_PreviewRotSpeed = 0;
                }
            }
        }
	}

    void EndTouchRot(Vector2 pos)
    {
        m_PreviewRotSpeed = (m_LastTouchPos.x - pos.x) * m_RotSpeed;
        m_CurrentCarPreview.transform.Rotate(0, m_PreviewRotSpeed, 0);
        m_LastTouchPos = pos;
        m_RotatingPreview = false;
    }
}
