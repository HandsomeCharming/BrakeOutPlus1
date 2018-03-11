using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RankUIRow
{
    public int rankNum;
    public Dictionary<string, object> row;
    public Text rank;
    public Text name;
    public Text score;

    public void Init(Transform parent)
    {
        rank = parent.Find("Num").GetComponent<Text>();
        name = parent.Find("ID").GetComponent<Text>();
        score = parent.Find("Score").GetComponent<Text>();
    }
}


public class TopManager : MonoBehaviour {

    public DateTime m_LastUpdateTime;
    public Transform m_ListParent;

    RankUIRow[] m_Rows;
    RankUIRow m_PlayerRow;

    UnityAction<string> BoardDailyAction;
    UnityAction<string> GetMyRankDailyAction;

    private void Awake()
    {
        m_LastUpdateTime = DateTime.Now;
        BoardDailyAction += GetLeaderBoardDaily;
        GetMyRankDailyAction += GetMyRankDaily;

        int childCount = m_ListParent.childCount;
        m_Rows = new RankUIRow[childCount];

        for (int i=0; i<childCount; ++i)
        {
            Transform ff = m_ListParent.GetChild(i);
            m_Rows[i] = new RankUIRow();
            m_Rows[i].Init(ff);
        }

        m_PlayerRow = new RankUIRow();
        m_PlayerRow.Init(transform.Find("Player"));
    }

    private void OnEnable()
    {
        NetworkManager.current.GetLeaderBoardDaily(BoardDailyAction);
        NetworkManager.current.GetMyRankDaily(GetMyRankDailyAction);
    }

    public void GetLeaderBoardDaily(string text)
    {
        print(text);

        Dictionary<string, object> outer = MiniJSON.Json.Deserialize(text) as Dictionary<string, object>;

        int i = 0;
        foreach (string key in outer.Keys)
        {
            m_Rows[i].row = outer[key] as Dictionary<string, object>;
            m_Rows[i].rankNum = int.Parse(m_Rows[i].row["rank"] as string);
            i++;
            //int index = int.Parse(row["rank"] as string) - 1;
        }

        Array.Sort(m_Rows, delegate (RankUIRow a, RankUIRow b)  { return a.rankNum.CompareTo(b.rankNum); });

        for(i=0; i<m_Rows.Length; ++i)
        {
            RankUIRow rowUI = m_Rows[i];
            rowUI.rank.text = rowUI.row["rank"] as string;
            rowUI.name.text = rowUI.row["name"] as string;
            rowUI.score.text = rowUI.row["top_score"] as string;
        }
    }

    public void GetMyRankDaily(string text)
    {
        print(text);
        Dictionary<string, object> outer = MiniJSON.Json.Deserialize(text) as Dictionary<string, object>;
        Dictionary<string, object> rank = outer["0"] as Dictionary<string, object>;
        m_PlayerRow.rank.text = rank["rank"] as string;
        m_PlayerRow.score.text = rank["top_score"] as string;
    }
}
