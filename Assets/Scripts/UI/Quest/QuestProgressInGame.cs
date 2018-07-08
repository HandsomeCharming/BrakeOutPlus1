using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestProgressInGame : MonoBehaviour {

    public float ShowTime = 3.0f;
    float ShowTimeRemain;

    public Text questText;
    bool fadingOut = false;

    const float fadeOutTime = 0.3f;

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
        else
        {
            questText.text = "";
        }
    }

    void Show()
    {
        fadingOut = false;

        Color color = questText.color;
        color.a = 1.0f;
        questText.color = color;
        //questText.gameObject.SetActive(true);
    }

    void Hide()
    {
        if(!fadingOut)
        {
            fadingOut = true;
            StartCoroutine(FadeOut());
        }
        //questText.gameObject.SetActive(false);
    }

    IEnumerator FadeOut()
    {
        float time = 0;

        while(time < fadeOutTime && fadingOut)
        {
            Color color = questText.color;
            color.a = Mathf.Lerp(1.0f, 0.0f, time / fadeOutTime);
            questText.color = color;
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        Color col = questText.color;
        col.a = 0;
        questText.color = col;
    }
}
