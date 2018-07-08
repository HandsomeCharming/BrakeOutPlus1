using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class QuestManager : MonoBehaviour {

    public static QuestManager current;
    
    public QuestSave m_QuestData;

    public float PlayedTime;
    public int FinishedQuests;
    public int GamesPlayed;

    const string QuestSavePath = "/savedQuest.gd";
    const string LastDailyDateKey = "LastDailyDate";
    const string PlayedTimeKey = "PlayedTime";
    const string FinishedQuestsKey = "FinishedQuests";
    const string GamesPlayedKey = "GamesPlayed";

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

    public List<Quest> GetQuestsToShowInGame()
    {
        List<Quest> quests = new List<Quest>();
        if (m_QuestData.levelQuest != null && ShouldDisplayInGame(m_QuestData.levelQuest))
        {
            quests.Add(m_QuestData.levelQuest);
        }
        foreach (var quest in m_QuestData.m_Quests)
        {
            if (quest != null)
                quests.Add(quest);
        }
        return quests;
    }

    public static bool HasLevelQuest()
    {
        if (current != null)
            return (current.m_QuestData.levelQuest != null);
        return false;
    }

    public static bool ShouldFinishLevelQuestInDeadMenu()
    {
        if(HasLevelQuest())
        {
            Quest quest = current.m_QuestData.levelQuest;
            QuestAction action = quest.action;
            bool res = action == QuestAction.Play || action == QuestAction.ReachScore || action == QuestAction.LeapGap || action == QuestAction.Glide
                || action == QuestAction.CrushCube;
            return res;
        }
        return false;
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
            FinishedQuests++;

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
                FinishedQuests++;
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

    public static void GameFinished()
    {
        if(current != null)
            current.GamesPlayed++;
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

    public static bool ShouldDisplayInGame(Quest quest)
    {
        QuestAction action = quest.action;
        bool res = action == QuestAction.Play || action == QuestAction.ReachScore || action == QuestAction.LeapGap || action == QuestAction.Glide
            || action == QuestAction.CrushCube;
        return res;
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
                if(count >= quest.targetCount)
                {
                    quest.currentCount = quest.targetCount;
                }
            }
            QuestProgressInGame.QuestUpdated(quest);
        }
    }

    // Use this for initialization
    void Awake () {
        current = this;
        LoadQuests();
    }

	void Update () {
        PlayedTime += Time.deltaTime;
	}

    private void OnDisable()
    {
        RecordManager.RecordFloat(PlayedTimeKey, PlayedTime);
        RecordManager.RecordInt(GamesPlayedKey, GamesPlayed);
        RecordManager.RecordInt(FinishedQuestsKey, FinishedQuests);
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

    void GetDailyQuest()
    {
        m_QuestData.m_Quests.Add(QuestDispenser.GetRandomDailyQuest());
        RecordManager.RecordDate(LastDailyDateKey, DateTime.Today);
    }

    bool ShouldGetDailyQuest()
    {
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

    void LoadRecords()
    {
        PlayedTime = PlayerPrefs.GetFloat(PlayedTimeKey, 0);
        GamesPlayed = PlayerPrefs.GetInt(GamesPlayedKey, 0);
        FinishedQuests = PlayerPrefs.GetInt(FinishedQuestsKey, 0);
    }
}
