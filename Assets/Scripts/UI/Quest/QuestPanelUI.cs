using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPanelUI : MonoBehaviour {

    public RecordsUI m_RecordsUI;
    public QuestSlotUI[] m_SlotUIs;

	// Use this for initialization
	void OnEnable () {
        RefreshUI();
    }

    void RefreshUI()
    {
        bool hasQuest = false;
        int index = 0;
        QuestSave questSave = QuestManager.current.m_QuestData;
        if (questSave.levelQuest != null)
        {
            m_SlotUIs[index].UpdateUIByQuest(QuestManager.current.m_QuestData.levelQuest);
            hasQuest = true;
            index++;
        }

        if (questSave.m_Quests.Count > 0)
        {
            foreach(var quest in QuestManager.current.m_QuestData.m_Quests)
            {
                m_SlotUIs[index].UpdateUIByQuest(quest);
                hasQuest = true;
                index++;
            }
        }
        while(index < 3)
        {
            m_SlotUIs[index].UpdateUINoQuest();
            index++;
        }

        // to-do: add no quest thing
    }
}
