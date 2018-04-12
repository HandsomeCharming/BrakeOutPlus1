using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadMenuUI : UIBase
{

    public Button restart;
    public Button revive;
    public Text m_Score;

    // Use this for initialization
    void Awake () {
        restart.onClick.RemoveAllListeners();
        restart.onClick.AddListener(ReloadGame);
    }

    private void OnEnable()
    {
        m_Score.text = ((int)GameManager.current.gameScore).ToString() ;
    }

    void ReloadGame()
    {
        GameManager.current.Reload();
    }
}
