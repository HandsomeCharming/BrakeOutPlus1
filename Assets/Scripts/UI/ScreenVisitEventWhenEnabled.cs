using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenVisitEventWhenEnabled : MonoBehaviour {

    public string VisitName;

	// Use this for initialization
	void OnEnable () {
        //UnityEngine.Analytics.AnalyticsEvent.ScreenVisit(VisitName);
    }
}
