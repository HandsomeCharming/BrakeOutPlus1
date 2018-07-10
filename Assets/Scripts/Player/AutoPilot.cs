using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPilot : MonoBehaviour {
    
    public float m_AutoPilotTime;
    public float m_AutoPilotScoreMax;
    public bool m_Start;

    bool m_Piloting;

    Player m_Player;
    PlayerPhysics m_PlayerPhysics;
    Rigidbody m_Rigidbody;

    int m_DestIndex;

    float m_OldMass;

    public float m_TurningSpeed = 2.0f;
    public float m_Speed = 40.0f;
    public float m_ReachThreshold = 5.0f;
    public bool m_DeathEffect = false;
    PilotMode m_PilotMode;

    const float UPY = 0.7f;

    //Decelerate stuff
    float m_DecTime;
    float m_TotalDecTime;
    float m_MinSpeed = 20.0f;

    bool m_PrepareToStop = false;
    bool m_Stopping = false;
    int m_StopIndex = -1;

    public enum PilotMode
    {
        Score,
        Time
    }



    // Use this for initialization
    void Awake () {
        m_Player = GetComponent<Player>();
        m_PlayerPhysics = m_Player.physics;
        m_Rigidbody = m_Player.GetComponent<Rigidbody>();
        m_ReachThreshold = 5.0f;
        m_TurningSpeed = 2.0f;
        AudioSystem.current.PlayEvent(AudioSystemEvents.AutoPilotStartEventName);
        m_PilotMode = PilotMode.Time;

        StartAutoPilot();
    }

    private void OnDisable()
    {
        if (m_DeathEffect)
            CameraEffectManager.current.StopAutoPilotEffect();
        AudioSystem.current.PlayEvent(AudioSystemEvents.AutoPilotStopEventName);
    }

    public void SetPilotModeAndCount(PilotMode mode, float count)
    {
        m_PilotMode = mode;
        if(mode == PilotMode.Time)
        {
            m_AutoPilotTime = count;
        }
        else
        {
            m_AutoPilotScoreMax = count;
        }
    }

    public void SetPilotTime(float time, bool deathEffect = false)
	{
		m_AutoPilotTime = time;
        m_PrepareToStop = false;
        m_DeathEffect = deathEffect;
        if (m_DeathEffect)
            CameraEffectManager.current.StartAutoPilotEffect();
    }

    public void StartAutoPilot()
    {
        m_Piloting = true;
        m_Player.playerState = Player.PlayerState.AutoControl;
        m_Player.vehicle.StartAutoPilot();
        m_PlayerPhysics.SetPhysicsState(PlayerPhysics.PlayerPhysicsState.AutoPilot);
        m_DestIndex = (FloorBuilder.current.collidingIndex + 3) % FloorBuilder.current.floorMeshCount;
        m_PrepareToStop = false;
        InGameUI.Instance.StartPowerup(Powerups.AutoPilot);
        BackgroundMaterial.current.StartAutoPilotIfColor();
    }

    public void AutoPilotTimeUp()
    {
        FloorBuilder.current.AddAutoPilotEnd();
        GameManager.current.AutoPilotSpeedChange(1.0f);

        m_PrepareToStop = true;
    }

    public void EndAutoPilotInTime(float time)
    {
        m_Stopping = true;
        m_DecTime = 0;
        m_TotalDecTime = time;
        AudioSystem.current.PlayEvent(AudioSystemEvents.AutoPilotCloseEventName);
        CameraEffectManager.current.StopAutoPilotEffect();
        Invoke("EndAutoPilot", time);
    }

    public void EndAutoPilot()
    {
        m_Piloting = false;
        m_Player.playerState = Player.PlayerState.Playing;
        m_Player.vehicle.EndAutoPilot();
        m_PlayerPhysics.SetPhysicsState(PlayerPhysics.PlayerPhysicsState.RegularMoving);
		m_Player.GetComponent<Rigidbody> ().velocity = m_Player.transform.forward * m_Speed;
        InGameUI.Instance.EndPowerup(Powerups.AutoPilot);
        BackgroundMaterial.current.EndAutoPilot();
        GameManager.current.AutoPilotSpeedChange(1.0f);
        GameManager.current.m_StartBoosting = false;
        Destroy(this);
    }

    void FixedUpdate()
    {
        if (m_Piloting)
        {
            /*FloorMesh dest = FloorBuilder.current.floorMeshes[m_DestIndex];
            Vector3 destPos = (dest.endPos1 + dest.endPos2) / 2.0f + ( Vector3.up * UPY);
            Vector3 dir = (destPos - transform.position).normalized;
            transform.forward = Vector3.RotateTowards(transform.forward, (destPos - transform.position).normalized, m_TurningSpeed * Time.fixedDeltaTime, 0);

            m_Rigidbody.velocity = dir * m_Speed; //Vector3.MoveTowards(transform.position, destPos, m_Speed * Time.fixedDeltaTime));

            Vector3 rot = transform.eulerAngles;
            rot.x = m_Player.m_TiltAngleX;
            transform.eulerAngles = rot;

            //transform.position = Vector3.MoveTowards(transform.position, destPos, m_Speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, destPos) < m_ReachThreshold)
            {
                if (m_PrepareToStop && m_StopIndex == m_DestIndex)
                    EndAutoPilot();
                m_DestIndex = (m_DestIndex + 1) % FloorBuilder.current.floorMeshCount;
            }

            m_AutoPilotTime -= Time.fixedDeltaTime;
            if (m_AutoPilotTime <= 0 && !m_PrepareToStop)
                AutoPilotTimeUp();*/
        }
    }

    // Update is called once per frame
    void Update () {
        if (m_Piloting)
        {

            FloorMesh dest = FloorBuilder.current.floorMeshes[m_DestIndex];
            Vector3 destPos = (dest.prevPos1 + dest.prevPos2) / 2.0f + ( Vector3.up * UPY);
            Vector3 dir = (destPos - transform.position).normalized;
            transform.forward = Vector3.RotateTowards(transform.forward, (destPos - transform.position).normalized, m_TurningSpeed * Time.deltaTime, 0);
            
            Vector3 rot = transform.eulerAngles;
            rot.x = m_Player.m_TiltAngleX;
            transform.eulerAngles = rot;

            transform.position = Vector3.MoveTowards(transform.position, destPos, m_Speed * Time.deltaTime);
            //m_Rigidbody.MovePosition(Vector3.MoveTowards(transform.position, destPos, m_Speed * Time.deltaTime));
            //print(Vector3.Distance(transform.position, destPos));

            //transform.position = Vector3.MoveTowards(transform.position, destPos, m_Speed * Time.deltaTime);

            //End
            if (Vector3.Distance(transform.position, destPos) < m_ReachThreshold)
            {
                //if (m_PrepareToStop && m_StopIndex == m_DestIndex)
                //    EndAutoPilot();
                m_DestIndex = (m_DestIndex + 1) % FloorBuilder.current.floorMeshCount;
            }

            m_AutoPilotTime -= Time.deltaTime;
            if (m_PilotMode == PilotMode.Time && m_AutoPilotTime <= 0 && !m_PrepareToStop)
                AutoPilotTimeUp();
            else if(m_PilotMode == PilotMode.Score && !m_PrepareToStop && GameManager.current.gameScore > m_AutoPilotScoreMax )
            {
                AutoPilotTimeUp();
            }
        }
    }
}
