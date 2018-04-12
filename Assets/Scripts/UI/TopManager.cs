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

    public int m_CurrentBoardIndex;

    bool Inited = false;

    RankUIRow[] m_Rows;
    RankUIRow m_PlayerRow;

    UnityAction<string> BoardDailyAction;
    UnityAction<string> GetMyRankDailyAction;

    string[] boardName = { "HR", "DL", "WL" };

    private void Awake()
    {
        Inited = false;
        m_LastUpdateTime = DateTime.Now;
        BoardDailyAction += GetLeaderBoardDaily;
        GetMyRankDailyAction += GetMyRankDaily;
        m_CurrentBoardIndex = 1;

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
        if(!Inited)
            InitLeaderBoardRows();
        GetLeaderBoard(m_CurrentBoardIndex);
        //NetworkManager.current.GetLeaderBoardDaily(BoardDailyAction);
        //NetworkManager.current.GetMyRankDaily(GetMyRankDailyAction);
    }

    public void InitLeaderBoardRows()
    {
        Inited = true;
        for (int i=0; i < 10; ++i)
        {
            m_Rows[i].name.text = "Loading";
            m_Rows[i].rank.text = "..";
            m_Rows[i].score.text = "..";
        }
    }

    public void GetLeaderBoard(int boardIndex)
    {
        if (m_CurrentBoardIndex != boardIndex) InitLeaderBoardRows();
        m_CurrentBoardIndex = boardIndex;

        new GameSparks.Api.Requests.LeaderboardDataRequest().SetLeaderboardShortCode(boardName[m_CurrentBoardIndex]).SetEntryCount(10).Send((response) =>
        {
            if (!response.HasErrors)
            {
                Debug.Log("Found Leaderboard Data...");
                int i = 0;
                foreach (GameSparks.Api.Responses.LeaderboardDataResponse._LeaderboardData entry in response.Data)
                {
                    int rank = (int)entry.Rank;
                    string playerName = entry.UserName;
                    if (playerName == "") playerName = "Driver";
                    string score = entry.JSONData["SCORE"].ToString();
                    Debug.Log("Rank:" + rank + " Name:" + playerName + " \n Score:" + score);
                    
                    m_Rows[i].name.text = playerName;
                    m_Rows[i].rank.text = rank.ToString();
                    m_Rows[i].score.text = score.ToString();
                    i++;
                }
                for (; i < 10; ++i)
                {
                    m_Rows[i].name.text = "Unavailable";
                    m_Rows[i].rank.text = i.ToString();
                    m_Rows[i].score.text = "0";
                }
            }
            else
            {
                Debug.Log("Error Retrieving Leaderboard Data...");

                for (int i=0; i < 10; ++i)
                {
                    m_Rows[i].name.text = "Unavailable";
                    m_Rows[i].rank.text = i.ToString();
                    m_Rows[i].score.text = "0";
                }
            }
        });
    }

    //Shitty code
    public void GetLeaderBoardWeekly()
    {
        if (m_CurrentBoardIndex != 2) InitLeaderBoardRows();
        m_CurrentBoardIndex = 2;

        new GameSparks.Api.Requests.LeaderboardDataRequest().SetLeaderboardShortCode("WL").SetEntryCount(10).Send((response) =>
        {
            if (!response.HasErrors)
            {
                Debug.Log("Found Leaderboard Data...");
                int i = 0;
                foreach (GameSparks.Api.Responses.LeaderboardDataResponse._LeaderboardData entry in response.Data)
                {
                    int rank = (int)entry.Rank;
                    string playerName = entry.UserName;
                    if (playerName == "") playerName = "Driver";
                    string score = entry.JSONData["SCORE"].ToString();
                    Debug.Log("Rank:" + rank + " Name:" + playerName + " \n Score:" + score);

                    m_Rows[i].name.text = playerName;
                    m_Rows[i].rank.text = rank.ToString();
                    m_Rows[i].score.text = score.ToString();
                    i++;
                }
                for (; i < 10; ++i)
                {
                    m_Rows[i].name.text = "Unavailable";
                    m_Rows[i].rank.text = i.ToString();
                    m_Rows[i].score.text = "0";
                }
            }
            else
            {
                Debug.Log("Error Retrieving Leaderboard Data...");
            }
        });
    }

    //shiitty
    public void GetLeaderBoardHonor()
    {
        if (m_CurrentBoardIndex != 3) InitLeaderBoardRows();
        m_CurrentBoardIndex = 3;

        new GameSparks.Api.Requests.LeaderboardDataRequest().SetLeaderboardShortCode("HR").SetEntryCount(10).Send((response) =>
        {
            if (!response.HasErrors)
            {
                Debug.Log("Found Leaderboard Data...");
                int i = 0;
                foreach (GameSparks.Api.Responses.LeaderboardDataResponse._LeaderboardData entry in response.Data)
                {
                    int rank = (int)entry.Rank;
                    string playerName = entry.UserName;
                    if (playerName == "") playerName = "Driver";
                    string score = entry.JSONData["SCORE"].ToString();
                    Debug.Log("Rank:" + rank + " Name:" + playerName + " \n Score:" + score);

                    m_Rows[i].name.text = playerName;
                    m_Rows[i].rank.text = rank.ToString();
                    m_Rows[i].score.text = score.ToString();
                    i++;
                }
                for (; i < 10; ++i)
                {
                    m_Rows[i].name.text = "Unavailable";
                    m_Rows[i].rank.text = i.ToString();
                    m_Rows[i].score.text = "0";
                }
            }
            else
            {
                Debug.Log("Error Retrieving Leaderboard Data...");
            }
        });
    }

    public void GetLeaderBoardDaily(string text)
    {
        print(text);
        

        /*Dictionary<string, object> outer = MiniJSON.Json.Deserialize(text) as Dictionary<string, object>;

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
        }*/
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
