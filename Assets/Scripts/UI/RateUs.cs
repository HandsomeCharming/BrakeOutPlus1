using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateUs : MonoBehaviour {

	public void OpenRateUsURL()
    {
        Application.OpenURL("itms-apps://itunes.apple.com/app/id1363631518");

		if (!RateUsPanel.IsRated ()) {
			RateUsPanel.SetRated ();
			GameManager.current.AddStar (188);
		}
    }
}
