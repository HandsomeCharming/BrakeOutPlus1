using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordsUI : MonoBehaviour {

    public Text LevelNum;
    public Text QuestFinished;
    public Text Highscore;
    public Text GamesPlayed;
    public Text TimePlayed;

	public void RefreshUI()
    {
        QuestFinished.text = QuestManager.current.FinishedQuests.ToString();
        Highscore.text = GameManager.current.gameHighScore.ToString();
        GamesPlayed.text = QuestManager.current.GamesPlayed.ToString();
        LevelNum.text = (QuestManager.current.m_QuestData.currentLevel+1).ToString();

        float secs = QuestManager.current.PlayedTime;
        TimeSpan t = TimeSpan.FromSeconds(secs);
        TimePlayed.text = string.Format("{0:D2}h:{1:D2}m",
                t.Hours,
                t.Minutes); 
    }
}
