using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    List<UIBase> m_UIs;

    public InGameUI m_Ingame;
    public MainMenuUI m_MainMenu;
    public ReviveMenuUI m_ReviveMenu;
    public DeadMenuUI m_DeadMenu;
    public PauseMenuUI m_PauseMenu;
    public ConsistantUI m_ConsistantUI;
    public LoginUI m_LoginUI;

    bool showMain;

    public static UIManager current;
    
	void Awake () {
        current = this;
        m_UIs = new List<UIBase>();
        m_UIs.Add(m_Ingame);
        m_UIs.Add(m_MainMenu);
        m_UIs.Add(m_ReviveMenu);
        m_UIs.Add(m_DeadMenu);
        m_UIs.Add(m_PauseMenu);
        m_UIs.Add(m_LoginUI);
        ChangeStateByGameState();
    }

    public void ChangeStateByGameState()
    {
        m_ConsistantUI.UpdateNumbers();
		if (GameManager.current.state == GameManager.GameState.Start || GameManager.current.state == GameManager.GameState.AssembleTrack )
        {
            if(AppManager.instance.HasName())
                StartMainMenu();
            else
			{
				AppManager.instance.RegisterPlayer("Driver"); // hard coded register 
				StartMainMenu();
            }
        }
        else if (GameManager.current.state == GameManager.GameState.Running)
        {
            StartGame();
        }
        else if (GameManager.current.state == GameManager.GameState.Paused)
        {
            Pause();
        }
        else if (GameManager.current.state == GameManager.GameState.ReviveMenu)
        {
            ShowRevive();
        }
        else if (GameManager.current.state == GameManager.GameState.Login)
        {

        }

        if (GameManager.current.state == GameManager.GameState.Dead)
        {
            GameOver();
        }
    }

    void DisableAll()
    {
        foreach(var ui in m_UIs)
        {
            ui.gameObject.SetActive(false);
        }
    }

    void StartMainMenu()
    {
        DisableAll();
        m_MainMenu.gameObject.SetActive(true);
    }
	
    void StartLogin()
    {
        DisableAll();
        m_LoginUI.gameObject.SetActive(true);
    }

    void StartGame()
    {
        DisableAll();
        m_Ingame.gameObject.SetActive(true);
    }

    void GameOver()
    {
		m_Ingame.ClearAllPowerups ();
        DisableAll();
        m_DeadMenu.gameObject.SetActive(true);
    }

    void ShowRevive()
    {
        DisableAll();
        m_ReviveMenu.m_Countdown = 5.0f;
        m_ReviveMenu.gameObject.SetActive(true);
    }

    void Pause()
    {
        DisableAll();
        m_PauseMenu.gameObject.SetActive(true);
    }
}

