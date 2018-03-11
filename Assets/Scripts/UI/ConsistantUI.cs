using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ConsistantUI : MonoBehaviour {

    public static ConsistantUI current;
    
    public Text coinNumbers;
    public Text starNumbers;

    private void Awake()
    {
        current = this;
    }

    public static void UpdateCoinAndStar()
    {
        if(current)
        {
            current.UpdateNumbers();
        }
    }

    public void UpdateNumbers()
    {
        starNumbers.text = ((int)GameManager.current.gameStars).ToString();
        coinNumbers.text = GameManager.current.GetCoinCount().ToString();
    }

    /*void Update()
    {
        if (GameManager.current)
        {
            if (GameManager.current.state == GameManager.GameState.Running)
            {
                UpdateNumbers();
            }
        }
    }*/
}
