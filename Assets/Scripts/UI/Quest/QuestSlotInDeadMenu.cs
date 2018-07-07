using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlotInDeadMenu : MonoBehaviour {

    public Text questText;
    public Text progress;
    public GameObject rewards;
    public GameObject coinIcon;
    public GameObject starIcon;
    public Text rewardNum;

    public void RefreshByQuest(Quest quest)
    {
        questText.text = QuestSlotUI.GetActionStringByActionAndCount(quest.action, quest.targetCount);

        if (quest.IsFinished())
        {
            progress.gameObject.SetActive(false);
            rewards.SetActive(true);
            bool isCoin = quest.currency == Currency.Coin;
            coinIcon.SetActive(isCoin);
            starIcon.SetActive(!isCoin);
            rewardNum.text = quest.rewardCount.ToString();
        }
        else
        {
            progress.gameObject.SetActive(true);
            progress.text = QuestSlotUI.GetProgressStringByQuest(quest);
            rewards.SetActive(false);
        }
    }
}
