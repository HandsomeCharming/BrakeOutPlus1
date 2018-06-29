using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class QuestManager : MonoBehaviour {

    public static QuestManager current;
    
    public QuestSave m_QuestData;
    Quest[] quests;
    int currentLevel;

    const string QuestSavePath = "/savedQuest.gd";
    const string PlayerLevelKey = "Level";
    const string LastDailyDateKey = "LastDailyDate";

    public Quest[] GetQuests()
    {
        return null;
    }
    
    public void SaveQuestData()
    {
        Debug.Log("save quest");
        BinaryFormatter bf = new BinaryFormatter();

        System.IO.FileInfo filepath = new System.IO.FileInfo(Application.persistentDataPath);
        filepath.Directory.Create();

        string path = Application.persistentDataPath + QuestSavePath;
        FileStream file = File.Create(Application.persistentDataPath + QuestSavePath);

        bf.Serialize(file, m_QuestData);
        file.Close();
    }

    public void LoadQuestData()
    {
        if (File.Exists(Application.persistentDataPath + QuestSavePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            string path = Application.persistentDataPath + QuestSavePath;
            FileStream file = File.Open(path, FileMode.Open);
            try
            {
                m_QuestData = (QuestSave)bf.Deserialize(file);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                m_QuestData = new QuestSave();
            }
            file.Close();
        }
        else
        {
            Debug.Log("Load new");
            m_QuestData = new QuestSave();
            SaveQuestData();
        }
    }

    // Use this for initialization
    void Awake () {
        current = this;
        InitLevel();
        LoadQuests();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitLevel()
    {
        if(!RecordManager.HasRecord(PlayerLevelKey))
        {
            RecordManager.RecordInt(PlayerLevelKey, 0);
        }
        currentLevel = RecordManager.GetRecordInt(PlayerLevelKey);
    }

    void LoadQuests()
    {
        LoadQuestData();

        if (ShouldGetLevelQuest())
        {
            m_QuestData.levelQuest = QuestDispenser.GetLevelQuest(currentLevel);
        }

        if(ShouldGetDailyQuest())
        {
            GetDailyQuest();
        }

        SaveQuestData();
    }

    void GetDailyQuest()
    {
        m_QuestData.m_Quests.Add(QuestDispenser.GetRandomDailyQuest());
        RecordManager.RecordDate(LastDailyDateKey, DateTime.Today);
    }

    bool ShouldGetDailyQuest()
    {
        // TO-DO: Add day check
        if(RecordManager.HasRecordDate(LastDailyDateKey))
        {
            DateTime lastDailyDate = RecordManager.GetRecordDate(LastDailyDateKey);
            return (DateTime.Today.Date != lastDailyDate.Date) && (m_QuestData.m_Quests.Count < 2);
        }
        else
        {
            return (m_QuestData.m_Quests.Count < 2);
        }
    }

    bool ShouldGetLevelQuest()
    {
        return (m_QuestData.levelQuest == null);
    }
}
