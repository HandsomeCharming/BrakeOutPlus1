using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour {

	public static TutorialUI current;

	public GameObject m_TurnTutorial;

	const string TurnTutorialShowedKey = "TurnTutorialShowed";
	const string CubeTutorialShowedKey = "CubeTutorialShowed";

	void Awake()
	{
		current = this;
	}

	public void ShowTurnAndBoostTutorialIfFirstTime()
	{
		//if(!RecordManager.HasRecord(TurnTutorialShowedKey))
		{
			GameManager.current.ShowTutorial (true);
			m_TurnTutorial.SetActive (true);
			RecordManager.Record (TurnTutorialShowedKey);
		}
	}

	public void FinishTurnAndBoostTutorial()
	{
		GameManager.current.ShowTutorial (false);
	}

	public void FinishShowCube()
	{
		GameManager.current.ShowTutorial (false);
	}

}
