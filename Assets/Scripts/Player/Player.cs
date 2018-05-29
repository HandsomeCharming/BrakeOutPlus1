using UnityEngine;
using System.Collections;

public enum Powerups
{
    AutoPilot,
    Magnet,
    DoubleScore,
    Shield,
    Timeslow
}

public class Player : MonoBehaviour {

    public enum PlayerState
    {
        Paused,
        Launching,
        Playing,
        AutoControl,
        Gliding,
        Dead
    }



    public static Player current;
    
    public PlayerState playerState;

    public VehicleSuper vehicle;
    public PlayerPhysics physics;

    [HideInInspector]
    public float m_AccelerateTime = 0;
    [HideInInspector]
    public float m_RegularPushTime = 0;
    [HideInInspector]
    public float m_CooldownBeforeMultDecrease = 1.0f;
    [HideInInspector]
    public float m_CooldownBeforeMultDecreaseRemain = 0;
    [HideInInspector]
    public float m_MultIncreaseRate = 0.055f;
    [HideInInspector]
    public float m_MultIncreaseGap = 2.0f;
    [HideInInspector]
    public float m_MaxMult = 0.2f;
    [HideInInspector]
	public float m_BoostResetTime =1.0f;
	[HideInInspector]
	public float m_TiltAngleX;
    public int m_Health;

    bool died;
    
    Rigidbody m_RigidBody;

    void Start () {
        playerState = PlayerState.Paused;
        died = false;
        current = this;
        m_RigidBody = GetComponent<Rigidbody>();
        m_Health = 1;

		m_MultIncreaseGap = 0.3f;
		m_BoostResetTime =0.3f;
        m_MultIncreaseRate = 0.025f;
        m_MaxMult = 1.7f;
		m_TiltAngleX = transform.eulerAngles.x;
    }
	
	void Update () {
		if(playerState == PlayerState.Launching)
        {

        }
        else if(playerState == PlayerState.Playing || playerState == PlayerState.Gliding)
        {
            UpdateScoreMultiplier();
            if(FloorBuilder.current != null && transform.position.y < FloorBuilder.current.m_LastMakeFloorY - 25.0f)
            {
                AudioSystem.current.PlayEvent(AudioSystemEvents.FallEventName);
                Die();
            }
        }
        
	}

	void FixedUpdate() {
        if(playerState == PlayerState.Playing || playerState == PlayerState.Dead)
        {
            //rigidBody.AddForce(transform.localToWorldMatrix * Vector3.forward * force);
        } else if(playerState == PlayerState.Launching)
        {
            //rigidBody.AddForce(Vector3.down * gravity * rigidBody.mass);
        }
    }

    public void Launch()
    {
        playerState = PlayerState.Launching;
        physics.SetPhysicsState(PlayerPhysics.PlayerPhysicsState.Dropping);
        physics.Launch();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(playerState == PlayerState.Launching)
        {
            playerState = PlayerState.Playing;
            physics.RegularPush();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (playerState == PlayerState.Launching)
        {
            playerState = PlayerState.Playing;
        }
    }

    public void hitGround()
    {
        if (playerState == PlayerState.Launching)
        {
            playerState = PlayerState.Playing;
        }
    }

    public void Accelerate()
    {
        if (playerState == PlayerState.Playing)
        {
            physics.AcceleratePush();
            vehicle.OnHoldBoth();
        }
    }

    public void Recover()
    {
        if (playerState == PlayerState.Playing)
        {
            physics.RegularPush();
        }
    }

    public void RotateLeft(float m_RotatePercentage = 1.0f)
    {
        if (playerState == PlayerState.Playing || playerState == PlayerState.Gliding)
        {
            physics.RotateLeft(m_RotatePercentage);
            vehicle.OnRotateLeft();
        }

    }

    public void RotateRight(float m_RotatePercentage = 1.0f)
    {
        if (playerState == PlayerState.Playing || playerState == PlayerState.Gliding)
        {
            physics.RotateRight(m_RotatePercentage);
            vehicle.OnRotateRight();
        }
    }

    public void EnteredFloor(FloorMesh floor)
    {
    }

    public void UpdateScoreMultiplier()
    {
        if(physics.playerPhysicsState==  PlayerPhysics.PlayerPhysicsState.RegularMoving)
        {
            m_AccelerateTime = 0;
            if(GameManager.current.m_BoostMultiplier > 1.05f)
            {
                if (m_CooldownBeforeMultDecreaseRemain >= 0)
                    m_CooldownBeforeMultDecreaseRemain -= Time.deltaTime;
                else
                {
                    m_RegularPushTime += Time.deltaTime;
                    m_AccelerateTime = 0.0f;
                    if (m_RegularPushTime >= m_BoostResetTime)
                    {
                        m_RegularPushTime = 0;
                        GameManager.current.m_BoostMultiplier = 1.0f;//GameManager.current.m_ScoreMultiplier - m_MultDecreaseRate > 1.0f ? GameManager.current.m_ScoreMultiplier - m_MultDecreaseRate : 1.0f;
                        UIManager.current.m_Ingame.HideBoostUI();   //UpdateBoostMultiplierNumber(GameManager.current.m_ScoreMultiplier, false);
                    }
                }
            }
            else
            {
                if (m_CooldownBeforeMultDecreaseRemain < m_CooldownBeforeMultDecrease)
                    m_CooldownBeforeMultDecreaseRemain += Time.deltaTime;
            }
            
        }
        else if(physics.playerPhysicsState == PlayerPhysics.PlayerPhysicsState.Accelerating)
        {
            m_RegularPushTime = 0;
            m_AccelerateTime += Time.deltaTime;
            m_CooldownBeforeMultDecreaseRemain = m_CooldownBeforeMultDecreaseRemain + Time.deltaTime >= m_CooldownBeforeMultDecrease?
                m_CooldownBeforeMultDecrease : m_CooldownBeforeMultDecreaseRemain + Time.deltaTime;
            if (m_AccelerateTime >= m_MultIncreaseGap)
            {
                m_AccelerateTime = 0.0f;
                GameManager.current.m_BoostMultiplier = GameManager.current.m_BoostMultiplier + m_MultIncreaseRate < m_MaxMult ? GameManager.current.m_BoostMultiplier + m_MultIncreaseRate : m_MaxMult;
                UIManager.current.m_Ingame.UpdateBoostMultiplierNumber(GameManager.current.m_BoostMultiplier, true);
            }
        }
    }

    public void HitByObstacle(ObstacleType type)
    {
        print("Hit");
        print(m_Health);
        if(type == ObstacleType.Cube)
        {
            DecreaseHealth();
        }
        else if(type == ObstacleType.Wall)
        {
            if (physics.CanBeKilledByWall())
                DecreaseHealth();
        }
    }

    void DecreaseHealth()
    {
        m_Health--;
        if(ShieldOnPlayer.current != null)
        {
            ShieldOnPlayer.current.PlayPopAnim();
        }
        if (m_Health <= 0)
        {
            Die();
        }
    }


	public void SetBoost(CarClassData classData, SingleCarSelectData carData, CarSaveData data)
	{
		int level = data.m_BoostLevel-1; 
		int maxLevel = carData.maxUpgradeLevel;
		float lerpAmount = ((float)level) / (float)maxLevel;
		m_MaxMult = Mathf.Lerp(classData.m_MaxBoostMultiplier.min, classData.m_MaxBoostMultiplier.max, lerpAmount);
        m_MultIncreaseRate = Mathf.Lerp(classData.m_BoostIncreaseRate.min, classData.m_BoostIncreaseRate.max, lerpAmount);
    }

    public void Die()
    {
		if (playerState == PlayerState.Dead) return;
        //Player.current = null;
        playerState = PlayerState.Dead;
        if (died == false)
        {
            //GameManager.current.ReloadAfterDelay(2.0f);
            GameManager.current.EndGame();
        }
        died = true;
        physics.SetPhysicsState(PlayerPhysics.PlayerPhysicsState.Dead);

        GameManager.current.m_BoostMultiplier = 1.0f;
        UIManager.current.m_Ingame.UpdateBoostMultiplierNumber(1.0f, false);
        UIManager.current.m_Ingame.HideBoostUI();
    }

    public void Revive()
    {
        FloorMesh fm = FloorBuilder.current.floorMeshes[FloorBuilder.current.collidingIndex];
        Vector3 pos = (fm.prevPos1 + fm.prevPos2)/2.0f;
        pos.y += 1.0f;
        playerState = PlayerState.Paused;
        physics.playerPhysicsState = PlayerPhysics.PlayerPhysicsState.Paused;
        died = false;
        //Vector3 pos = new Vector3(2.5f, 2.0f, 0.0f);//(FloorBuilder.current.floorMeshes[GameManager.current.cIndex].prevPos1 + FloorBuilder.current.floorMeshes[GameManager.current.cIndex].prevPos2) / 2.0f;
        transform.position = pos;
        transform.forward = fm.prevDir;// Vector3.forward;//(FloorBuilder.current.floorMeshes[GameManager.current.cIndex].endPos1 - FloorBuilder.current.floorMeshes[GameManager.current.cIndex].prevPos1).normalized;
        
		Vector3 rot = transform.eulerAngles;
		rot.x = m_TiltAngleX;
		transform.eulerAngles = rot;

		CameraFollow.current.SnapBack();

        m_RigidBody.velocity = Vector3.zero;
        m_RigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        m_RigidBody.useGravity = false;
        GameManager.current.m_BoostMultiplier = 1.0f;
        InputHandler.current.ResetControls();
        Invoke("ReviveAfterCountdown", 0.3f);
        //ReviveAfterCountdown();
    }

    void ReviveAfterCountdown()
    {
        playerState = PlayerState.Playing;

        if (Player.current.gameObject.GetComponent<AutoPilot>() == null)
        {
            Player.current.gameObject.AddComponent<AutoPilot>().SetPilotTime(2.0f);
            Player.current.gameObject.GetComponent<AutoPilot>().enabled = true;
        }
        else
        {
            Player.current.gameObject.GetComponent<AutoPilot>().enabled = true;
            Player.current.gameObject.GetComponent<AutoPilot>().SetPilotTime(2.0f);
        }
        //physics.SetPhysicsState(PlayerPhysics.PlayerPhysicsState.);
        //physics.Launch();
        UIManager.current.m_Ingame.HideBoostUI();   //UpdateBoostMultiplierNumber(GameManager.current.m_ScoreMultiplier, false);
    }
}
