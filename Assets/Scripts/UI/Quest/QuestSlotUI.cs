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

    public void UpdateUINoQuest()
    {
        ContentGo.SetActive(false);
    }

    public void UpdateUIByQuest(Quest quest)
    {
        bool isCoin = quest.currency == Currency.Coin;
        bool finished = quest.targetCount == quest.currentCount;
        CoinGo.SetActive(isCoin);
        StarGo.SetActive(!isCoin);
        ProgressGo.SetActive(!finished);
        if(ClaimGo != null)
            ClaimGo.SetActive(finished);

        if (isCoin) m_CoinText.text = quest.rewardCount.ToString();
        else m_StarText.text = quest.rewardCount.ToString();

        m_ActionText.text = GetActionStringByActionAndCount(quest.action, quest.targetCount);
        m_ProgressText.text = quest.currentCount.ToString() + "/" + quest.targetCount.ToString();
    }

    string GetActionStringByActionAndCount(QuestAction action, int count)
    {
        string res = "";
        switch(action)
        {
            case QuestAction.Play:
                res = "Play " + count.ToString() + " Games";
                break;
            case QuestAction.ReachScore:
                res = "Reach " + count.ToString() + " Score";
                break;
            case QuestAction.LeapGap:
                res = "Leap " + count.ToString() + " Gaps";
                break;
            case QuestAction.UpgradeCar:
                res = "Upgrade Car " + count.ToString() + " Times";
                break;
            case QuestAction.UpgradeCarToMax:
                res = "Upgrade Car To Max " + count.ToString() + " Times";
                break;
            case QuestAction.OpenChest:
                res = "Open " + count.ToString() + " Chests";
                break;
            case QuestAction.UpgradeItem:
                res = "Upgrade Item " + count.ToString() + " Times";
                break;
            case QuestAction.Glide:
                res = "Glide " + count.ToString() + " Times";
                break;
            case QuestAction.CrushCube:
                res = "Crush Cube " + count.ToString() + " Times";
                break;
            default:
                break;
        }

        return res;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
