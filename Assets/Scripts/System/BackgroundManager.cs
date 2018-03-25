using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackgroundEnum
{
    Color,
    SkyCity
}

public class BackgroundManager : MonoBehaviour {

    public BackgroundEnum m_Background;
    public GameObject m_ColorCanvas;

    public BackgroundData m_Storer;

    public static BackgroundManager current;

    float m_NextObjectTime;

    public static BackgroundEnum GetBackgroundState()
    {
        return current != null ? current.m_Background: BackgroundEnum.Color;
    }

    public void ChangeBackground(BackgroundEnum background)
    {
        if (m_Background == background) return;
        m_Background = background;

        switch (m_Background)
        {
            case BackgroundEnum.Color:
                Camera.main.clearFlags = CameraClearFlags.Depth;
                m_ColorCanvas.SetActive(true);
                FloorBuilder.current.EnableAllFloorMaterials(true);
                FloorBuilder.current.ChangeAllFloorMaterials(m_Storer.floorColorMat);
                break;
            case BackgroundEnum.SkyCity:
                Camera.main.clearFlags = CameraClearFlags.Skybox;
                m_ColorCanvas.SetActive(false);
                FloorBuilder.current.EnableAllFloorMaterials(false);
                //FloorBuilder.current.ChangeAllFloorMaterials(m_Storer.floorSkyMat);
                break;
            default:
                break;
        }
    }

	void Awake () {
        current = this;
        m_NextObjectTime = Random.Range(m_Storer.m_SkyNewObjectTime.min, m_Storer.m_SkyNewObjectTime.max);
    }
	
	void Update () {
		if(GameManager.GetGameState() == GameManager.GameState.Running && m_Background == BackgroundEnum.SkyCity)
        {
            m_NextObjectTime -= Time.deltaTime;
            if(m_NextObjectTime <= 0)
            {
                GameObject go = Instantiate(m_Storer.m_SkyPrefabs[Random.Range(0, m_Storer.m_SkyPrefabs.Count)], 
                    Player.current.transform.position - Player.current.transform.forward * 
                    Random.Range(m_Storer.m_SkyNewObjectDistance.min, m_Storer.m_SkyNewObjectDistance.max), Quaternion.identity);
                if(go.GetComponent<ItemSuper>() != null)
                {
                    go.GetComponent<ItemSuper>().StartAnim();
                }
                Destroy(go, 20.0f);

                m_NextObjectTime = Random.Range(m_Storer.m_SkyNewObjectTime.min, m_Storer.m_SkyNewObjectTime.max);
            }
        }
	}
}
