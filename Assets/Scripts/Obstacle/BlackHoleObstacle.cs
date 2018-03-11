using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleObstacle : Obstacle
{ 
    public float m_Radius;
    public float m_PullForceMax;
    public float m_PullForceMin;

    public GameObject m_OutRange;
    public GameObject[] m_Sprites;
    public float m_SpriteAnimTime;

    const float m_SpriteSize = 9.0f;
    float[] m_SpriteAnimRemainTimes;
    float m_AnimGap;
    float m_SpriteOriginalScale;
    int m_SpriteCount;

    private void Awake()
    {
        m_SpriteCount = m_Sprites.Length;
        m_SpriteAnimRemainTimes = new float[m_SpriteCount];
        m_AnimGap = 0;
        m_SpriteOriginalScale = m_Radius / m_SpriteSize;
        m_OutRange.transform.localScale = new Vector3(m_SpriteOriginalScale, m_SpriteOriginalScale, m_SpriteOriginalScale);

        for (int i = 0; i < m_SpriteCount; ++i)
        {
            m_Sprites[i].SetActive(false);
            m_SpriteAnimRemainTimes[i] = m_SpriteAnimTime;
        }
    }

    private void Update()
    {
        for(int i=0; i< m_SpriteCount; ++i)
        {
            if(m_AnimGap <= 0 && !m_Sprites[i].activeSelf)
            {
                m_Sprites[i].SetActive(true);
                m_AnimGap = m_SpriteAnimTime / (float)m_SpriteCount;
            }
            
            if(m_Sprites[i].activeSelf)
            {
                float scale = Mathf.Lerp(m_SpriteOriginalScale, 0, 1.0f - m_SpriteAnimRemainTimes[i] / m_SpriteAnimTime);
                m_SpriteAnimRemainTimes[i] -= Time.deltaTime;
                if (m_SpriteAnimRemainTimes[i] <= 0)
                    m_SpriteAnimRemainTimes[i] = m_SpriteAnimTime;
                m_Sprites[i].transform.localScale = new Vector3(scale, scale, scale);
            }
        }
        m_AnimGap -= Time.deltaTime;
    }

    void FixedUpdate () {
        float dist = Vector3.Distance(transform.position, Player.current.transform.position);

        if (Player.current != null && Player.current.playerState == Player.PlayerState.Playing && dist < m_Radius)
        {
            float pullForce = Mathf.Lerp(m_PullForceMax, m_PullForceMin, dist / m_Radius);
            Vector3 force = (transform.position - Player.current.transform.position).normalized * pullForce;
            Player.current.physics.ApplyForce(force);
        }
	}
}
