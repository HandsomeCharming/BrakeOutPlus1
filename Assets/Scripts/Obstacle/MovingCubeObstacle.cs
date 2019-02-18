using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCubeObstacle : Obstacle {

    public float m_MoveSpeed;
    public float m_WaitTimeWhenReached;
    public float m_ImpactImpulse = 10.0f;
    
    // if true, insta kill, if false, can be knock by accelerating
    public bool m_DamageIsCube;
    public Rigidbody rb;

    [HideInInspector]
    public Vector3[] movePos;
    [HideInInspector]
    public int targetIndex;

    bool m_Moving;
    float m_WaitTime;
    const float m_DestroyTime = 2.0f;

    void Awake()
    {
        m_Moving = false;
    }

    public override void MoveToCallBack()
    {
        GetComponent<BoxCollider>().enabled = true;
        rb.useGravity = true;
        m_Moving = true;
        m_WaitTime = 0;
    }

    void Update()
    {
        if(m_Moving)
        {
            //Don't move when waiting
            if(m_WaitTime <= 0)
            {
                Vector3 target = movePos[targetIndex];
                transform.position = Vector3.MoveTowards(transform.position, target, m_MoveSpeed * Time.deltaTime);
                if (transform.position.Equals(target))
                {
                    targetIndex = targetIndex == 0 ? 1 : 0;
                    m_WaitTime = m_WaitTimeWhenReached;
                }
            }
            m_WaitTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !m_Triggered)
        {
            m_Triggered = true;
            if (Player.current.playerState == Player.PlayerState.Playing)
            {
                ObstacleType type = ObstacleType.Cube;
                if (!m_DamageIsCube)
                    type = ObstacleType.Wall;
                Player.current.HitByObstacle(type);
            }
            HitEffect();
            m_Moving = false;
        }
    }

    void HitEffect() {
        transform.GetComponentInChildren<EasyBreakable>().Damage(100);

        CameraEffectManager.Bloom(2.0f, 0.1f);
        CameraFollow.current.StartShaking(0.2f, 0.3f);
        if (Player.current.m_Health >= 0)
            AudioSystem.current.PlayEvent(AudioSystemEvents.WallEventName);

        Destroy(this.gameObject, m_DestroyTime);
    }
}
