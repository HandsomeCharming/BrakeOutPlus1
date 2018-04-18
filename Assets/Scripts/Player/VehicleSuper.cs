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
    
    public float tiltAngle;
    public VehicleState m_State;
    public bool m_AutoPiloting;
    public bool m_Gliding;
    public bool m_Boosting;

    public GameObject m_AutoPilotAndBoostTrails;
    public TrailComponent m_Trail;

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
        if(m_Trail != null)
        {
            if (m_Boosting || m_State == VehicleState.AutoPilot)
            {
                m_Trail.StartBoost();
            }
            else
            {
                m_Trail.StopBoost();
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
