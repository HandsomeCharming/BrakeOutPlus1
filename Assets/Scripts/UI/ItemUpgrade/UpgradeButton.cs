using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour {

    public Text NameText;
    public Text LevelText;
    public Text CoinText;
    public Transform SlotParent;
    
    public int m_Level;

    Image[] m_Slots;
    const int maxLevel = 5;
    
    void Awake () {
        //m_Slots = SlotParent.GetComponentsInChildren<Image>();
        m_Level = 1;

        // Todo: add save load
        SetUIToLevel();
    }

    public void TryToUpgrade()
    {
        m_Level++;
        // Todo: add coin cost
        SetUIToLevel();
    }
	
    void SetUIToLevel()
    {
        Color solid = Color.white;
        Color trans = solid;
        trans.a = 0.5f;

        if(m_Slots != null)
        {
            for (int i = 0; i < m_Level; ++i)
            {
                m_Slots[i].color = solid;
            }
            for (int i = m_Level; i < maxLevel; ++i)
            {
                m_Slots[i].color = trans;
            }
        }
    }
}
