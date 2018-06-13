using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadMenuUI : UIBase
{
    public static DeadMenuUI current;

    public Button CollectButton;
    public Button DoubleCollectButton;
    public Text m_CoinEarned;
    public Text m_Score;

    // Use this for initialization
    void Awake () {
        current = this;

        CollectButton.onClick.RemoveAllListeners();
        CollectButton.onClick.AddListener(ReloadGameAndCollect);
        DoubleCollectButton.onClick.RemoveAllListeners();
        DoubleCollectButton.onClick.AddListener(TryShowDoubleVideo);
    }

    private void OnEnable()
    {
        m_Score.text = ((int)GameManager.current.gameScore).ToString() ;
        m_CoinEarned.text = ((int)GameManager.current.singleGameCoins).ToString();
    }

    public void ReloadGameAndCollect()
    {
        GameManager.current.CollectInGameCoins();
        GameManager.current.Reload();
    }

    void TryShowDoubleVideo()
    {
        if (!AdManager.Instance.ShowDoubleCollectVideo())
        {
            ReloadGameAndCollect();
        }
    }

    public void ReloadGameAndCollectDouble()
    {
        GameManager.current.CollectInGameCoins(true);
        GameManager.current.Reload();
    }
}
