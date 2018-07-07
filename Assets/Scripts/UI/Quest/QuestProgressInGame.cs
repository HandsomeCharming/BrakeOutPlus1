using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestProgressInGame : MonoBehaviour {

    public float ShowTime = 3.0f;
    float ShowTimeRemain;

    public Text questText;

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
        string text = QuestSlotUI.GetActionStringByActionAndCount(quest.action, quest.targetCount) + "   " + QuestSlotUI.GetProgressStringByQuest(quest);
        questText.text = text;
        //progress.text = QuestSlotUI.GetProgressStringByQuest(quest);
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
        List<Quest> quests = QuestManager.current.GetQuestsToShowInGame();
        if (quests.Count > 0)
        {
            UpdateQuest(quests[0]);
        }
    }

    void Show()
    {
        questText.gameObject.SetActive(true);
    }

    void Hide()
    {
        questText.gameObject.SetActive(false);
    }
}
