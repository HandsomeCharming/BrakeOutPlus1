using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ConsistantUI : MonoBehaviour {

    enum ConsistantUIState
    {
        Normal,
        InGame
    }

    public static ConsistantUI current;

    ConsistantUIState state;

    public float addCoinRate;
    public float addStarRate;
    public float actualAddCoinRate;
    public float actualAddStarRate;

    public Text coinNumbers;
    public Text starNumbers;
    public Text addCoins;
    public Text addStars;

    float m_CurrentCoins;
    float m_CurrentStars;
    float m_ActualCoins;
    float m_ActualStars;
    

    private void Awake()
    {
        current = this;
        m_ActualCoins = GameManager.current.GetCoinCount();
        m_ActualStars = GameManager.current.gameStars;
        m_CurrentCoins = m_ActualCoins;
        m_CurrentStars = m_ActualStars;

        starNumbers.text = ((int)GameManager.current.gameStars).ToString();
        coinNumbers.text = GameManager.current.GetCoinCount().ToString();
    }

    public static void UpdateCoinAndStar()
    {
        if(current)
        {
            current.UpdateNumbers();
        }
    }

    public static void UpdateInGameCoins(int coins)
    {
        if (current)
        {
            current.UpdateInGameCoinsText(coins);
        }
    }

    public void UpdateInGameCoinsText(int coins)
    {
        addCoins.text = "+" + coins.ToString();
    }

    public void UpdateNumbers()
    {
        m_ActualCoins = GameManager.current.GetCoinCount();
        m_ActualStars = GameManager.current.gameStars;

        if(m_ActualCoins + 5.0f < m_CurrentCoins || m_CurrentCoins > m_ActualCoins - 5.0f)
        {
            m_CurrentCoins = m_ActualCoins;
            addCoins.gameObject.SetActive(false);
            coinNumbers.text = ((int)m_CurrentCoins).ToString();
        }
        else
        {
            addCoins.gameObject.SetActive(true);

            int diff = (int)(m_ActualCoins - m_CurrentCoins);

            actualAddCoinRate = addCoinRate;
            if(diff/addCoinRate > 3.0f)
            {
                actualAddCoinRate = diff / 3.0f;
            }

            addCoins.text = "+" + (diff).ToString();
        }

        if(m_ActualStars - 1.0f < m_CurrentStars)
        {
            m_CurrentStars = m_ActualStars;
            addStars.gameObject.SetActive(false);
            starNumbers.text = ((int)m_ActualStars).ToString();
        }
        else
        {
            addStars.gameObject.SetActive(true);

            int diff = (int)(m_ActualStars - m_CurrentStars);
            actualAddStarRate = addStarRate;
            if (diff / addStarRate > 3.0f)
            {
                actualAddStarRate = diff / 3.0f;
            }

            addStars.text = "+" + (diff).ToString();
        }

        if(state == ConsistantUIState.InGame)
        {
            addCoins.gameObject.SetActive(true);
            UpdateInGameAddCoins();
        }
    }
    
    public void StartGameAndSetCurrentToActual()
    {
        m_ActualCoins = GameManager.current.GetCoinCount();
        m_ActualStars = GameManager.current.gameStars;
        m_CurrentCoins = m_ActualCoins;
        m_CurrentStars = m_ActualStars;
        state = ConsistantUIState.InGame;
        UpdateNumbers();
    }

    public void EndGame()
    {
        state = ConsistantUIState.Normal;
        UpdateNumbers();
    }

    private void Update()
    {
        if(state == ConsistantUIState.Normal)
        {
            AddCoinAndStar();
        }
    }

    void UpdateInGameAddCoins()
    {
        if(GameManager.current.singleGameCoins > 0)
        {
            addCoins.text = "+" + GameManager.current.singleGameCoins.ToString();
        }
        else
        {
            addCoins.text = "";
        }
    }

    void AddCoinAndStar()
    {
        if (m_ActualCoins != m_CurrentCoins)
        {
            m_CurrentCoins = m_CurrentCoins + (actualAddCoinRate * Time.deltaTime);
            m_CurrentCoins = m_CurrentCoins > m_ActualCoins ? m_ActualCoins : m_CurrentCoins;

            coinNumbers.text = ((int)m_CurrentCoins).ToString();
            addCoins.text = "+" + ((int)(m_ActualCoins - m_CurrentCoins)).ToString();
            if (m_CurrentCoins == m_ActualCoins)
            {
                addCoins.gameObject.SetActive(false);
            }
        }
        else if (addCoins.gameObject.activeSelf)
        {
            addCoins.gameObject.SetActive(false);
        }

        if (m_ActualStars != m_CurrentStars)
        {
            m_CurrentStars = m_CurrentStars + (actualAddStarRate * Time.deltaTime);
            m_CurrentStars = m_CurrentStars > m_ActualStars ? m_ActualStars : m_CurrentStars;

            starNumbers.text = ((int)m_CurrentStars).ToString();
            addStars.text = "+" + ((int)(m_ActualStars - m_CurrentStars)).ToString();
            if (m_CurrentStars == m_ActualStars)
            {
                addStars.gameObject.SetActive(false);
            }
        }
        else if (addStars.gameObject.activeSelf)
        {
            addStars.gameObject.SetActive(false);
        }
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
