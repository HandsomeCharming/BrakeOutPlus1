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

    const string BGMPrefabName = "Prefabs/Audio/BGM";
    const string SFXPrefabName = "Prefabs/Audio/SFX";
    const string BankName = "Brakeout_Soundbank";


    GameObject bgm;
    GameObject sfx;
    bool bankInited = false;
	// Use this for initialization
	void Awake () {
        current = this;

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
            print("Add listener");
            AkSoundEngine.RegisterGameObj(Camera.main.gameObject);
            Camera.main.gameObject.AddComponent<AkAudioListener>();
        }

        if (bgm == null)
        {
            bgm = Instantiate((GameObject)Resources.Load(BGMPrefabName));
            DontDestroyOnLoad(bgm);
            AkSoundEngine.PostEvent(AudioSystemEvents.StartAudioName, bgm);

        }

        if (sfx == null)
        {
            sfx = Instantiate((GameObject)Resources.Load(SFXPrefabName));
            DontDestroyOnLoad(sfx);
        }

        Invoke("LaterInit", 0.5f);

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
        AkSoundEngine.SetRTPCValue("SCORE", score, bgm);
    }

    //0 - 100
    public void SetSpeed(float speed)
    {
        AkSoundEngine.SetRTPCValue("SPEED", speed, bgm);
    }

    public void PlayCrash()
    {
        AkSoundEngine.PostEvent(AudioSystemEvents.CrashEventName, sfx);
    }

    public void PlayCoin()
    {
        AkSoundEngine.PostEvent(AudioSystemEvents.CoinEventName, sfx);
    }

    public void PlayEvent(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, bgm);
    }

    void LaterInit()
    {

    }
}
