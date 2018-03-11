using UnityEngine;
using System.Collections;

public enum CameraTrackWay
{
    trackHead,
    trackVelocity,
    trackPosition,
	SmoothFollow
}

public class CameraRotate : MonoBehaviour {

    public VehicleSuper follow;

    public float rotateSpeed;

    public float cameraAngleY;

    public CameraTrackWay cameraTrackWay;

    void Start () {
	
	}

	void Update () {
        if (Player.current != null && Player.current.playerState != Player.PlayerState.Dead)
        {
            if(CameraTrackWay.trackHead == cameraTrackWay)
            {
                transform.rotation =Quaternion.Euler(30.0f, Player.current.transform.rotation.eulerAngles.y, 0);
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(30.0f, Player.current.transform.rotation.eulerAngles.y, 0), rotateSpeed*Time.deltaTime);
            }
            else if (CameraTrackWay.trackVelocity == cameraTrackWay)
            {
                Quaternion target = Quaternion.Euler(30.0f, Player.current.transform.rotation.eulerAngles.y, 0);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotateSpeed * Time.deltaTime);
                transform.position = Player.current.transform.position - transform.forward * 30.0f;
                //Quaternion.RotateTowards(transform.rotation, target, rotateSpeed * Time.deltaTime);
                //Quaternion.Euler(90.0f, angle, 0);
                //transform.rotation = target; //Quaternion.RotateTowards(transform.rotation, target, rotateSpeed * Time.deltaTime);
                //Vector3 velocityDir = Player.current.GetComponent<Rigidbody>().velocity.normalized;
                //float angle = Vector3.Angle(Vector3.forward, velocityDir);
                //if (velocityDir.x < 0) angle = -angle;
                // float tiltScaler = 1.0f;
                // float tiltAngle = follow.tiltAngle > 180.0f ? 360 - (360 - follow.tiltAngle) / tiltScaler : follow.tiltAngle / tiltScaler;
                // Quaternion target = Quaternion.Euler(30.0f, Player.current.transform.rotation.eulerAngles.y - tiltAngle, 0);//Quaternion.Euler(90.0f, angle, 0);
                //transform.rotation = target; //Quaternion.RotateTowards(transform.rotation, target, rotateSpeed * Time.deltaTime);
                //transform.Rotate(0, follow.transform.eulerAngles.z, 0);
            } else
            {

            }
        }
    }

    public void RotateLeft()
    {
        //transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }

    public void RotateRight()
    {
        //transform.Rotate(Vector3.forward, -rotateSpeed * Time.deltaTime);
    }
}
