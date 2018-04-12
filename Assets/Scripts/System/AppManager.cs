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

    public void SaveName(string name)
    {
        PlayerPrefs.SetString(UsernamePref, name);
        PlayerPrefs.Save();
    }

    public void RegisterGameSpark(string name)
    {
        new GameSparks.Api.Requests.DeviceAuthenticationRequest().SetDisplayName(name).Send((response) =>
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
            DailyLoginGS();
        }
    }

    public void DailyLoginGS()
    {
        if (!GameSparks.Core.GS.Authenticated && HasName())
        {
            string name = GetUserName();
            if (name == null) name = "Driver";
            new GameSparks.Api.Requests.DeviceAuthenticationRequest().Send((response) =>
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

    public void UpdateDailyLeaderboardScore(int score)
    {
        if (!GameSparks.Core.GS.Authenticated)
        {
            DailyLoginGS();
        }
        if (GameSparks.Core.GS.Authenticated)
        {
            new GameSparks.Api.Requests.LogEventRequest().SetEventKey("DLUpdate").SetEventAttribute("SCORE", score).Send((response) =>
            {
                if (!response.HasErrors)
                {
                    Debug.Log("Score Posted Successfully...");
                }
                else
                {
                    Debug.Log("Error Posting Score...");
                }
            });
        }
        
    }

    // Update is called once per frame
    void Update () {
		
	}
}
