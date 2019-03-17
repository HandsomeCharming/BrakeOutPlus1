using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObstacle : Obstacle {

    Rigidbody[] childRigidBodies;
    const float m_DestroyTime = 2.0f;

    private void Awake()
    {
        childRigidBodies = GetComponentsInChildren<Rigidbody>();
    }

    public override void MoveToCallBack()
    {
        foreach (var rb in childRigidBodies)
        {
            //rb.useGravity = true;
            //rb.gameObject.GetComponent<Collider>().enabled = true;
        }
    }

    void HitOnWall()
    {
        Player.current.HitByObstacle(ObstacleType.Wall);
        HitEffect();

        CubeTrigger[] cubes = GetComponentsInChildren<CubeTrigger>();
        foreach(var cube in cubes)
        {
            cube.GetComponent<Collider>().enabled = false;
        }
    }

    void HitOnCube()
    {
        Player.current.HitByObstacle(ObstacleType.Cube);
        HitEffect();
    }

    void HitEffect()
    {
        transform.GetComponentInChildren<EasyBreakable>().Damage(100);

        CameraEffectManager.Bloom(2.0f, 0.1f);
        CameraFollow.current.StartShaking(0.2f, 0.3f);
        if (Player.current.m_Health >= 0)
            AudioSystem.current.PlayEvent(AudioSystemEvents.WallEventName);

        Destroy(this.gameObject, m_DestroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Player.current.GetComponent<Rigidbody>().constraints = 0;
            print("WALLLLLLL");
            if (Player.current.playerState == Player.PlayerState.Playing) {
                HitOnWall();
            } else if(Player.current.playerState == Player.PlayerState.AutoControl) {
                HitEffect();
            }
        }
    }
}
