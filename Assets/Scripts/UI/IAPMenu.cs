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

    private void OnEnable() {
        ResetRemovedAdUI();
        // call it twice to override iap button translate
        Invoke("ResetRemovedAdUI", 0.3f);
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
            RemoveAdPriceText.text = LocalizationManager.tr("OWNED");
        }
    }
}
