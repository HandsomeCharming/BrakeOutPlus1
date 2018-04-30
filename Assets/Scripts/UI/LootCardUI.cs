using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCardUI : MonoBehaviour {

    // has to be in order of pool
    public LootBoxManager m_Manager;
    public List<LootCardSingle> m_SelectCards;
    public AnimationCurve m_LootShuffleTime;
    public MinMaxDataInt m_Jumps;
    bool animating = false;
    // Use this for initialization

    private void Awake()
    {
        animating = false;
    }

    void Start () {
        //m_SelectCards = new List<LootCardSingle>();
        //m_SelectCards.AddRange(GetComponentsInChildren<LootCardSingle>());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AnimateToLootBox(int prizeIndex)
    {
        animating = true;
        int jumps = Random.Range(m_Jumps.min, m_Jumps.max);
        int[] order = new int[jumps];

        int size = m_SelectCards.Count;
        order[jumps - 1] = 0;
        while (order[jumps - 1] == prizeIndex)
        {
            order[jumps - 1] = Random.Range(0, size);
        }

        for(int i=jumps - 2; i >= 0; --i)
        {
            order[i] = 0;
            while (order[i] == order[i+1])
            {
                order[i] = Random.Range(0, size);
            }
        }

        StartCoroutine(AnimateLootBox(order, prizeIndex));
    }

    IEnumerator AnimateLootBox(int[] order, int prizeIndex)
    {
        float shuffleTime = 0;
        for(int i=0; i < order.Length; ++i)
        { 
            foreach(var card in m_SelectCards)
            {
                card.Selected(false);
            }
            m_SelectCards[order[i]].Selected(true);
            shuffleTime = m_LootShuffleTime.Evaluate(i / (float)order.Length);
            yield return new WaitForSeconds(shuffleTime);
        }

        foreach (var card in m_SelectCards)
        {
            card.Selected(false);
        }
        m_SelectCards[prizeIndex].Selected(true);
        animating = false;
        m_Manager.FinishAnim();
    }
}
