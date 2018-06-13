//#define TESTING_AD

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_ADS
using UnityEngine.Advertisements; // only compile Ads code on supported platforms
#endif
using GoogleMobileAds.Api;


public class AdManager : MonoBehaviour {

    public static AdManager Instance;

    RewardType m_RewardType;

    [SerializeField] string gameID = "1640909";
    private BannerView bannerView;
    string m_PlayingVideo;
    private RewardBasedVideoAd rewardBasedVideo;
    private InterstitialAd interstitial;
	private bool InterstitialShowed = false;
    private bool rewardReceived = false;

#if TESTING_AD
    string appId = "ca-app-pub-3372369278999623~1593905393";
    string interstitialID = "ca-app-pub-3940256099942544/4411468910";
    string rewardId = "ca-app-pub-3940256099942544/1712485313";
    string lootboxId = "ca-app-pub-3372369278999623/1544913597";
#else
    string appId = "ca-app-pub-3372369278999623~1593905393";
    string interstitialID = "ca-app-pub-3372369278999623/8232776726";
    string rewardId = "ca-app-pub-3372369278999623/1544913597";
#endif

    enum RewardType
    {
        None,
        Revive,
        LootBox,
        DoubleCollect
    }


    void Awake()
    {
        Instance = this;

        m_RewardType = RewardType.None;

#if UNITY_ANDROID
        string appId = "ca-app-pub-3372369278999623~1593905393";
#elif UNITY_IPHONE
        string appId = "ca-app-pub-3372369278999623~1593905393";
#else
        string appId = "unexpected_platform";
#endif
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        // Get singleton reward based video ad reference.
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;
        rewardBasedVideo.OnAdRewarded += HandleRewardReviveRewarded;
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        this.RequestRewardBasedVideo();
		if (InterstitialNeedsLoading ()) {
			RequestInterstitial ();
		}
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
    
    public void RequestInterstitial()
    {
        if (interstitial != null)
            interstitial.Destroy();
        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(interstitialID);
        interstitial.OnAdClosed += HandleInterstitialClosed;
        interstitial.OnAdFailedToLoad += HandleInterstitialFailToLoad;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
		InterstitialShowed = false;
    }

	public bool InterstitialIsLoaded()
	{
		if (interstitial != null && interstitial.IsLoaded ()) {
			return true;
		}
		return false;
	}

	public bool InterstitialNeedsLoading()
	{
		if (InterstitialShowed || interstitial == null) {
			return true;
		}
		return false;
	}

    public bool ShowInterstitial()
    {
		print ("Show interstitial");
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
			InterstitialShowed = true;
            return true;
        }
        else return false;
    }

    private void HandleInterstitialClosed(object sender, EventArgs args)
    {

    }
    private void HandleInterstitialFailToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        interstitial.Destroy();
        RequestInterstitial();
    }

    public void RequestRewardBasedVideo()
    {
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        this.rewardBasedVideo.LoadAd(request, rewardId);
    }

    public bool ShowRewardReviveVideo()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            rewardReceived = false;
            m_RewardType = RewardType.Revive;
            rewardBasedVideo.Show();
            UnityEngine.Analytics.Analytics.CustomEvent("ShowVideo", new Dictionary<string, object>
            {
                { "id", "Revive"},
            });
            return true;
        }
        return false;
    }

    public bool ShowLootboxVideo()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            rewardReceived = false;
            m_RewardType = RewardType.LootBox;
            rewardBasedVideo.Show();
            UnityEngine.Analytics.Analytics.CustomEvent("ShowVideo", new Dictionary<string, object>
            {
                { "id", "LootBox"},
            });
            return true;
        }
        return false;
    }

    public bool ShowDoubleCollectVideo()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            rewardReceived = false;
            m_RewardType = RewardType.DoubleCollect;
            rewardBasedVideo.Show();
            UnityEngine.Analytics.Analytics.CustomEvent("ShowVideo", new Dictionary<string, object>
            {
                { "id", "Double Collect"},
            });
            return true;
        }
        return false;
    }

    public void HandleRewardReviveRewarded(object sender, Reward args)
    {
        print("Rewarded");
        rewardReceived = true;
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        if(rewardReceived)
        {
            if (m_RewardType == RewardType.Revive)
            {
                GameManager.current.RevivePlayer();
            }
            else if (m_RewardType == RewardType.LootBox)
            {
                LootBoxManager.instance.StartLootAdRewarded();
            }
            else if(m_RewardType == RewardType.DoubleCollect)
            {
                DeadMenuUI.current.ReloadGameAndCollectDouble();
            }
            m_RewardType = RewardType.None;
        }
        else
        {
            if (m_RewardType == RewardType.DoubleCollect)
            {
                DeadMenuUI.current.ReloadGameAndCollect();
            }
            m_RewardType = RewardType.None;
        }
        this.RequestRewardBasedVideo();
    }
}
