using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCubeObstacle : Obstacle {

    public float m_MoveSpeed;
    public float m_WaitTimeWhenReached;
    public float m_ImpactImpulse = 10.0f;
    
    // if true, insta kill, if false, can be knock by accelerating
    public bool m_DamageIsCube;

    [HideInInspector]
    public Vector3[] movePos;
    [HideInInspector]
    public int targetIndex;

    bool m_Moving;
    float m_WaitTime;

    void Awake()
    {
        m_Moving = false;
    }

    public override void MoveToCallBack()
    {
        GetComponent<BoxCollider>().enabled = true;
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
            //Player.current.GetComponent<Rigidbody>().constraints = 0;
            if (Player.current.playerState == Player.PlayerState.Playing)
            {
                ObstacleType type = ObstacleType.Cube;
                if (!m_DamageIsCube)
                    type = ObstacleType.Wall;
                Player.current.HitByObstacle(type);
                AudioSystem.current.PlayEvent(AudioSystemEvents.WallEventName);
            }
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * m_ImpactImpulse + transform.up * 1.0f, ForceMode.Impulse);
            rb.AddTorque(new Vector3(2, 2, 2), ForceMode.Impulse);
            rb.useGravity = true;
            GetComponent<BoxCollider>().isTrigger = false;
            //Destroy(gameObject, 1.0f);
            m_Moving = false;
        }
    }
}
