using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ConsistantUI : MonoBehaviour {

    public static ConsistantUI current;

    public int addCoinRate;
    public int addStarRate;

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

    public void UpdateNumbers()
    {
        m_ActualCoins = GameManager.current.GetCoinCount();
        m_ActualStars = GameManager.current.gameStars;

        if(m_ActualCoins + 5 < m_CurrentCoins || m_CurrentCoins > m_ActualCoins - 5)
        {
            m_CurrentCoins = m_ActualCoins;
            addCoins.gameObject.SetActive(false);
            coinNumbers.text = ((int)m_CurrentCoins).ToString();
        }
        else
        {
            addCoins.gameObject.SetActive(true);
            addCoins.text = ((int)(m_ActualCoins - m_CurrentCoins)).ToString();
        }

        if(m_ActualStars + 1 < m_CurrentStars)
        {
            m_CurrentStars = m_ActualStars;
            addStars.gameObject.SetActive(false);
            starNumbers.text = ((int)m_ActualStars).ToString();
        }
        else
        {
            addStars.gameObject.SetActive(true);
            addStars.text = ((int)(m_ActualStars - m_CurrentStars)).ToString();
        }
    }

    private void Update()
    {
        if(m_ActualCoins != m_CurrentCoins)
        {
            m_CurrentCoins = m_CurrentCoins + (addCoinRate * Time.deltaTime);
            m_CurrentCoins = m_CurrentCoins > m_ActualCoins ? m_ActualCoins : m_CurrentCoins;

            coinNumbers.text = ((int)m_CurrentCoins).ToString();
            addCoins.text = "+" + ((int)(m_ActualCoins - m_CurrentCoins)).ToString();
            if (m_CurrentCoins == m_ActualCoins)
            {
                addCoins.gameObject.SetActive(false);
            }
        }
        else if(addCoins.gameObject.activeSelf)
        {
            addCoins.gameObject.SetActive(false);
        }

        if(m_ActualStars != m_CurrentStars)
        {
            m_CurrentStars = m_CurrentStars + (addStarRate * Time.deltaTime);
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
