using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class LootBoxAdButton : MonoBehaviour {

    public LootBoxManager manager;
    public Text remainTime;


    const string nextAdTimeKey = "LBAdLastTime";
    const string timeFormat = "yyyyMMddHHmmss";
    // Use this for initialization
    void OnEnable () {
        if (!CheckCanLoot())
        {
            remainTime.gameObject.SetActive(true);
            RefreshTimer();
        }
        else
        {
            remainTime.gameObject.SetActive(false);
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
	}

    void SetLastAdTime()
    {
        DateTime current = DateTime.Now;
        current = current.AddHours(2);
        print(current.ToString(timeFormat));
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
                remainTime.gameObject.SetActive(false);
                return true;
            }
            else
            {
                remainTime.gameObject.SetActive(true);
                return false;
            }
        }
        else
        {
            remainTime.gameObject.SetActive(false);
            return true;
        }
    }
}
