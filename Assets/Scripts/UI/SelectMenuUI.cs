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

    public GameObject m_CurrentCarPreview;

    // Use this for initialization
    void Awake ()
    {
        m_NextButton.onClick.AddListener(NextCar);
        m_PrevButton.onClick.AddListener(PrevCar);

        RefreshCarUI();
    }
	
    public void RefreshCarUI()
    {
        m_CarText.text = m_Manager.GetCurrentCarName();

        if (!m_Manager.isCurrentCarAvailable())
        {
            m_BuyButton.SetActive(true);
        }
        else
        {
            m_BuyButton.SetActive(false);
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

    // Update is called once per frame
    void Update () {
		
	}
}
