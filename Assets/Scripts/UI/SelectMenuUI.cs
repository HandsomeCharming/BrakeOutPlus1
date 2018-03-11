using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMenuUI : MonoBehaviour {

    [HideInInspector]
    public SelectCarManager m_Manager;
    public Button m_NextButton;
    public Button m_PrevButton;
    public Text m_CarText;

    // Use this for initialization
    void Awake ()
    {
        m_NextButton.onClick.AddListener(NextCar);
        m_PrevButton.onClick.AddListener(PrevCar);

    }
	
    void NextCar()
    {
        m_Manager.NextCar();
        m_CarText.text = m_Manager.m_CurrentCarName;
    }

    void PrevCar()
    {
        m_Manager.PrevCar();
        m_CarText.text = m_Manager.m_CurrentCarName;
    }

    public void SetText(string str)
    {
        m_CarText.text = str;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
