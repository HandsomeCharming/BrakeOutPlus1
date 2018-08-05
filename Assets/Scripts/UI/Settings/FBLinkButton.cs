using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FBLinkButton : MonoBehaviour {

    public Text m_Text;

    private void OnEnable()
    {
        Refresh();
    }


    void Refresh()
    {
        m_Text.text = FacebookManager.current.HasFBConnected() ? LocalizationManager.tr("LINKED") : LocalizationManager.tr("LINK");
    }

    void RegisterWithFacebook(string displayName)
    {
        string text = displayName;
        if (text == "") text = "Driver";
        AppManager.instance.RegisterPlayerFB(text);
        Refresh();
    }

    private void ConnectFacebookToGameSparks()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            Debug.Log("Logging Into Facebook...");
            var perms = new List<string>() { "public_profile", "email", "user_friends" };
            FB.LogInWithReadPermissions(perms, (result) =>
            {
                if (FB.IsLoggedIn)
                {
                    Debug.Log("Logged In, Connecting GS via FB..");
                    FacebookManager.current.SetFBToken(AccessToken.CurrentAccessToken.TokenString);
                    new GameSparks.Api.Requests.FacebookConnectRequest().SetAccessToken(AccessToken.CurrentAccessToken.TokenString)
                             .SetDoNotLinkToCurrentPlayer(false)// we don't want to create a new account so link to the player that is currently logged in
                             .SetSwitchIfPossible(true)//this will switch to the player with this FB account id they already have an account from a separate login
                             .Send((fbauth_response) => {
                                 if (!fbauth_response.HasErrors)
                                 {
                                     print("success");
                                     print(fbauth_response.DisplayName);
                                     RegisterWithFacebook(fbauth_response.DisplayName);
                                 }
                                 else
                                 {
                                     Debug.LogWarning(fbauth_response.Errors.JSON);//if we have errors, print them out
                                 }
                             });
                }
                else
                {
                    Debug.LogWarning("Facebook Login Failed:" + result.Error);
                }
            });// lastly call another method to login, and when logged in we have a callback
        }
        else
        {
            LoginFacebook_button();// if we are still not connected, then try to process again
        }
    }


    public void LoginFacebook_button()
    {
        if (FacebookManager.current.HasFBConnected()) return;
        Debug.Log("Connecting Facebook With GameSparks...");// first check if FB is ready, and then login //
                                                            // if it's not ready we just init FB and use the login method as the callback for the init method //
        if (!FB.IsInitialized)
        {
            Debug.Log("Initializing Facebook...");
            FB.Init(ConnectFacebookToGameSparks, null);
        }
        else
        {
            FB.ActivateApp();
            ConnectFacebookToGameSparks();
        }
    }
}