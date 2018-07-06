using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour {

	public static TutorialUI current;

	public GameObject m_TurnTutorial;
    public GameObject m_JumpTutorial;


    const string TurnTutorialShowedKey = "TurnTutorialShowed";
	const string CubeTutorialShowedKey = "CubeTutorialShowed";
    const string JumpTutorialShowedKey = "JTS";

    void Awake()
	{
		current = this;
	}

	public void ShowTurnAndBoostTutorialIfFirstTime()
	{
		if(!RecordManager.HasRecord(TurnTutorialShowedKey))
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

    public void ShowJumpTutorialIfFirstTime()
    {
        //if (!RecordManager.HasRecord(JumpTutorialShowedKey))
        {
            GameManager.current.ShowTutorial(true);
            m_JumpTutorial.SetActive(true);
            RecordManager.Record(JumpTutorialShowedKey);
        }
    }

    public void HideJumpTutorial()
    {
        GameManager.current.ShowTutorial(false);
    }
}
