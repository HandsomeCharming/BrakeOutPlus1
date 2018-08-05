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
    public GameObject NotConnectedPanel;
    public Text NotConnectedText;
    public GameObject RankPanel;

    public int m_CurrentBoardIndex;

    bool Inited = false;

    RankUIRow[] m_Rows;
    RankUIRow m_PlayerRow;

    UnityAction<string> BoardDailyAction;
    UnityAction<string> GetMyRankDailyAction;

    string[] boardName = { "HR", "DL", "DL" };

    private void Awake()
    {
        Inited = false;
        m_LastUpdateTime = DateTime.Now;
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
        m_PlayerRow.Init(transform.Find("Content/Player"));
    }

    private void OnEnable()
    {
        if(!Inited)
            InitLeaderBoardRows();
        GetLeaderBoard(m_CurrentBoardIndex);

        ChangeNamePanel.SetActive(!RecordManager.HasRecord(GlobalKeys.m_RenamedKey));
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

    public void GetFacebookBoard()
    {
        InitLeaderBoardRows();
        AppManager.instance.LoginOrRegister();
        m_CurrentBoardIndex = 2;
        string shortCode = GetShortCode(boardName[m_CurrentBoardIndex]);

        new GameSparks.Api.Requests.SocialLeaderboardDataRequest().SetLeaderboardShortCode(shortCode).SetEntryCount(10).Send((response) =>
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
                    m_Rows[i].name.text = "";
                    m_Rows[i].rank.text = i.ToString();
                    m_Rows[i].score.text = "";
                }
            }
            else
            {
                Debug.Log("Error Retrieving Leaderboard Data...");
                Debug.Log(response.Errors.JSON);
                Debug.Log(shortCode);
                for (int i = 0; i < 10; ++i)
                {
                    m_Rows[i].name.text = "";
                    m_Rows[i].rank.text = i.ToString();
                    m_Rows[i].score.text = "";
                }
                if (response.Errors.JSON.Contains("NOTSOCIAL"))
                {
                    RankPanel.SetActive(false);
                    ShowNotConnectedPanel();
                    return;
                }
            }
        });

        m_PlayerRow.rank.text = "..";
        m_PlayerRow.score.text = LocalizationManager.tr("Loading");
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
                            m_PlayerRow.rank.text = "";
                            m_PlayerRow.score.text = "";
                            Debug.Log(response2.Errors.JSON);
                        }
                    });
                }
                else
                {
                    HidePlayerName();
                    Debug.Log(response.Errors.JSON);
                }

            });
    }

    public void GetLeaderBoard(int boardIndex)
    {
        if (m_CurrentBoardIndex != boardIndex) InitLeaderBoardRows();
        if(boardIndex == 2)
        {
            GetFacebookBoard();
            return;
        }

        // Show rank and hide not connected because this is global rank
        RankPanel.SetActive(true);
        NotConnectedPanel.SetActive(false);

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
                    m_Rows[i].name.text = "";
                    m_Rows[i].rank.text = i.ToString();
                    m_Rows[i].score.text = "";
                }
            }
            else
            {
                Debug.Log("Error Retrieving Leaderboard Data...");
				Debug.Log(response.Errors.JSON);
					Debug.Log(shortCode);

                for (int i=0; i < 10; ++i)
                {
                    m_Rows[i].name.text = "";
                    m_Rows[i].rank.text = i.ToString();
                    m_Rows[i].score.text = "";
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
                        if (!response2.HasErrors)
                        {
                            foreach (var jj in response2.JSONData)
                            {
                                //print(jj.Key);
                                if (jj.Key.Contains(boardName[m_CurrentBoardIndex]))
                                {
                                    print(jj.Key);
                                    GSData data = (GSData)jj.Value;
                                    int score = (int)data.GetNumber("SCORE");
                                    int rank = (int)data.GetNumber("rank");
                                    m_PlayerRow.rank.text = rank.ToString();
                                    m_PlayerRow.score.text = score.ToString();
                                    m_PlayerRow.name.text = data.GetString("userName");
                                }
                                else
                                {
                                    HidePlayerName();
                                }
                            }
                        }
                        else
                        {
                            HidePlayerName();
                            Debug.Log(response2.Errors.JSON);
                        }
                    });
                }
                else
                {
                    HidePlayerName();
                    Debug.Log(response.Errors.JSON);
                }

            });
    }
    
    public void GetMyRankDaily(string text)
    {
        print(text);
        Dictionary<string, object> outer = MiniJSON.Json.Deserialize(text) as Dictionary<string, object>;
        Dictionary<string, object> rank = outer["0"] as Dictionary<string, object>;
        m_PlayerRow.rank.text = rank["rank"] as string;
        m_PlayerRow.score.text = rank["top_score"] as string;
    }

    void HidePlayerName()
    {
        m_PlayerRow.rank.text = "";
        m_PlayerRow.score.text = "";
        m_PlayerRow.name.text = "";
    }

    void ShowNotConnectedPanel()
    {
        NotConnectedPanel.SetActive(true);
        if(FacebookManager.current.HasFBConnected())
        {
            NotConnectedText.text = LocalizationManager.tr("Facebook friends not playing this game yet, invite them!");
        }
        else
        {
            NotConnectedText.text = LocalizationManager.tr("Not connected with Facebook");
        }
    }
}
