using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystemEvents
{
    public const string StartAudioName = "startaudio";
    public const string CrashEventName = "Crash";
    public const string CoinEventName = "yellowcoin";
    public const string FallEventName = "Fall";
    public const string MagnetStartEventName = "magnetstart";
    public const string MagnetStopEventName = "magnetstop";
    public const string SlowTimeStartEventName = "slowtimestart";
    public const string SlowTimeStopEventName = "slowtimestop";
    public const string AutoPilotStartEventName = "autopilotstart";
    public const string AutoPilotStopEventName = "autopilotstop";
    public const string AutoPilotCloseEventName = "autopilotclose";
    public const string WallEventName = "wall";
    public const string ShieldEventName = "shield";
    public const string GlideStartEventName = "glidestart";
    public const string GlideStopEventName = "glidestop";
}

public class AudioSystem : MonoBehaviour {

    public static AudioSystem current;
    public AkAmbient BGMEvent;

    public bool m_PlayAudio;

    const string BGMPrefabName = "Prefabs/Audio/BGM";
    const string SFXPrefabName = "Prefabs/Audio/SFX";
    const string BankName = "Brakeout_Soundbank";

    const string PlayAudioPrefName = "PlayAudio";

    GameObject bgm;
    GameObject sfx;
    bool bankInited = false;
	// Use this for initialization
	void Awake () {
        current = this;
        m_PlayAudio = PlayerPrefs.GetInt(PlayAudioPrefName, 1) == 1? true : false;

        print("Audio awake");

		if(GetComponent<AkInitializer>() == null)
        {
            gameObject.AddComponent<AkInitializer>();
        }
        if (GetComponent<AkTerminator>() == null)
        {
            gameObject.AddComponent<AkTerminator>();
        }

        if(!bankInited)
        {
            AkBankManager.LoadBank(BankName, true, true);
            bankInited = true;
        }

        if (Camera.main.GetComponent<AkAudioListener>() == null)
        {
            //print("Add listener");
            AkSoundEngine.RegisterGameObj(Camera.main.gameObject);
            Camera.main.gameObject.AddComponent<AkAudioListener>();
        }

        if (bgm == null)
        {
            bgm = Instantiate((GameObject)Resources.Load(BGMPrefabName));
            DontDestroyOnLoad(bgm);
            if(m_PlayAudio)
            {
                StartInitialSound();
            }
        }

        if (sfx == null)
        {
            sfx = Instantiate((GameObject)Resources.Load(SFXPrefabName));
            DontDestroyOnLoad(sfx);
        }

        Invoke("LaterInit", 0.5f);

    }

    public void StartInitialSound()
    {
        AkSoundEngine.PostEvent(AudioSystemEvents.StartAudioName, bgm);
    }


    void OnLevelWasLoaded()
    {
        if (GetComponent<AkInitializer>() != null)
        {
            if (Camera.main.GetComponent<AkAudioListener>() == null)
            {
                print("Add listener");
                AkSoundEngine.RegisterGameObj(Camera.main.gameObject);
                Camera.main.gameObject.AddComponent<AkAudioListener>();
            }
        }
    }

    public void SetScore(float score)
    {
        if (m_PlayAudio)
        {
            AkSoundEngine.SetRTPCValue("SCORE", score, bgm);
        }
    }

    //0 - 100
    public void SetSpeed(float speed)
    {
        if (m_PlayAudio)
        {
            AkSoundEngine.SetRTPCValue("SPEED", speed, bgm);
        }
    }

    public void PlayCrash()
    {
        if (m_PlayAudio)
        {
            AkSoundEngine.PostEvent(AudioSystemEvents.CrashEventName, sfx);
        }
    }

    public void PlayCoin()
    {
        if (m_PlayAudio)
        {
            AkSoundEngine.PostEvent(AudioSystemEvents.CoinEventName, sfx);
        }
    }

    public void PlayEvent(string eventName)
    {
        if(m_PlayAudio)
        {
            AkSoundEngine.PostEvent(eventName, bgm);
        }
    }

    void LaterInit()
    {

    }
    
    public void StopAllSound()
    {
        m_PlayAudio = false;
        AkSoundEngine.StopAll();

        PlayerPrefs.SetInt(PlayAudioPrefName, 0);
        PlayerPrefs.Save();
    }
    
    public void StartAllSound()
    {
        m_PlayAudio = true;

        StartInitialSound();

        PlayerPrefs.SetInt(PlayAudioPrefName, 1);
        PlayerPrefs.Save();
    }
   
}
