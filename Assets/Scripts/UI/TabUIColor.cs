using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabUIColor : MonoBehaviour {

    public BackgroundMaterial background;
    public Image[] images;
    public Text[] texts;

    bool inPlayTab = true;
    Color otherColor;
    Color skyColor;

    // Use this for initialization
    void Awake () {
        inPlayTab = true;
        otherColor = new Color(37.0f/255.0f, 170.0f / 255.0f, 225.0f / 255.0f);
        skyColor = new Color(37.0f / 255.0f, 170.0f / 255.0f, 225.0f / 255.0f);
    }
	
	// Update is called once per frame
	void Update () {
		if(BackgroundManager.current.m_Background == BackgroundEnum.Color && inPlayTab == true)
        {
            Color col = background.GetCurrentDownColor();

            foreach(var image in images)
            {
                image.color = col;
            }

            foreach(var text in texts)
            {
                text.color = col;
            }
        }
	}

    public void SetColorInPlayTab()
    {
        inPlayTab = true;
    }

    public void SetColorInOtherTabs()
    {
        inPlayTab = false;

        foreach (var image in images)
        {
            image.color = otherColor;
        }

        foreach (var text in texts)
        {
            text.color = otherColor;
        }
    }

    public void ChangeBackground()
    {
        if(BackgroundManager.current.m_Background == BackgroundEnum.SkyCity)
        {
            foreach (var image in images)
            {
                image.color = skyColor;
            }

            foreach (var text in texts)
            {
                text.color = skyColor;
            }
        }
    }
}
