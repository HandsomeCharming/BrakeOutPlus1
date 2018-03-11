using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSuckOnly : Obstacle
{
    public float m_Radius;
    public float m_PullForceMax;
    public float m_PullForceMin;

    // Use this for initialization
    void Start()
    {
        transform.localScale = new Vector3(m_Radius, m_Radius, m_Radius);
    }


    void FixedUpdate()
    {
        float dist = Vector3.Distance(transform.position, Player.current.transform.position);

        if (Player.current != null && Player.current.playerState == Player.PlayerState.Playing && dist < m_Radius)
        {
            float pullForce = Mathf.Lerp(m_PullForceMax, m_PullForceMin, dist / m_Radius);
            Vector3 force = (transform.position - Player.current.transform.position).normalized * pullForce;
            Player.current.physics.ApplyForce(force);
        }
    }
}