using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrailCard : MonoBehaviour {

    public SelectCarManager m_Manager;
    public string TrailName;

    Text m_TrailName;
    GameObject m_BuyButtonGO;
    Button m_BuyButton;
    GameObject m_SelectButtonGO;
    Button m_SelectButton;
    Text m_SelectText;
    GameObject m_TrailGO;
    Text m_PriceText;
    GameObject m_PriceIconGO;

    bool inited = false;

    Color selectedColor;

    void Awake () {
        selectedColor = new Color(100.0f / 255.0f, 120.0f / 255.0f, 135.0f / 255.0f);
    }

    void Init()
    {
        m_TrailName = transform.Find("Name").GetComponent<Text>();
        m_BuyButtonGO = transform.Find("Button").gameObject;
        m_BuyButton = transform.Find("Button/BuyButton").GetComponent<Button>();
        m_SelectButtonGO = transform.Find("SelectButton").gameObject;
        m_SelectButton = transform.Find("SelectButton/SelectBut").GetComponent<Button>();
        m_SelectText = transform.Find("SelectButton/Text").GetComponent<Text>();
        m_BuyButton.onClick.AddListener(BuyTrail);
        m_SelectButton.onClick.AddListener(SelectTrail);
        m_PriceText = transform.Find("Button/Price").GetComponent<Text>();
        m_PriceIconGO = transform.Find("Button/Icon").gameObject;
        inited = true;
    }

    public void BuyTrail()
    {
        if(SaveManager.instance.BuyTrail(TrailName))
        {
            SelectTrail();
            m_BuyButtonGO.SetActive(false);
            m_SelectButtonGO.SetActive(true);
        }
    }

    public void SelectTrail()
    {
        m_Manager.SelectTrail(TrailName);
    }

    public void RefreshUI(TrailSelectData data)
    {
        if (!inited) Init();

        TrailName = data.name;
        m_TrailName.text = data.name;

        if (m_TrailGO == null)
        {
            m_TrailGO = Instantiate(data.TrailDisplayPrefab, transform.position, Quaternion.identity);
            m_TrailGO.transform.parent = transform;
        }
        
        if(SaveManager.instance.HasTrail(data.name))
        {
            m_BuyButtonGO.SetActive(false);
            m_SelectButtonGO.SetActive(true);
            m_PriceText.text = "";
            m_PriceIconGO.SetActive(false);

            if (GameManager.current.m_DefaultTrailName == TrailName)
            {
                m_SelectButton.image.color = selectedColor;
                m_SelectText.text = "EQUIPED";
            }
            else
            {
                m_SelectButton.image.color = Color.white;
                m_SelectText.text = "EQUIP";
            }
        }
        else
        {
            m_BuyButtonGO.SetActive(true);
            m_SelectButtonGO.SetActive(false);
            m_PriceText.text = data.price.ToString();
            m_PriceIconGO.SetActive(true);
        }


        m_TrailGO.SetActive(true);
    }

    private void OnDisable()
    {
        if(m_TrailGO)
        {
            m_TrailGO.SetActive(false);
        }
    }
}
