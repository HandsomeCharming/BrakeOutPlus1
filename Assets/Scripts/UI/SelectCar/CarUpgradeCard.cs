using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum CarUpgradeCatagory
{
    Accelerate,
    Handling,
    Boost
}


public class CarUpgradeCard : MonoBehaviour {

    public CarUpgradeCatagory m_Type;
    public Text m_LevelText;
    public Text m_Price;
    public GameObject m_CoinIcon;
	public RectTransform m_MinBar;
	public RectTransform m_AddedBar;
    public Button m_UpgradeButton;
    public Text m_UpgradeText;


    public SelectCarManager m_Manager;
	// Use this for initialization
	void Awake () {
        m_UpgradeButton.onClick.AddListener(Upgrade);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Upgrade()
    {
        if(m_Manager)
        {
            m_Manager.UpgradeCurrentCar(m_Type);
        }
        else
        {
            Debug.LogError("No manager to upgrade car");
        }
    }

	public void RefreshUI(int level, int maxLevel, float minScalex, float addScalex, bool hasCar, int price)
    {
        m_LevelText.text = (level+1).ToString() + "/" + (maxLevel + 1).ToString();
        m_Price.text = price.ToString();

		Vector3 minScale = m_MinBar.localScale;
		minScale.x = minScalex;
        m_MinBar.localScale = minScale;

		Vector3 addedScale = m_MinBar.localScale;
		addedScale.x = addScalex;
		m_AddedBar.localScale = addedScale;
		Vector3 addedPos = m_AddedBar.anchoredPosition;
		addedPos.x = -795.0f + (420.0f * minScalex);
		m_AddedBar.anchoredPosition = addedPos;

        bool canUpgrade = level != maxLevel && hasCar;
        m_UpgradeButton.enabled = canUpgrade;
        m_UpgradeText.text = canUpgrade ? "UPGRADE" : "MAX";
        m_Price.gameObject.SetActive(canUpgrade);
		m_CoinIcon.SetActive(canUpgrade);
		m_UpgradeButton.gameObject.SetActive (hasCar);
    }
}
