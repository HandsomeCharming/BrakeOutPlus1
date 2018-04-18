using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneVehicle : VehicleSuper
{
    public float rotateSpeed;
    public float recoverSpeed;
    public float rotateAngle; 

    public GameObject m_AutoPilotTrails;

    bool turning = false;
    bool m_StartAccelerate = true;
    float m_AccelerateTurnDir;
    float m_Throttle = 0;

    Vector3 m_OriginalRot;

    public void Awake()
    {
        m_OriginalRot = transform.localEulerAngles;
        //m_DriveSound = RuntimeManager.CreateInstance("event:/Drive");
        //RuntimeManager.AttachInstanceToGameObject(, transform, GetComponent<Rigidbody>());
        //m_DriveSound.start();
        m_Throttle = 0;
        //m_DriveSound.setParameterValue("Throttle", m_Throttle);
    }

    public override void OnRotateLeft()
    {
        turning = true;
        if (transform.rotation.eulerAngles.z > rotateAngle && transform.rotation.eulerAngles.z < 180) return;
        //print(transform.localEulerAngles);
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    public override void OnRotateRight()
    {
        turning = true;
        if (transform.rotation.eulerAngles.z < 360 - rotateAngle && transform.rotation.eulerAngles.z > 180) return;
        // print(transform.localEulerAngles);
        transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
    }

    public override void OnHoldBoth()
    {
        base.OnHoldBoth();
    }

    public override void OnAutoPiloting()
    {

    }

    public override void Update()
    {
        base.Update();

        /*if(m_DriveSound.isValid())
        {
            m_DriveSound.setParameterValue("Throttle", m_Throttle);
            if(m_Throttle >= 0)
                m_Throttle -= 0.01f;
            //print(m_Throttle);
        }*/

        if (m_State == VehicleState.Normal || m_State == VehicleState.Gliding || m_State == VehicleState.AutoPilot)
        {
            tiltAngle = transform.eulerAngles.z;
            if (turning)
            {
                turning = false;
                return;
            }
            m_StartAccelerate = true;

            // Below are recovering code
            if (transform.rotation.eulerAngles.z > 120.0f && transform.rotation.eulerAngles.z < 180.0f)
            {
                Vector3 rot = transform.rotation.eulerAngles;
                rot.z -= 120.0f;
                transform.eulerAngles = rot;
            }
            if (transform.rotation.eulerAngles.z < 240.0f && transform.rotation.eulerAngles.z > 180.0f)
            {
                Vector3 rot = transform.rotation.eulerAngles;
                rot.z += 120.0f;
                transform.eulerAngles = rot;
            }

            if (transform.rotation.eulerAngles.z <= rotateSpeed * Time.deltaTime || transform.rotation.eulerAngles.z >= 360 - rotateSpeed * Time.deltaTime)
                transform.localEulerAngles = m_OriginalRot; //Rotate(0,0, -transform.rotation.eulerAngles.z);
            else if ((transform.rotation.eulerAngles.z < 180.0f && transform.rotation.eulerAngles.z > 90.0f) || (transform.rotation.eulerAngles.z > 270.0f))
            {
                transform.Rotate(0, 0, recoverSpeed * Time.deltaTime);
            }
            else// if ((transform.rotation.eulerAngles.z < 270.0f && transform.rotation.eulerAngles.z > 180.0f) || (transform.rotation.eulerAngles.z < 90.0f))
            {
                transform.Rotate(0, 0, -recoverSpeed * Time.deltaTime);
            }
        }


    }

    private void OnDisable()
    {
        //m_DriveSound.stop(STOP_MODE.IMMEDIATE);
        //m_DriveSound.release();
    }

    public override void StartAutoPilot()
    {
        base.StartAutoPilot();
        if (m_AutoPilotTrails != null)
        {
            m_AutoPilotTrails.SetActive(true);
        }
    }
    public override void EndAutoPilot()
    {
        base.EndAutoPilot();
        if (m_AutoPilotTrails != null)
        {
            m_AutoPilotTrails.SetActive(false);
        }
    }
}
