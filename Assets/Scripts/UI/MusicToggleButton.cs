using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicToggleButton : MonoBehaviour {

    public bool m_IsOn;
    public Text text;

    private void OnEnable()
    {
        RefreshText();
    }

    void RefreshText()
    {
        if (!AudioSystem.current.m_PlayAudio)
        {
            text.text = "OFF";
        }
        else
        {
            text.text = "ON";
        }
    }

    public void Toggle()
    {
        m_IsOn = !AudioSystem.current.m_PlayAudio;
        if(AudioSystem.current.m_PlayAudio)
        {
            AudioSystem.current.StopAllSound();
        }
        else
        {
            AudioSystem.current.StartAllSound();
        }
        RefreshText();
    }
}
