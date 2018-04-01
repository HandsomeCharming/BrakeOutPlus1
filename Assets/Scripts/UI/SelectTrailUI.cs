using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTrailUI : MonoBehaviour {

    TrailCard[] cards;

    private void Awake()
    {
    }

    public void RefreshUI(List<TrailSelectData> data)
    {
        if(cards == null)
            cards = transform.GetComponentsInChildren<TrailCard>();

        for (int i=0; i< data.Count; ++i)
        {
            cards[i].RefreshUI(data[i]);
        }
    }
}
