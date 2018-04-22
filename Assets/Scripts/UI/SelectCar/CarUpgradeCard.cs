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
    public RectTransform m_Bar;
    public Button m_UpgradeButton;

    public SelectCarManager m_Manager;

	// Use this for initialization
	void Start () {
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

    public void RefreshUI(int level, int maxLevel, float scalex, bool hasCar, int price)
    {
        m_LevelText.text = (level+1).ToString() + "/" + (maxLevel + 1).ToString();
        m_Price.text = price.ToString();

        Vector3 scale = m_Bar.localScale;
        scale.x = scalex;
        m_Bar.localScale = scale;

        bool canUpgrade = level != maxLevel && hasCar;
        m_UpgradeButton.gameObject.SetActive(canUpgrade);
        m_Price.gameObject.SetActive(canUpgrade);
        m_CoinIcon.SetActive(canUpgrade);
    }
}
