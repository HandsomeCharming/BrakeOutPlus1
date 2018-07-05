using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPanelUI : MonoBehaviour {

    public RecordsUI m_RecordsUI;
    public QuestSlotUI[] m_SlotUIs;
    public GameObject m_NoQuest;

    public void TryFinishLevelQuest()
    {
        QuestManager.current.TryFinishLevelQuest();
        RefreshUI();
    }

	// Use this for initialization
	void OnEnable () {
        RefreshUI();
    }

    void RefreshUI()
    {
        bool hasQuest = false;
        int index = 0;
        QuestSave questSave = QuestManager.current.m_QuestData;
        
        foreach (var slot in m_SlotUIs)
        {
            slot.gameObject.SetActive(true);
        }

        if (questSave.levelQuest != null)
        {
            m_SlotUIs[index].UpdateUIByQuest(QuestManager.current.m_QuestData.levelQuest, true);
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
            m_SlotUIs[index].HideContents();
            index++;
        }

        // to-do: add no quest thing
        if(questSave.m_Quests.Count == 0)
        {
            UpdateUINoQuest();
        }

        m_RecordsUI.RefreshUI();
    }

    void UpdateUINoQuest()
    {
        foreach(var slot in m_SlotUIs)
        {
            slot.gameObject.SetActive(false);
        }

        m_NoQuest.SetActive(true);
    }
}
