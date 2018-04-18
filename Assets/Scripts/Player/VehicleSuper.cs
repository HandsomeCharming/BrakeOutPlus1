using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VehicleState
{
    Normal,
    AutoPilot,
    Gliding
}

public class VehicleSuper : MonoBehaviour {

    Player m_Player;
    public float tiltAngle;
    public VehicleState m_State;
    public bool m_AutoPiloting;
    public bool m_Gliding;
    public bool m_Boosting;

    public GameObject m_AutoPilotAndBoostTrails;

    public virtual void Awake()
    {
        if (m_AutoPilotAndBoostTrails == null)
        {
            Transform boosty = transform.Find("Boost");
            if (boosty != null)
            {
                m_AutoPilotAndBoostTrails = boosty.gameObject;
            }
        }
    }

    public virtual void OnRotateLeft() { }

    public virtual void OnRotateRight() { }

    public virtual void OnHoldBoth()
    {
    }

    public virtual void StartAutoPilot()
    {
        m_State = VehicleState.AutoPilot;
    }
    public virtual void EndAutoPilot()
    {
        m_State = VehicleState.Normal;
    }

    public virtual void OnAutoPiloting()
    {

    }

    public virtual void StartGliding()
    {
        m_State = VehicleState.Gliding;
    }

    public virtual void EndGliding()
    {
        m_State = VehicleState.Normal;
    }
    public virtual void OnGliding()
    {

    }

    public virtual void Update()
    {
        if(Player.current != null && Player.current.physics.playerPhysicsState == PlayerPhysics.PlayerPhysicsState.Accelerating)
        {
            m_Boosting = true;
        }
        else
        {
            m_Boosting = false;
        }
        if(m_Boosting)
        {
            if (m_AutoPilotAndBoostTrails != null)
            {
                m_AutoPilotAndBoostTrails.SetActive(true);
            }
        }
        else
        {
            if (m_AutoPilotAndBoostTrails != null)
            {
                m_AutoPilotAndBoostTrails.SetActive(false);
            }
        }

        if(m_State == VehicleState.AutoPilot)
        {
            OnAutoPiloting();
        }
        else if(m_State == VehicleState.Gliding)
        {
            OnGliding();
        }
    }
}
