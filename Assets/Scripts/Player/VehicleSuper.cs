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
