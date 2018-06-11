using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class LootBoxButton : MonoBehaviour {

    public AnimationCurve m_ZRotationCurve;
    public float m_ShakeDuration;
    public float m_ShakeMultiplier;

    bool shaking = false;


    const string nextAdTimeKey = "LBAdLastTime";
    const string timeFormat = "yyyyMMddHHmmss";

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(!shaking && CheckCanLoot())
        {
            shaking = true;
            StartCoroutine(Shake());
        }
	}

    IEnumerator Shake()
    {
        yield return new WaitForSeconds(2.0f);
        float time = 0;
        RectTransform rect = GetComponent<RectTransform>();
        while (time < m_ShakeDuration)
        {
            float lerpAmount = Mathf.Lerp(0, 1.0f, time / m_ShakeDuration);
            Vector3 rotEul = rect.localEulerAngles;
            rotEul.z = m_ZRotationCurve.Evaluate(lerpAmount) * m_ShakeMultiplier;
            rect.localEulerAngles = rotEul;
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        rect.localEulerAngles = Vector3.zero;
        shaking = false;
    }

    public bool CheckCanLoot()
    {
        if (PlayerPrefs.HasKey(nextAdTimeKey))
        {
            string lastTimeStr = PlayerPrefs.GetString(nextAdTimeKey);
            DateTime lastAd = DateTime.ParseExact(lastTimeStr, timeFormat, CultureInfo.InvariantCulture);
            DateTime now = DateTime.Now;
            TimeSpan span = now - lastAd;
            if (span.Hours >= 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }
}
