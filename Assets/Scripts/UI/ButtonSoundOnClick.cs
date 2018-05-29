    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSoundOnClick : MonoBehaviour {

    public enum SoundType
    {
        Button,
        Tab
    }
    public SoundType soundType = SoundType.Button;

	// Use this for initialization
	void Start () {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(PlayUISound);
	}
	
    void PlayUISound()
    {
        string eventName = soundType == SoundType.Button ? "click" : "click2";
        AudioSystem.current.PlayEvent(eventName);
    }
}
