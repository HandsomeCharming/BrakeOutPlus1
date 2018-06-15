using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotateWhenPaused : MonoBehaviour {

	bool rot = false;

	void OnEnable()
	{
		rot = true;
		StartCoroutine (Rotate ());
	}

	void OnDisable()
	{
		rot = false;
	}

	IEnumerator Rotate()
	{
		while (rot) {
			yield return new WaitForEndOfFrame ();

		}
	}
}
