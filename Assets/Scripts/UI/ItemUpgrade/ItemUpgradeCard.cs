using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUpgradeCard : MonoBehaviour {

    public ItemType m_Type;
    
    [HideInInspector]
    public Transform SlotParent;
    [HideInInspector]
    public Button m_PurchaseButton;
	[HideInInspector]
	public Image m_PurchaseButtonImage;
    [HideInInspector]
    public Text m_PurchaseText;
    [HideInInspector]
    public Image m_CoinImage;
	[HideInInspector]
	public Image m_ItemImage;
    [HideInInspector]
    public Text m_PriceText;
    [HideInInspector]
    public Text m_LevelText;

    [HideInInspector]
    public int m_Level;
	[HideInInspector]
	public int m_Price;

    Image[] m_Slots;

    const int maxLevel = 5;
    const string SlotsName = "LevelSlot";
    const string ButtonName = "ButtonBase";
    const string ButtonTextName = "Upgrade";
    const string CoinImageName = "Coin";
	const string ItemImageName = "Icon";
    const string CoinPriceName = "CoinNum";
    const string LevelTextName = "Lv";

    bool inited = false;

    // Use this for initialization
    void Start () {

        SlotParent = transform.Find(SlotsName).transform;
        m_PurchaseButton = transform.Find(ButtonName).GetComponent<Button>();
		m_PurchaseButtonImage = transform.Find(ButtonName).GetComponent<Image>();
        m_PurchaseText = transform.Find(ButtonTextName).GetComponent<Text>();
        m_CoinImage = transform.Find(CoinImageName).GetComponent<Image>();
		m_ItemImage = transform.Find(ItemImageName).GetComponent<Image>();
        m_PriceText = transform.Find(CoinPriceName).GetComponent<Text>();
		m_LevelText = transform.Find(LevelTextName).GetComponent<Text>();

        m_Slots = SlotParent.GetComponentsInChildren<Image>();
        m_Level = ItemManager.current.GetItemLevel(m_Type);
		m_Price = ItemManager.current.GetItemPrice(m_Type);
        m_PurchaseButton.onClick.AddListener(BuyItem);

        RefreshUI();
        inited = true;
    }

    private void OnEnable()
    {
        if(inited)
            RefreshUI();
    }

    public void RefreshUI()
    {
        m_Level = ItemManager.current.GetItemLevel(m_Type);
		m_Price = ItemManager.current.GetItemPrice(m_Type);

		Color purchaseColor = new Color(0.153F, 0.667F, 0.882F);
		Color insufficientCoinColor = new Color(0.69F, 0.69F, 0.69F);
		Color solid = Color.white;
		Color trans = solid;
		trans.a = 0.785f;

        if (m_Level == 5)
        {
            m_PriceText.gameObject.SetActive(false);
            m_CoinImage.gameObject.SetActive(false);
			m_PurchaseText.text = "MAX";
            SetSlotToLevel();
            m_LevelText.text = "Lv 5/5";
			//m_ItemImage.rectTransform.anchoredPosition = new Vector2(-275.0F, 32.5F);
			//m_ItemImage.rectTransform.localScale = new Vector3(0.32F, 0.32F, 0.20F);
			m_ItemImage.GetComponent<Animator>().Play("ItemImageEnlarged");
        }
        else if (m_Level == 0)
        {
            m_PriceText.text = ItemManager.current.GetItemPrice(m_Type).ToString();
            m_PurchaseText.text = "PURCHASE";
            SlotParent.gameObject.SetActive(true);
			SetSlotToLevel();
            m_LevelText.text = "";
			//m_ItemImage.rectTransform.anchoredPosition = new Vector2(-275.0F, 52.7F);
			//m_ItemImage.rectTransform.localScale = new Vector3(0.25F, 0.25F, 0.20F);
			m_ItemImage.GetComponent<Animator>().Play("ItemImageNothing");

			if (GameManager.current.gameCoins < m_Price) {
				m_PurchaseText.color = insufficientCoinColor;
				m_PurchaseButtonImage.color = trans;
			} 
			else 
			{
				m_PurchaseText.color = purchaseColor;
				m_PurchaseButtonImage.color = solid;
			}
        }
        else
        {
            m_PriceText.text = ItemManager.current.GetItemPrice(m_Type).ToString();
            m_PurchaseText.text = "UPGRADE";
            SlotParent.gameObject.SetActive(true);
            SetSlotToLevel();
            m_LevelText.text = "Lv " + m_Level.ToString() + "/5";
			//m_ItemImage.rectTransform.anchoredPosition = new Vector2(-275.0F, 52.7F);
			//m_ItemImage.rectTransform.localScale = new Vector3(0.25F, 0.25F, 0.20F);
			m_ItemImage.GetComponent<Animator>().Play("ItemImageNothing");

			if (GameManager.current.gameCoins < m_Price) 
			{
				m_PurchaseText.color = insufficientCoinColor;
				m_PurchaseButtonImage.color = trans;
			} 
			else 
			{
				m_PurchaseText.color = purchaseColor;
				m_PurchaseButtonImage.color = solid;
			}
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetSlotToLevel()
    {
		Color filled = new Color(0.427F, 0.831F, 1.0F);
        Color solid = Color.white;
        Color trans = solid;
        trans.a = 0.5f;

        if (m_Slots != null)
        {
			if (m_Level == 0) 
			{
				for (int i = 0; i < maxLevel; ++i) 
				{
					m_Slots [i].color = trans;
				}
			} 
			else 
			{
				for (int i = 0; i < m_Level; ++i)
				{
					m_Slots[i].color = filled;
				}
				for (int i = m_Level; i < maxLevel; ++i)
				{
					m_Slots[i].color = solid;
				}
			}
            
        }
    }

    public void BuyItem()
    {
		if (m_Level == 4)
		{
			m_ItemImage.GetComponent<Animator>().Play("ItemImageEnlarge");
		}

		ItemManager.current.UpgradeItem(m_Type);
        RefreshUI();
    }
}
