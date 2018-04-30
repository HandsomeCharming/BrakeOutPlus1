using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCardSingle : MonoBehaviour {
    

    GameObject m_SelectedCard;
	// Use this for initialization
	void Start () {
        m_SelectedCard = transform.Find("Selected").gameObject;
	}
	
    public void Selected(bool select)
    {
        m_SelectedCard.SetActive(select);
    }
}
