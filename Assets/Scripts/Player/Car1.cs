using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car1 : VehicleSuper {
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider backLeft;
    public WheelCollider backRight;

    bool steering = false;


    public override void OnRotateLeft()
    {
        frontLeft.steerAngle = -30;
        frontRight.steerAngle = -30;
        steering = true;
    }

    public override void OnRotateRight()
    {
        frontLeft.steerAngle = 30;
        frontRight.steerAngle = 30;
        steering = true;
    }

    public override void OnHoldBoth() {
        backLeft.motorTorque = 1000.0f;
        backRight.motorTorque = 1000.0f;
    }

    void Update()
    {
        if(steering == true)
        {
            steering = false;
        } else
        {
            frontLeft.steerAngle = 0;
            frontRight.steerAngle = 0;
        }
    }
}
