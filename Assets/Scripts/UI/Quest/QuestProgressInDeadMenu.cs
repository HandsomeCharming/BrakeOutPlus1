using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestProgressInDeadMenu : MonoBehaviour {

    public QuestSlotInDeadMenu[] m_QuestSlots;

    public void ShowQuestProgress()
    {
        int index = 0;
        List<Quest> quests = QuestManager.current.GetQuests();
        bool isLevel = QuestManager.HasLevelQuest();
        foreach(var quest in quests)
        {
            m_QuestSlots[index].gameObject.SetActive(true);
            m_QuestSlots[index].RefreshByQuest(quest, !isLevel);
            ++index;

            if (isLevel)
            {
                isLevel = false;
            }
        }

        while (index < 3)
        {
            m_QuestSlots[index].gameObject.SetActive(false);
            ++index;
        }
    }
}
