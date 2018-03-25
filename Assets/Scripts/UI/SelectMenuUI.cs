using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMenuUI : MonoBehaviour {
    
    public SelectCarManager m_Manager;
    public Button m_NextButton;
    public Button m_PrevButton;
    public Text m_CarText;

    // Use this for initialization
    void Awake ()
    {
        m_NextButton.onClick.AddListener(NextCar);
        m_PrevButton.onClick.AddListener(PrevCar);

        m_CarText.text = m_Manager.GetCurrentCarName();
    }
	
    void NextCar()
    {
        m_Manager.NextCar();
        m_CarText.text = m_Manager.GetCurrentCarName();
    }

    void PrevCar()
    {
        m_Manager.PrevCar();
        m_CarText.text = m_Manager.GetCurrentCarName();
    }

    public void SetText(string str)
    {
        m_CarText.text = str;
    }

    public void RefreshForManager()
    {
        m_CarText.text = m_Manager.GetCurrentCarName();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
