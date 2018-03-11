using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : UIBase
{

    public Button resume;
    public Button restart;

    // Use this for initialization
    void Awake()
    {
        restart.onClick.RemoveAllListeners();
        resume.onClick.RemoveAllListeners();
        restart.onClick.AddListener(ReloadGame);
        resume.onClick.AddListener(Resume);
    }

    void ReloadGame()
    {
        GameManager.current.Reload();
    }
    void Resume()
    {
        GameManager.current.Pause(false);
    }
}
