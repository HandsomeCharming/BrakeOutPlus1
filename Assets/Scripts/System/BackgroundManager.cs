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
    float m_NextObjectByRoadTime;

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
                //FloorBuilder.current.EnableAllFloorMaterials(false);
                FloorBuilder.current.EnableAllFloorMaterials(true);
                FloorBuilder.current.ChangeAllFloorMaterials(m_Storer.floorSkyMat);

                int count = Random.Range(2, 4);
                for(int i=0; i < count; ++i)
                {
                    float degree = Random.Range(-60.0f, 60.0f);
                    float distance = Random.Range(m_Storer.m_SkyNewObjectDistance.min, m_Storer.m_SkyNewObjectDistance.max);
                    Vector3 pos = Player.current.transform.position + Quaternion.Euler(0, degree, 0) * Player.current.transform.forward * distance;
                    pos.y -= Random.Range(m_Storer.m_SkyNewObjectHeight.min, m_Storer.m_SkyNewObjectHeight.max);
                    GameObject go = Instantiate(m_Storer.m_SkyPrefabs[Random.Range(0, m_Storer.m_SkyPrefabs.Count)],
                        pos, Quaternion.identity);
                    go.transform.Rotate(0, Random.Range(0, 360.0f), 0);
                    float scale = Random.Range(m_Storer.m_SkyNewObjectScale.min, m_Storer.m_SkyNewObjectScale.max);
                    go.transform.localScale = new Vector3(scale, scale, scale);
                    if (go.GetComponent<BackgroundObject>() != null)
                    {
                        go.GetComponent<BackgroundObject>().Invoke("Fly", 60.0f);
                    }
                    go.AddComponent<BackgroundKeepDistance>();
                }

                break;
            default:
                break;
        }
    }

	void Awake () {
        current = this;
        m_NextObjectTime = Random.Range(m_Storer.m_SkyNewObjectTime.min, m_Storer.m_SkyNewObjectTime.max);
    }

    private void Start()
    {
        GameManager.current.StartLoadCar();
    }

    public SkyByRoadPrefab NextObjectByRoad()
    {
        if(m_NextObjectByRoadTime <= 0)
        {
            m_NextObjectByRoadTime = Random.Range(m_Storer.m_SkyNewObjectByRoadTime.min, m_Storer.m_SkyNewObjectByRoadTime.max);
            return m_Storer.m_SkyByRoadPrefabs[Random.Range(0, m_Storer.m_SkyByRoadPrefabs.Length)];
        }
        return null;
    }

    void Update () {
		if(GameManager.GetGameState() == GameManager.GameState.Running && m_Background == BackgroundEnum.SkyCity)
        {
            m_NextObjectTime -= Time.deltaTime;
            if(m_NextObjectByRoadTime > 0)
                m_NextObjectByRoadTime -= Time.deltaTime;
            if(m_NextObjectTime <= 0)
            {
                float degree = Random.Range(90.0f, 270.0f);
                float distance = Random.Range(m_Storer.m_SkyNewObjectDistance.min, m_Storer.m_SkyNewObjectDistance.max);
                Vector3 pos = Player.current.transform.position + Quaternion.Euler(0, degree, 0) * Player.current.transform.forward * distance;
                pos.y -= Random.Range(m_Storer.m_SkyNewObjectHeight.min, m_Storer.m_SkyNewObjectHeight.max);
                GameObject go = Instantiate(m_Storer.m_SkyPrefabs[Random.Range(0, m_Storer.m_SkyPrefabs.Count)], 
                    pos, Quaternion.identity);
                go.transform.Rotate(0, Random.Range(0, 360.0f), 0);
                float scale = Random.Range(m_Storer.m_SkyNewObjectScale.min, m_Storer.m_SkyNewObjectScale.max);
                go.transform.localScale = new Vector3(scale, scale, scale);
                if (go.GetComponent<BackgroundObject>() != null)
                {
                    go.GetComponent<BackgroundObject>().Invoke("Fly", 60.0f);
                }
                go.AddComponent<BackgroundKeepDistance>();

                m_NextObjectTime = Random.Range(m_Storer.m_SkyNewObjectTime.min, m_Storer.m_SkyNewObjectTime.max);
            }
        }
	}
}
