using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class LootBoxAdButton : MonoBehaviour {

    public LootBoxManager manager;
    public Text remainTime;
    public Image adBase;


    const string nextAdTimeKey = "LBAdLastTime";
    const string timeFormat = "yyyyMMddHHmmss";
    // Use this for initialization
    void OnEnable () {
        if (!CheckCanLoot())
        {
            SetAdButtonCanLoot(false);
            RefreshTimer();
        }
        else
        {
            SetAdButtonCanLoot(true);
        }
    }

    void SetAdButtonCanLoot(bool can)
    {
        if(can)
        {
            remainTime.gameObject.SetActive(false);
            adBase.color = Color.white;
        }
        else
        {
            remainTime.gameObject.SetActive(true);
            adBase.color = Color.gray;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(!CheckCanLoot())
        {
            RefreshTimer();
        }
        else
        {
            remainTime.gameObject.SetActive(false);
        }

		if (Input.GetKeyDown (KeyCode.K))
		{
			PlayerPrefs.DeleteKey (nextAdTimeKey);
			PlayerPrefs.Save ();
			CheckCanLoot ();
		}
    }

    public void LootIfYouCan()
    {
        if (CheckCanLoot())
        {
			manager.StartLootAd ();
        }
    }

	public void ResetTimerWhenLootSuccess()
	{
		SetLastAdTime();
		RefreshTimer();
        SetAdButtonCanLoot(false);
    }

    void SetLastAdTime()
    {
        DateTime current = DateTime.Now;
        current = current.AddHours(2);
        PlayerPrefs.SetString(nextAdTimeKey, current.ToString(timeFormat));
    }

    void RefreshTimer()
    {
        string nextTimeStr = PlayerPrefs.GetString(nextAdTimeKey);
        DateTime nextAd = DateTime.ParseExact(nextTimeStr, timeFormat, CultureInfo.InvariantCulture);
        DateTime now = DateTime.Now;
        TimeSpan span = nextAd - now;
        remainTime.text = span.Hours.ToString() + ":" + span.Minutes.ToString() + ":" + span.Seconds.ToString();
    }

    bool CheckCanLoot()
    {
        if(PlayerPrefs.HasKey(nextAdTimeKey))
        {
            string lastTimeStr = PlayerPrefs.GetString(nextAdTimeKey);
            DateTime lastAd = DateTime.ParseExact(lastTimeStr, timeFormat, CultureInfo.InvariantCulture);
            DateTime now = DateTime.Now;
            TimeSpan span = now - lastAd;
            if(span.Hours >= 2)
            {
                SetAdButtonCanLoot(true);
                return true;
            }
            else
            {
                SetAdButtonCanLoot(false);
                return false;
            }
        }
        else
        {
            SetAdButtonCanLoot(true);
            return true;
        }
    }
}
