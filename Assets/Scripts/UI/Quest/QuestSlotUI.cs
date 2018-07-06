using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlotUI : MonoBehaviour {

    public GameObject HighlightedFrame;
    public GameObject StarGo;
    public GameObject CoinGo;
    public GameObject ContentGo;
    public GameObject ProgressGo;
    public GameObject ClaimGo;

    public Text m_CoinText;
    public Text m_StarText;
    public Text m_ActionText;
    public Text m_ProgressText;

    public void HideContents()
    {
        ContentGo.SetActive(false);
    }

    public void UpdateUIByQuest(Quest quest, bool isLevelingQuest = false)
    {
        bool isCoin = quest.currency == Currency.Coin;
        bool finished = quest.targetCount == quest.currentCount;
        HighlightedFrame.SetActive(isLevelingQuest);
        CoinGo.SetActive(isCoin);
        StarGo.SetActive(!isCoin);
        ProgressGo.SetActive(!finished);
        if(ClaimGo != null)
            ClaimGo.SetActive(finished);

        if (isCoin) m_CoinText.text = quest.rewardCount.ToString();
        else m_StarText.text = quest.rewardCount.ToString();

        m_ActionText.text = GetActionStringByActionAndCount(quest.action, quest.targetCount);
        m_ProgressText.text = GetProgressStringByQuest(quest);
    }

    public static string GetProgressStringByQuest(Quest quest)
    {
        string res = "";
        if(quest.action == QuestAction.ReachScore)
        {
            res = quest.currentCount < quest.targetCount ? "0" : "1";
            res += "/1";
        }
        else
        {
            res = quest.currentCount.ToString() + "/" + quest.targetCount.ToString();
        }
        return res;
    }

    public static string GetActionStringByActionAndCount(QuestAction action, int count)
    {
        string res = "";
        switch(action)
        {
            case QuestAction.Play:
                res = "Play " + count.ToString() + " Game";
                break;
            case QuestAction.ReachScore:
                res = "Reach " + count.ToString() + " Score";
                break;
            case QuestAction.LeapGap:
                res = "Leap " + count.ToString() + " Gap";
                break;
            case QuestAction.UpgradeCar:
                res = "Upgrade Car " + count.ToString() + " Time";
                break;
            case QuestAction.UpgradeCarToMax:
                res = "Upgrade Car To Max " + count.ToString() + " Time";
                break;
            case QuestAction.OpenChest:
                res = "Open " + count.ToString() + " Chest";
                break;
            case QuestAction.UpgradeItem:
                res = "Upgrade Item " + count.ToString() + " Time";
                break;
            case QuestAction.Glide:
                res = "Glide " + count.ToString() + " Time";
                break;
            case QuestAction.CrushCube:
                res = "Crush Cube " + count.ToString() + " Time";
                break;
            default:
                break;
        }
        if (count > 1 && action != QuestAction.ReachScore) res += "s";

        return res;
    }

    public void FindChildsEditor()
    {
        HighlightedFrame = transform.Find("Frame/Highlighted").gameObject;
        StarGo = transform.Find("Content/Star").gameObject;
        CoinGo = transform.Find("Content/Coin").gameObject;
        ContentGo = transform.Find("Content").gameObject;
        ProgressGo = transform.Find("Content/Progress").gameObject;
        ClaimGo = transform.Find("Content/Claim").gameObject;

        m_CoinText = transform.Find("Content/Coin/Num").GetComponent<Text>();
        m_StarText = transform.Find("Content/Star/Num").GetComponent<Text>();
        m_ActionText = transform.Find("Content/QuestText").GetComponent<Text>();
        m_ProgressText = transform.Find("Content/Process/Text").GetComponent<Text>();
    }

}
