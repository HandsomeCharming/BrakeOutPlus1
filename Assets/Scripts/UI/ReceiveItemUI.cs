using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceiveItemUI : MonoBehaviour {

    public Text m_PrizeName;
    public Text m_NewOrNot;
    public Button m_BlurBackground;
    public GameObject[] m_Rarities;
    public GameObject[] m_RarityEffectPrefab;

    [HideInInspector]
    public GameObject m_CurrentCarPreview;
    [HideInInspector]
    public GameObject m_CurrentEffect;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReceiveCar(CarShortData data)
    {
        SingleCarSelectData selectData = CarSelectDataReader.Instance.GetCarData(data.carIndex, data.sceneIndex);
        LoadCurrentCarPreview(selectData);

        m_PrizeName.text = selectData.name;
        SetRarity(selectData.rarity);

        if(SaveManager.instance.HasCar(selectData.name))
        {
            m_NewOrNot.text = "Owned";
            if(selectData.returnWhenLootedCurrency == Currency.Coin)
            {
                GameManager.current.AddCoin(selectData.returnedAmountWhenLooted);
            }
            else
            {
                GameManager.current.AddStar(selectData.returnedAmountWhenLooted);
            }
        }
        else
        {
            m_NewOrNot.text = "New";
            SaveManager.instance.AcquireCar(data.carIndex, data.sceneIndex);
        }
        m_BlurBackground.enabled = false;
        Invoke("EnableTouch", 0.5f);

        gameObject.SetActive(true);
    }

    void EnableTouch()
    {
        m_BlurBackground.enabled = true;
    }

    public void SetRarity(Rarity rarity)
    {
        foreach(var go in m_Rarities)
        {
            go.SetActive(false);
        }
        int index = (int)rarity;
        m_Rarities[index].SetActive(true);

        m_CurrentEffect = Instantiate(m_RarityEffectPrefab[index], Camera.main.transform);
        m_CurrentEffect.transform.localPosition = new Vector3(0.0f, 0.8f, 5.0f);
    }

    public void LoadCurrentCarPreview(SingleCarSelectData data)
    {
        if (m_CurrentCarPreview != null)
        {
            Destroy(m_CurrentCarPreview);
        }
        
        if (data.CarForViewPrefab != null)
        {
            m_CurrentCarPreview = Instantiate(data.CarForViewPrefab, Camera.main.transform);

            m_CurrentCarPreview.transform.localPosition = new Vector3(0.0f, -0.4f, 5.0f);

            Vector3 rot = Vector3.one;
            if (!data.customViewRot)
                rot = new Vector3(0, 180.0f, 0);
            else
                rot = data.ViewRot;

            m_CurrentCarPreview.transform.localEulerAngles = rot;
            //m_CurrentRot = m_CurrentCarPreview.transform.localEulerAngles;
            //m_InitRot = m_CurrentCarPreview.transform.localRotation;
        }

    }

    public void Close()
    {
        if (m_CurrentCarPreview != null)
        {
            Destroy(m_CurrentCarPreview);
        }
        if(m_CurrentEffect != null)
        {
            Destroy(m_CurrentEffect);
        }

        LootBoxManager.instance.CloseReceiveCarPanel();
        gameObject.SetActive(false);
    }
}
