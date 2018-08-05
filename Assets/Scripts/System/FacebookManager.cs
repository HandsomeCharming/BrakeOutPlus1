using Facebook.Unity;
using GameSparks.Api.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacebookManager : MonoBehaviour {

    public static FacebookManager current;
    public string fbAuthToken = "";
    const string tokenKey = "fbAuthToken";

    void Awake () {
        current = this;
	}
    
    public bool HasFBConnected()
    {
        Load();
        return fbAuthToken != "";
    }

    public void SetFBToken(string token)
    {
        fbAuthToken = token;
        Save();
    }

    public void GameSparksFBLogin(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            fbAuthToken = AccessToken.CurrentAccessToken.TokenString;
            Save();
            new FacebookConnectRequest()
                .SetAccessToken(AccessToken.CurrentAccessToken.TokenString)
                .SetSwitchIfPossible(true)
                .SetSyncDisplayName(true)
                .Send((response) => {
                    if (response.HasErrors)
                    {
                        Debug.Log("Something failed when connecting with Facebook - " + result.Error);
                    }
                    else
                    {
                        Debug.Log("Gamesparks Facebook login successful");
                    }
                });
        }
    }

    public void GameSparksFBLogBackIn()
    {
        Load();
        if(fbAuthToken != "")
        {
            new FacebookConnectRequest()
                .SetAccessToken(fbAuthToken)
                .Send((response) => {
                    if (response.HasErrors)
                    {
                        Debug.Log("Something failed when connecting with Facebook on log back in");
                    }
                    else
                    {
                        Debug.Log("Gamesparks Facebook login successful on log back in");
                    }
                });
        }
        else
        {
            Debug.Log("FB no token");
        }
    }


    void Save()
    {
        PlayerPrefs.SetString(tokenKey, fbAuthToken);
        PlayerPrefs.Save();
    }

    void Load()
    {
        fbAuthToken = PlayerPrefs.GetString(tokenKey);
    }
}
