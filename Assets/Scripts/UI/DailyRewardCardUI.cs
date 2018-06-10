using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DailyRewardCardUI : MonoBehaviour {
    public GameObject m_SelectedImage;
    
    public void SetSelected(bool value)
    {
        m_SelectedImage.SetActive(value);
    }
}
