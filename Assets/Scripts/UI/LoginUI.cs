using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : UIBase
{
    public GameObject m_LoginPage;
    public GameObject m_NamePage;
    public InputField m_Text;
	
    public void PressPlay()
    {
        m_LoginPage.SetActive(false);

        AppManager.instance.RegisterPlayer("Driver");
        GameManager.current.state = GameManager.GameState.Start;
        UIManager.current.ChangeStateByGameState();

        // start naming, old
       // m_NamePage.SetActive(true);
    }

    public void ConfirmName()
    {
        m_NamePage.SetActive(false);
        string text = m_Text.text;
        if (text == "") text = "Driver";
        AppManager.instance.RegisterPlayer(text);
        UIManager.current.ChangeStateByGameState();
    }

    public void LoginFacebook_button()
    {
        Debug.Log("Connecting Facebook With GameSparks...");// first check if FB is ready, and then login //
                                                            // if it's not ready we just init FB and use the login method as the callback for the init method //
        if (!FB.IsInitialized)
        {
            Debug.Log("Initializing Facebook...");
            FB.Init(ConnectGameSparksToGameSparks, null);
        }
        else
        {
            FB.ActivateApp();
            ConnectGameSparksToGameSparks();
        }
    }

    void RegisterWithFacebook(string displayName)
    {
        m_LoginPage.SetActive(false);
        string text = displayName;
        if (text == "") text = "Driver";
        AppManager.instance.RegisterPlayerFB(text);
        GameManager.current.state = GameManager.GameState.Start;
        UIManager.current.ChangeStateByGameState();
    }

    ///<summary>
    ///This method is used as the delegate for FB initialization. It logs in FB
    /// </summary>
    private void ConnectGameSparksToGameSparks()
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
                    new GameSparks.Api.Requests.FacebookConnectRequest()
                             .SetAccessToken(AccessToken.CurrentAccessToken.TokenString)
                             .SetDoNotLinkToCurrentPlayer(false)// we don't want to create a new account so link to the player that is currently logged in
                             .SetSwitchIfPossible(false)//this will switch to the player with this FB account id they already have an account from a separate login
                             .SetSyncDisplayName(true)
                             .Send((fbauth_response) => {
                         if (!fbauth_response.HasErrors)
                         {
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

}
