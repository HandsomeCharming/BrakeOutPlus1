using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public static CameraFollow current;

    public GameObject follow;
    public float cameraY;

    public CameraTrackWay cameraTrackWay;

    public Rigidbody carRigidBody;
    public Vector3 trackVelocityOffset;
    public float trackVelocityLookOffset;
    public float positionThreshold = 1f;
    public float rotationThreshold = 1f;
    public float cameraStickiness = 10.0f;
	public float cameraRotationSpeed = 5.0f;

	[SerializeField]
	private float distance = 10.0f;
    [SerializeField]
    private float boostDistance = 13.0f;
    // the height we want the camera to be above the target
    [SerializeField]
	private float height = 5.0f;

	[SerializeField]
	private float rotationDamping;
	[SerializeField]
	private float heightDamping;
	[SerializeField]
	private float forwardDist;

    public bool zoom;
    public float zoomVelocityLowThreshold;
    public float zoomOutWeight;

    float boostTime;

    bool shaking = false;
    float m_ShakeTime;
    float m_ShakeAmount;

	void Start () {
        current = this;
    }
	
	void LateUpdate () {
        if(follow == null && GameManager.current != null && GameManager.current.player != null)
        {
            follow = Player.current.gameObject;
            carRigidBody = follow.GetComponent<Rigidbody>();
        }
	    if(follow != null && Player.current && Player.current.playerState != Player.PlayerState.Dead)
        {
            CameraTrackWay trackWay = cameraTrackWay;//GetComponent<CameraRotate>().cameraTrackWay;
            if (trackWay == CameraTrackWay.trackHead)
            {
                Vector3 offset = follow.transform.localToWorldMatrix * Vector3.back * 15.0f;//(Quaternion.Euler(0, follow.transform.rotation.eulerAngles.y, 0) * Vector3.back * 40.0f);
                Vector3 pos = follow.transform.position + offset;
                if (zoom)
                    pos.y = pos.y + cameraY;//getZoomCameraY(); //cameraY;
                else
                    pos.y = pos.y + cameraY;
                transform.position = pos;
                transform.rotation = Quaternion.Euler(30.0f, Player.current.transform.rotation.eulerAngles.y, 0);
            }
			if (trackWay == CameraTrackWay.trackVelocity) {
				// Moves the camera to match the car's position.
				Vector3 offset = (follow.transform.localToWorldMatrix * trackVelocityOffset);
				transform.position = Vector3.Lerp (transform.position, follow.transform.position + offset, cameraStickiness * Time.deltaTime);

				// If the car isn't moving, default to looking forwards. Prevents camera from freaking out with a zero velocity getting put into a Quaternion.LookRotation
				Vector3 carForward;
				Quaternion look;
				if (carRigidBody.velocity.magnitude < rotationThreshold) {
					carForward = follow.transform.forward;
				} else {
					carForward = follow.transform.forward;//carRigidBody.velocity.normalized;
				}
				Vector3 lookOffset = follow.transform.position + carForward * trackVelocityLookOffset;
				lookOffset -= transform.position;
				look = Quaternion.LookRotation (lookOffset.normalized);

				// Rotate the camera towards the velocity vector.
				look = Quaternion.Slerp (transform.rotation, look, cameraRotationSpeed * Time.deltaTime);
				transform.rotation = look;
			} else if (trackWay == CameraTrackWay.SmoothFollow) {
				Transform target = follow.transform;
				Vector3 foPos = target.position + target.forward * forwardDist;

				// Calculate the current rotation angles
				var wantedRotationAngle = target.eulerAngles.y;
				var wantedHeight = target.position.y + height;

				var currentRotationAngle = transform.eulerAngles.y;
				var currentHeight = transform.position.y;

				// Damp the rotation around the y-axis
				currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

				// Damp the height
				currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

				// Convert the angle into a rotation
				var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

				// Set the position of the camera on the x-z plane to:
				// distance meters behind the target
				transform.position = foPos;//target.position;
                float dist = Mathf.Lerp(distance, boostDistance, Player.current.physics.cameraZoomLerpAmount);
				transform.position -= currentRotation * Vector3.forward * dist;

				// Set the height of the camera
				transform.position = new Vector3(transform.position.x ,currentHeight , transform.position.z);

                if(shaking)
                {
                    Vector3 shakeOffset = Random.insideUnitSphere * m_ShakeAmount;
                    transform.position += shakeOffset;
                    m_ShakeTime -= Time.deltaTime;
                    if (m_ShakeTime < 0)
                        shaking = false;
                }

				// Always look at the target
				transform.LookAt(foPos);
			}
        }
	}

    public void SnapBack()
    {
        Vector3 offset = (transform.localToWorldMatrix * trackVelocityOffset);
        transform.position = follow.transform.position + offset;

        Vector3 carForward;
        Quaternion look;
        if (carRigidBody.velocity.magnitude < rotationThreshold)
        {
            carForward = follow.transform.forward;
        }
        else
        {
            carForward = follow.transform.forward;// carRigidBody.velocity.normalized;
        }
        Vector3 lookOffset = follow.transform.position + carForward * trackVelocityLookOffset;
        lookOffset -= transform.position;
        look = Quaternion.LookRotation(lookOffset.normalized);

        // Rotate the camera towards the velocity vector.
        look = look;
        transform.rotation = look;
    }

    public void StartShaking(float shakeTime, float shakeAmount = 0)
    {
        m_ShakeTime = shakeTime;
        m_ShakeAmount = shakeAmount;
        shaking = true;
    }

    public float getZoomCameraY()
    {
        if (Player.current == null) return 0;
        Vector3 velocity = Player.current.GetComponent<Rigidbody>().velocity;
        Vector3 pos = transform.position;
        print(velocity.magnitude);
        if (velocity.magnitude < zoomVelocityLowThreshold)
        {
            pos.y = cameraY;
        }
        else
        {
            pos.y = cameraY + zoomOutWeight * (velocity.magnitude - zoomVelocityLowThreshold);
        }
        
        return pos.y;
    }

    private void OnEnable()
    {
        EnableMotionBlur(false);
    }

    public void EnableMotionBlur(bool enable)
    {
        Camera.main.GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().profile.motionBlur.enabled = enable;
    }
}
