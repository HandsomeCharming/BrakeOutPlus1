using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class NetworkManager : MonoBehaviour {

    public static NetworkManager current;

    public int m_ID;

    const string PlayerIDPrefStr = "PlayerID";

    const string LoginAction = "create_new_user"; // pass: name
    const string UpdateDailyScoreAction = "update_daily_topscore"; // pass: id, score
    const string RequestDailyBoardAction = "update_leaderboard_daily"; // 
    const string GetUserRankDaily = "get_userrank_daily";
    const string GetLeaderboardDailY = "get_leaderboard_daily";

    const string IDKey = "id";

    void Start()
    {
        current = this; 
        /*m_ID = PlayerPrefs.GetInt(PlayerIDPrefStr);
        print(m_ID);
        if(m_ID == 0)
        {
            StartCoroutine(Login());
        }*/
    }

    IEnumerator Login()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("action=create_new_user&name=aa"));

        //UnityWebRequest www = UnityWebRequest.Post("https://brakeout-192623.appspot.com/", formData);
        WWWForm form = new WWWForm();
        form.AddField("action", LoginAction);
        form.AddField("device_number", SystemInfo.deviceUniqueIdentifier);
        form.AddField("name", "asdf");
        UnityWebRequest www = UnityWebRequest.Post("https://brakeout-192623.appspot.com/", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            DownloadHandler dd = www.downloadHandler;
            print(dd.text);
            string[] js = dd.text.Split('\n');
            print(js[1]);
            //Dictionary<string, object> dict = 
            Dictionary<string, object> idDict = MiniJSON.Json.Deserialize(js[1]) as Dictionary<string, object>; //dict["0"] as Dictionary<string, object>;
            print(idDict[IDKey]);
            if (idDict != null)
            {
                if (idDict.ContainsKey(IDKey) )
                {
                    int id = int.Parse((string)idDict[IDKey], System.Globalization.NumberStyles.Integer);
                    print(id);
                    m_ID = id;
                    PlayerPrefs.SetInt(PlayerIDPrefStr, id);
                    PlayerPrefs.Save();
                }
            }
        }
    }

    public void SendDailyHighScore(int score)
    {
        StartCoroutine(SendDayHighScore(score));
    }

    IEnumerator SendDayHighScore(int score)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

        WWWForm form = new WWWForm();
        form.AddField("action", UpdateDailyScoreAction);
        form.AddField("id", m_ID);
        form.AddField("score", score);
        UnityWebRequest www = UnityWebRequest.Post("https://brakeout-192623.appspot.com/", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            print("Update success");
        }
    }


    public void GetLeaderBoardDaily(UnityAction<string> callback)
    {
        StartCoroutine(GetLeaderBoardDailyIEum(callback));
    }

    IEnumerator GetLeaderBoardDailyIEum(UnityAction<string> callback)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

        WWWForm form = new WWWForm();
        form.AddField("action", GetLeaderboardDailY);
        form.AddField("top", 0);
        form.AddField("amount", 10);
        UnityWebRequest www = UnityWebRequest.Post("https://brakeout-192623.appspot.com/", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            DownloadHandler dd = www.downloadHandler;

            callback.Invoke(dd.text);
        }
    }

    public void GetMyRankDaily(UnityAction<string> callback)
    {
        StartCoroutine(GetMyRankDailyIEum(callback));
    }

    IEnumerator GetMyRankDailyIEum(UnityAction<string> callback)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

        WWWForm form = new WWWForm();
        form.AddField("action", GetUserRankDaily);
        form.AddField("id", m_ID);
        UnityWebRequest www = UnityWebRequest.Post("https://brakeout-192623.appspot.com/", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            callback.Invoke("error");
        }
        else
        {
            DownloadHandler dd = www.downloadHandler;
            print(dd.text);

            callback.Invoke(dd.text);
        }
    }
}
