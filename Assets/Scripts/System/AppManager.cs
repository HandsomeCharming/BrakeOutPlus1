using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

}

public class AppManager : MonoBehaviour {

    public static AppManager instance;
    public string m_CarPrefabName = "Prefabs/Car/Trimid";
    public int m_Coins;

	void Awake () {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(transform.gameObject);
        m_CarPrefabName = "Prefabs/Car/Trimid";

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
	
    public void SetCarName(string carName)
    {
        GameObject go = (GameObject) Resources.Load("Prefabs/Car/" + carName);
        if(go != null)
        {
            m_CarPrefabName = "Prefabs/Car/" + carName;
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
