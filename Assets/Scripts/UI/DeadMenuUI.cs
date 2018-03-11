using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadMenuUI : UIBase
{

    public Button restart;
    public Button revive;

    // Use this for initialization
    void Awake () {
        restart.onClick.RemoveAllListeners();
        restart.onClick.AddListener(ReloadGame);
    }
	
    void ReloadGame()
    {
        GameManager.current.Reload();
    }
}
