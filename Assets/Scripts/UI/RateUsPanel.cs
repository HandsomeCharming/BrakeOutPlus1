using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RateUsPanel : MonoBehaviour {

	const string Rated = "Rated";

	public void Show()
	{
		gameObject.SetActive (true);
	}

	public void Hide()
	{
		gameObject.SetActive (false);
	}

	public void RateUs()
	{
		Application.OpenURL("itms-apps://itunes.apple.com/app/id1363631518");

		SetRated ();

		GameManager.current.AddStar (188);

		Hide ();
	}

	public static void SetRated()
	{
		PlayerPrefs.SetInt (Rated, 1);
		PlayerPrefs.Save ();
	}

	public static bool IsRated()
	{
		return PlayerPrefs.HasKey (Rated);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
