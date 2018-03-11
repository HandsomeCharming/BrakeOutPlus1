using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements; // only compile Ads code on supported platforms
#endif
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour {

    public static AdManager Instance;

    [SerializeField] string gameID = "1640909";
    private BannerView bannerView;
    string m_PlayingVideo;

    void Awake()
    {
		Instance = this;
		#if UNITY_ADS
        Advertisement.Initialize(gameID, true);
		#endif
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);

        bannerView.Show();
    }

    public void ShowBannerAd()
    {
        RequestBanner();
    }

    public void ShowVideoAd(string playingVideo = "")
    {
#if UNITY_EDITOR
            StartCoroutine(WaitForAd ());
#endif
        m_PlayingVideo = playingVideo;
        const string RewardedPlacementId = "rewardedVideo";

#if UNITY_ADS
        if (!Advertisement.IsReady(RewardedPlacementId))
        {
            Debug.Log(string.Format("Ads not ready for placement '{0}'", RewardedPlacementId));
            return;
        }

        var options = new ShowOptions { resultCallback = HandleShowResult };
        Advertisement.Show(RewardedPlacementId, options);
#endif
    }

#if UNITY_ADS
    private void HandleShowResult(ShowResult result)
    {
        print(result);
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                if(m_PlayingVideo == "Revive")
                {
                    GameManager.current.RevivePlayer();
                }
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                if(m_PlayingVideo == "Revive")
                {
                    GameManager.current.SkipRevive();
                }
                break;
        }
    }

#endif

    IEnumerator WaitForAd()
    {
        float currentTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return null;

		#if UNITY_ADS
        while (Advertisement.isShowing)
            yield return null;
		#endif

        Time.timeScale = currentTimeScale;
    }
}
