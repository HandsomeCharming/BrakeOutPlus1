using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

}

public class AppManager : MonoBehaviour {

    public static AppManager instance;
    public int m_Coins;

	void Awake () {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(transform.gameObject);

        // Init localization
        if(GetComponent<LocalizationManager>() == null)
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
