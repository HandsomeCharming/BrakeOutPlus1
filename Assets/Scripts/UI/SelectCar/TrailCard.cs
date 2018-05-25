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
    GameObject m_TrailGO;

    bool inited = false;

    void Awake () {

    }

    void Init()
    {
        m_TrailName = transform.Find("Name").GetComponent<Text>();
        m_BuyButtonGO = transform.Find("Button").gameObject;
        m_BuyButton = transform.Find("Button/BuyButton").GetComponent<Button>();
        m_SelectButtonGO = transform.Find("SelectButton").gameObject;
        m_SelectButton = transform.Find("SelectButton/SelectBut").GetComponent<Button>();
        m_BuyButton.onClick.AddListener(BuyTrail);
        m_SelectButton.onClick.AddListener(SelectTrail);
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
        }
        else
        {
            m_BuyButtonGO.SetActive(true);
            m_SelectButtonGO.SetActive(false);
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
