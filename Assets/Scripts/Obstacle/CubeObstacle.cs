using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeObstacle : Obstacle {


    public float m_ImpactImpulse = 10.0f;

    // Update is called once per frame
    public override void MoveToCallBack()
    {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<BoxCollider>().enabled = true;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !m_Triggered)
        {
            //Player.current.GetComponent<Rigidbody>().constraints = 0;
            m_Triggered = true;
            if (Player.current.playerState == Player.PlayerState.Playing)
            {
                Player.current.HitByObstacle(ObstacleType.Cube);
                AudioSystem.current.PlayCrash();
            }
            GetComponent<Rigidbody>().AddForce(transform.forward * m_ImpactImpulse, ForceMode.Impulse);
        }
    }
}
