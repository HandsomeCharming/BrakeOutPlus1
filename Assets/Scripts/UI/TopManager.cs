using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using GameSparks.Core;

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
    public GameObject ChangeNamePanel;

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

        ChangeNamePanel.SetActive(!RecordManager.HasRecord(GlobalKeys.m_RenamedKey));
        //NetworkManager.current.GetLeaderBoardDaily(BoardDailyAction);
        //NetworkManager.current.GetMyRankDaily(GetMyRankDailyAction);

        UnityEngine.Analytics.AnalyticsEvent.ScreenVisit("Top");
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

	string GetShortCode(string shortCode)
	{
		string res = shortCode;
		if (res == "DL") {
			res += ".DAY." + DateTime.UtcNow.ToString ("yyyyMMdd");
		}
		return res;
	}

    public void GetLeaderBoard(int boardIndex)
    {
        if (m_CurrentBoardIndex != boardIndex) InitLeaderBoardRows();
        AppManager.instance.LoginOrRegister();
        m_CurrentBoardIndex = boardIndex;
		string shortCode = GetShortCode(boardName [m_CurrentBoardIndex]);
		Debug.Log (shortCode);

		new GameSparks.Api.Requests.LeaderboardDataRequest().SetLeaderboardShortCode(shortCode).SetEntryCount(10).Send((response) =>
        {
            if (!response.HasErrors)
            {
                //Debug.Log("Found Leaderboard Data...");
                int i = 0;
                foreach (GameSparks.Api.Responses.LeaderboardDataResponse._LeaderboardData entry in response.Data)
                {
                    int rank = (int)entry.Rank;
                    string playerName = entry.UserName;
                    if (playerName == "") playerName = "Driver";
                    string score = entry.JSONData["SCORE"].ToString();
                    //Debug.Log("Rank:" + rank + " Name:" + playerName + " \n Score:" + score);
                    
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
				Debug.Log(response.Errors.JSON);
					Debug.Log(shortCode);

                for (int i=0; i < 10; ++i)
                {
                    m_Rows[i].name.text = "Unavailable";
                    m_Rows[i].rank.text = i.ToString();
                    m_Rows[i].score.text = "..";
                }
            }
        });
        
        m_PlayerRow.rank.text = "..";
        m_PlayerRow.score.text = "Loading";
        m_PlayerRow.name.text = AppManager.instance.GetUserName();

        List<string> lbs = new List<string>();
        lbs.Add(boardName[m_CurrentBoardIndex]);

        print(GameSparks.Core.GS.Authenticated);

        new GameSparks.Api.Requests.AccountDetailsRequest()
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    string PlayerID = response.UserId;
                    new GameSparks.Api.Requests.GetLeaderboardEntriesRequest().SetPlayer(PlayerID).SetLeaderboards(lbs).Send((response2) =>
                    {
                        //print(response.ScriptData);
                        //int score = (int)response2.BaseData.GetNumber("SCORE");
                        if (!response2.HasErrors)
                        {
                            foreach (var jj in response2.JSONData)
                            {
                                //print(jj.Key);
                                if (jj.Key.Contains(boardName[m_CurrentBoardIndex]))
                                {
                                    print(jj.Key);
                                    //print((jj.Value.GetType()));
                                    GSData data = (GSData)jj.Value;
                                    //print(data.JSON);
                                    //print(data.GetNumber("SCORE"));
                                    //print(data.GetNumber("rank"));
                                    int score = (int)data.GetNumber("SCORE");
                                    int rank = (int)data.GetNumber("rank");
                                    m_PlayerRow.rank.text = rank.ToString();
                                    m_PlayerRow.score.text = score.ToString();
                                    m_PlayerRow.name.text = data.GetString("userName");
                                }
                            }
                        }
                        else
                        {
                            Debug.Log(response2.Errors.JSON);
                        }
                        //print(score);
                        //print(response2.BaseData.JSON);
                    });
                }
                else
                {
                    Debug.Log(response.Errors.JSON);
                }

            });

        /*new GameSparks.Api.Requests.GetLeaderboardEntriesRequest().SetLeaderboards(lbs).Send((response) =>
        {
            if (!response.HasErrors)
            {
                Debug.Log("Found my Leaderboard Data...");
                print(response.ScriptData);
                if(response.ScriptData != null)
                {
                    //m_PlayerRow.rank.text = response.ScriptData.
                    //m_PlayerRow.score.text = rank["top_score"] as string;
                }
                else
                {
                    m_PlayerRow.rank.text = "..";
                    m_PlayerRow.score.text = "Not here";
                    m_PlayerRow.name.text = AppManager.instance.GetUserName();
                }
            }
            else
            {
                Debug.Log("Error Retrieving my leaderboard Data...");

                m_PlayerRow.rank.text = "..";
                m_PlayerRow.score.text = "Not here";
                m_PlayerRow.name.text = AppManager.instance.GetUserName();
            }
        });*/
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
