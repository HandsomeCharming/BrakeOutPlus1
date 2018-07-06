using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestProgressInGame : MonoBehaviour {

    public float ShowTime = 3.0f;
    float ShowTimeRemain;

    public Text questText;
    public Text progress;

    public static QuestProgressInGame current;

    public static void QuestUpdated(Quest quest)
    {
        if(current)
        {
            current.UpdateQuest(quest);
        }
    }

    private void Awake()
    {
        current = this;    
    }

    private void OnEnable()
    {
        ShowTimeRemain = ShowTime;
        ShowFirstQuest();
    }

    void UpdateQuest(Quest quest)
    {
        ShowTimeRemain = ShowTime;
        Show();
        questText.text = QuestSlotUI.GetActionStringByActionAndCount(quest.action, quest.targetCount);
        progress.text = QuestSlotUI.GetProgressStringByQuest(quest);
    }

    void Update () {
        ShowTimeRemain -= Time.deltaTime;
        
        if(ShowTimeRemain <= 0.0f)
        {
            Hide();
        }
    }

    void ShowFirstQuest()
    {
        List<Quest> quests = QuestManager.current.GetQuests();
        if (quests.Count > 0)
        {
            UpdateQuest(quests[0]);
        }
    }

    void Show()
    {
        questText.gameObject.SetActive(true);
        progress.gameObject.SetActive(true);
    }

    void Hide()
    {
        questText.gameObject.SetActive(false);
        progress.gameObject.SetActive(false);
    }
}
