using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class QuestManager : MonoBehaviour {

    public static QuestManager current;
    
    public QuestSave m_QuestData;

    const string QuestSavePath = "/savedQuest.gd";
    const string LastDailyDateKey = "LastDailyDate";

    public List<Quest> GetQuests()
    {
        List<Quest> quests = new List<Quest>();
        if(m_QuestData.levelQuest != null)
        {
            quests.Add(m_QuestData.levelQuest);
        }
        foreach(var quest in m_QuestData.m_Quests)
        {
            if(quest != null)
                quests.Add(quest);
        }
        return quests;
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

    public void TryFinishLevelQuest()
    {
        Quest quest = m_QuestData.levelQuest;
        if(quest != null && IsQuestFinished(quest))
        {
            GameManager.current.AddCurrency(quest.currency, quest.rewardCount);
            m_QuestData.levelQuest = null;
            m_QuestData.currentLevel++;

            if (ShouldGetLevelQuest())
            {
                m_QuestData.levelQuest = QuestDispenser.GetLevelQuest(m_QuestData.currentLevel);
            }
            SaveQuestData();
        }
    }

    public void TryFinishDailyQuests()
    {
        Queue<Quest> destroyQueue = new Queue<Quest>();
        foreach(var quest in m_QuestData.m_Quests)
        {
            if(quest != null && IsQuestFinished(quest))
            {
                FinishDailyQuest(quest);
                destroyQueue.Enqueue(quest);
            }
        }
        bool shouldSave = destroyQueue.Count > 0;

        while (destroyQueue.Count > 0)
        {
            m_QuestData.m_Quests.Remove(destroyQueue.Dequeue());
        }

        if(shouldSave)
            SaveQuestData();
    }

    public static int GetActiveQuestCount()
    {
        int res = 0;
        if(current != null)
        {
            if (current.m_QuestData.levelQuest != null) res++;
            res += current.m_QuestData.m_Quests.Count;
        }
        return res;
    }

    public static void UpdateQuestsStatic(QuestAction action, int count = 1)
    {
        if(current != null)
        {
            current.UpdateQuests(action, count);
        }
    }

    bool IsQuestFinished(Quest quest)
    {
        return quest.currentCount >= quest.targetCount;
    }

    void FinishDailyQuest(Quest quest)
    {
        GameManager.current.AddCurrency(quest.currency, quest.rewardCount);
    }

    void UpdateQuests(QuestAction action, int count = 1)
    {
        if (m_QuestData.levelQuest != null)
        {
            UpdateQuest(action, count, m_QuestData.levelQuest);
        }

        foreach(var quest in m_QuestData.m_Quests)
        {
            if(quest != null)
            {
                UpdateQuest(action, count, quest);
            }
        }
    }

    void UpdateQuest(QuestAction action, int count, Quest quest)
    {
        if(quest.action == action)
        {
            if(action != QuestAction.ReachScore)
            {
                quest.currentCount += count;
                quest.currentCount = quest.currentCount > quest.targetCount ? quest.targetCount : quest.currentCount;
            }
            else
            {
                if(quest.currentCount < count)
                {
                    quest.currentCount = count;
                }
            }
            QuestProgressInGame.QuestUpdated(quest);
        }
    }

    // Use this for initialization
    void Awake () {
        current = this;
        LoadQuests();
        ListenToQuests();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadQuests()
    {
        LoadQuestData();

        if (ShouldGetLevelQuest())
        {
            m_QuestData.levelQuest = QuestDispenser.GetLevelQuest(m_QuestData.currentLevel);
        }

        if(ShouldGetDailyQuest())
        {
            GetDailyQuest();
        }

        SaveQuestData();
    }

    void ListenToQuests()
    {
        
    }


    void GetDailyQuest()
    {
        m_QuestData.m_Quests.Add(QuestDispenser.GetRandomDailyQuest());
        RecordManager.RecordDate(LastDailyDateKey, DateTime.Today);
    }

    bool ShouldGetDailyQuest()
    {
        return true;
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
