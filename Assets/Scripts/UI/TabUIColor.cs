using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabUIColor : MonoBehaviour {

    public BackgroundMaterial background;
    public Image[] images;
    public Text[] texts;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(BackgroundManager.current.m_Background == BackgroundEnum.Color)
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

    public void ChangeBackground()
    {
        if(BackgroundManager.current.m_Background == BackgroundEnum.SkyCity)
        {
            Color col = Color.blue;

            foreach (var image in images)
            {
                image.color = col;
            }

            foreach (var text in texts)
            {
                text.color = col;
            }
        }
    }
}
