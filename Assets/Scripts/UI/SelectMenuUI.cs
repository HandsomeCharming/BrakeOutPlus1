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
    public Text m_CoinPrice;
    public Text m_StarPrice;
    public Text m_FirstFeature;
    public Text m_SecondFeature;
    public GameObject m_TrailButton;
    public GameObject m_Foreground;
    public GameObject[] m_SceneGOs;
    public GameObject[] m_SceneButtons;
    public CarUpgradeCard[] m_CarUpgradeCards;
    public RectTransform m_CarRotBotLeft;
    public RectTransform m_CarRotTopRight;
    public GameObject[] m_Rarities;
    public float m_RotSpeed;
    public float m_RotRecoverSpeed = 1.0f;

    [HideInInspector]
    public GameObject m_CurrentCarPreview;

    bool m_CurrentLocked;
    Vector2 m_BotLeft;
    Vector2 m_TopRight;

    bool m_RotatingPreview;
    Vector2 m_LastTouchPos;
    float m_PreviewRotSpeed;
    Vector3 m_CurrentRot;
    Quaternion m_InitRot;
    Quaternion m_EndTouchRot;
    float m_RotSlerpAmount;

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

    public void ChangeScene(int index)
    {
        print("change scene");
        for(int i=0; i< m_SceneButtons.Length; ++i)
        {
            if (i == index)
                m_SceneButtons[i].transform.localScale = Vector3.one;
            else
                m_SceneButtons[i].transform.localScale = Vector3.one * 0.9f;
        }
        ChangeSceneGO(index);
    }

    public void RefreshCarUI()
    {
        m_CarText.text = m_Manager.GetCurrentCarName();

        if (!m_Manager.isCurrentCarAvailable())
        {
            m_CurrentLocked = true;
            ShowBuyButtonAndPrice(true);
            m_TrailButton.SetActive(false);
        }
        else
        {
            m_CurrentLocked = false;
            ShowBuyButtonAndPrice(false);
            if(m_Manager.GetCurrentCarData().CanChangeTrail)
                m_TrailButton.SetActive(true);
        }

        SetCurrentRarity();
        ChangeFeatureText();
        RefreshUpgradeCards();
        LoadCurrentCarPreview();
    }

    void SetCurrentRarity()
    {
        var carData = m_Manager.GetCurrentCarData();
        Rarity rarity = carData.rarity;
        int rarityIndex = (int)rarity;

        foreach(var go in m_Rarities)
        {
            go.SetActive(false);
        }
        m_Rarities[rarityIndex].SetActive(true);
    }

    void ChangeFeatureText()
    {
        var carData = m_Manager.GetCurrentCarData();
        m_FirstFeature.text = carData.firstLine;
        m_SecondFeature.text = carData.secondLine;
    }

    public void ShowBuyButtonAndPrice(bool show)
    {
        m_BuyButton.SetActive(show);
        if(show)
        {
            var carData = m_Manager.GetCurrentCarData();
            m_CoinPrice.text = carData.coinPrice.ToString();
            m_StarPrice.text = carData.starPrice.ToString();
        }
    }

    public void RefreshUpgradeCards()
    {
        // hard code lol
        SingleCarSelectData carData = m_Manager.GetCurrentCarData();
        CarClassData classData = CarSelectDataReader.Instance.GetCarClassData(carData.carClass);
        CarSaveData saveData = SaveManager.instance.GetSavedCarData(carData.name);
        bool hasCar = saveData != null;
        for (int i=0; i<3; ++i)
        {
            CarUpgradeCatagory type = (CarUpgradeCatagory)i;
            int level = 0;
            float min = 0, max = 0;
            switch (type)
            {
                case CarUpgradeCatagory.Accelerate:
                    if(saveData != null)
                        level = saveData.m_AccLevel;
                    min = classData.m_MinAcceleration;
                    max = classData.m_MaxAcceleration;
                    break;
                case CarUpgradeCatagory.Boost:
                    if (saveData != null)
                        level = saveData.m_BoostLevel;
                    min = classData.m_MinBoost;
                    max = classData.m_MaxBoost;
                    break;
                case CarUpgradeCatagory.Handling:
                    if (saveData != null)
                        level = saveData.m_HandlingLevel;
                    min = classData.m_MinHandling;
                    max = classData.m_MaxHandling;
                    break;
            }
			float minScale = min;
            float addedscale = Mathf.Lerp(min, max, (float)level / carData.maxUpgradeLevel) - minScale;
			m_CarUpgradeCards[i].RefreshUI(level, carData.maxUpgradeLevel, minScale, addedscale, hasCar, carData.GetUpgradePrice(level, type));
        }
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

            Vector3 rot = Vector3.one;
            if (!data.customViewRot)
                rot = new Vector3(0, 180.0f, 0);
            else
                rot = data.ViewRot;

            m_CurrentCarPreview.transform.localEulerAngles = rot;
            m_CurrentRot = m_CurrentCarPreview.transform.localEulerAngles;
            m_InitRot = m_CurrentCarPreview.transform.localRotation;
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

    public void OpenTrailMenu()
    {
        //RefreshCarUI();
        m_Foreground.SetActive(false);
        m_BuyButton.SetActive(false);
        m_CurrentCarPreview.SetActive(false);
    }

    public void CloseTrailMenu()
    {
        RefreshCarUI();
        m_Foreground.SetActive(true); 
        m_CurrentCarPreview.SetActive(true);
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
                        //m_CurrentCarPreview.transform.Rotate((m_LastTouchPos.y - pos.y) * m_RotSpeed, (m_LastTouchPos.x - pos.x) * m_RotSpeed, 0);
                        //m_CurrentRot.x += (m_LastTouchPos.y - pos.y) * m_RotSpeed;
                        m_CurrentRot.y += (m_LastTouchPos.x - pos.x) * m_RotSpeed;
                        m_CurrentCarPreview.transform.localEulerAngles = m_CurrentRot;

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
            }*/
            if (Input.GetMouseButtonUp(0))
            {
                EndTouchRot(Input.mousePosition);
            }
            if(!m_RotatingPreview && m_EndTouchRot != m_InitRot)
            {
                /*m_CurrentCarPreview.transform.Rotate(0, m_PreviewRotSpeed, 0);
                m_PreviewRotSpeed = Mathf.Lerp(m_PreviewRotSpeed, 0, 0.2f);
                if(Mathf.Abs(m_PreviewRotSpeed) < 0.01f)
                {
                    m_PreviewRotSpeed = 0;
                }*/
                m_CurrentRot = m_CurrentCarPreview.transform.localEulerAngles;
                m_RotSlerpAmount += m_RotRecoverSpeed * Time.deltaTime;
                m_CurrentCarPreview.transform.localRotation = Quaternion.Lerp(m_EndTouchRot, m_InitRot, m_RotSlerpAmount);
            }
        }
	}

    void EndTouchRot(Vector2 pos)
    {
        m_PreviewRotSpeed = (m_LastTouchPos.x - pos.x) * m_RotSpeed;
        //m_CurrentCarPreview.transform.Rotate(0, m_PreviewRotSpeed, 0);
        m_LastTouchPos = pos;
        m_EndTouchRot = m_CurrentCarPreview.transform.localRotation;
        m_RotSlerpAmount = 0;
        m_RotatingPreview = false;
    }
}
