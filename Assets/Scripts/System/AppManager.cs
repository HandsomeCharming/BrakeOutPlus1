using GameSparks.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

[System.Serializable]
public class PlayerData
{

}

public class AppManager : MonoBehaviour {

    public static AppManager instance;
    public int m_Coins;

    public bool m_HasName;
    public string m_Username;
    public bool m_GSRegistered;

    const string UsernamePref = "Username";
    const string GSRegisteredPref = "GameSparkRegister";

    void Awake () {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(transform.gameObject);
        Social.localUser.Authenticate(success =>
        {
            if (success)
                print("Succeess game cccentre");
            else
                Debug.Log("Failed to authenticate");
        });

        // Init localization
        if (GetComponent<LocalizationManager>() == null)
        {
            gameObject.AddComponent<LocalizationManager>();
        }

        if(GetComponent<AudioSystem>() == null)
        {
            gameObject.AddComponent<AudioSystem>();
        }

        if (GetComponent<NetworkManager>() == null)
        {
            gameObject.AddComponent<NetworkManager>();
        }
		GameSparks.Core.GS.GameSparksAvailable += GsServiceHandler;
		//DailyLoginGS();
    }

	void Start()
	{

	}

    public bool HasName()
    {
        return PlayerPrefs.HasKey(UsernamePref);
    }

    public string GetUserName()
    {
        return PlayerPrefs.GetString(UsernamePref);
    }

    public bool IsGSRegistered()
    {
        return PlayerPrefs.HasKey(GSRegisteredPref);
    }

    public void RegisterPlayer(string name)
    {
        if(!HasName())
            SaveName(name);

        if(!IsGSRegistered())
        {
            RegisterGameSpark(name);
        }
    }

    public void RegisterPlayerFB(string name)
    {
        if (!HasName())
            SaveName(name);

        PlayerPrefs.SetString(GSRegisteredPref, name);
        PlayerPrefs.Save();
    }

    public void SaveName(string name)
    {
        PlayerPrefs.SetString(UsernamePref, name);
        PlayerPrefs.Save();
        if(name != "Driver")
        {
            RecordManager.Record(GlobalKeys.m_RenamedKey);
        }
    }

    public void RegisterGameSpark(string name)
    {
        if(GameSparks.Core.GS.Available)
        {
            new GameSparks.Api.Requests.DeviceAuthenticationRequest().SetDisplayName(name).Send((response) =>
            {
                if (!response.HasErrors)
                {
                    print("Registered");
                    print(response.JSONString);
                    PlayerPrefs.SetString(GSRegisteredPref, name);
                    PlayerPrefs.Save();
                }
                else
                {
                    Debug.Log("Register gamespark failed");
                }
            });
        }
    }

    public void RenameGameSpark(string name)
    {

        new GameSparks.Api.Requests.ChangeUserDetailsRequest().SetDisplayName(name).Send((response) =>
        {
            if (!response.HasErrors)
            {
                print("Registered");
                PlayerPrefs.SetString(GSRegisteredPref, name);
                PlayerPrefs.Save();
            }
            else
            {
                Debug.Log("Register gamespark failed");
            }
        });
    }

    void GsServiceHandler(bool available)
    {
        if (!available)
        {
            Debug.LogError("gs service connection lost");
        }
        else
        {
            //LoginOrRegister();
        }
    }

	public void LoginOrRegister()
	{
		if (!IsGSRegistered ()) {
			RegisterPlayer ("Driver");
		}
		else {
			DailyLoginGS ();
		}
	}

    public void DailyLoginGS()
    {
		if (GameSparks.Core.GS.Available && IsGSRegistered())
        {
            string name = GetUserName();
            if (name == null) name = "Driver";
            if(FacebookManager.current.HasFBConnected())
            {
                FacebookManager.current.GameSparksFBLogBackIn();
            } else
            {
                var request = new GameSparks.Api.Requests.DeviceAuthenticationRequest();
                request.Send((response) =>
                {
                    if (!response.HasErrors)
                    {
                        print("Login");
                    }
                    else
                    {
                        Debug.Log("Register gamespark failed");
                    }
                });
            }
        }
    }

	public void LoginAndPostScore(int score)
	{

		if (GameSparks.Core.GS.Available && !GameSparks.Core.GS.Authenticated)
		{
			string name = GetUserName();
			var request = new GameSparks.Api.Requests.DeviceAuthenticationRequest ();
			if (name == null) {
				name = "Driver";
				request.SetDisplayName (name);
			}
			request.Send((response) =>
				{
					if (!response.HasErrors)
					{
						print("Login");
						SendPostScoreRequest(score);
					}
					else
					{
						Debug.Log("Register gamespark failed");
					}
				});
		}
	}

	void SendPostScoreRequest(int score)
	{
		new GameSparks.Api.Requests.LogEventRequest ().SetEventKey ("DLUpdate").SetEventAttribute ("SCORE", score).Send ((response) => {
			if (!response.HasErrors) {
				Debug.Log ("Score Posted Successfully...");
			} else {
				Debug.Log ("Error Posting Score...");
			}
		});
	}

    public void UpdateDailyLeaderboardScore(int score)
    {
        if (!GameSparks.Core.GS.Authenticated)
        {
			LoginAndPostScore (score);
        }
		else{
			SendPostScoreRequest (score);
		} 
    }

    // Update is called once per frame
    void Update () {
		
	}
}
