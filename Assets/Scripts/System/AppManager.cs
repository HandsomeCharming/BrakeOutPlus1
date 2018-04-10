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
    public bool m_Registered;

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
    }

	// Update is called once per frame
	void Update () {
		
	}
}
