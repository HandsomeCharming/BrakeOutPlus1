using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IAPMenu : MonoBehaviour {

    public Button RemoveAdButton;
    public Text RemoveAdPriceText;

    private void Awake()
    {
        ResetRemovedAdUI();
    }

    public void GetCoin(int coinCount)
    {
        GameManager.current.AddCoin(coinCount);
    }
    public void GetStar(int starCount)
    {
        GameManager.current.AddStar(starCount);
    }

    public void RemoveAd()
    {
        GameManager.current.RemoveAds();
        ResetRemovedAdUI();
    }

    void ResetRemovedAdUI()
    {
        if (GameManager.current.AdRemoved())
        {
            RemoveAdButton.enabled = false;
            RemoveAdButton.GetComponent<UnityEngine.Purchasing.IAPButton>().enabled = false;
            RemoveAdPriceText.text = "OWNED";
        }
    }
}
