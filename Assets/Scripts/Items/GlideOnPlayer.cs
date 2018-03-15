using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class GlideOnPlayer : MonoBehaviour {

    bool canCollide;
    PlayerPhysics m_PlayerPhysics;
    Player m_Player;

    const float m_GlideTillCollideTime = 2.0f;
    const float m_GlideMinSpeed = 45.0f;

    private void Awake()
    {
        m_Player = GetComponent<Player>();
        m_PlayerPhysics = m_Player.physics;
        canCollide = false;
        if(GetComponent<AutoPilot>())
        {
            GetComponent<AutoPilot>().EndAutoPilot();
        }
        Invoke("SetCanCollide", m_GlideTillCollideTime);
        StartGliding();

        AudioSystem.current.PlayEvent(AudioSystemEvents.GlideStartEventName);
    }

    void SetCanCollide()
    {
        canCollide = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(canCollide && collision.gameObject.CompareTag("Floor"))
        {
            EndGliding();
        }
    }

    public void StartGliding()
    {
        m_Player.playerState = Player.PlayerState.Gliding;
        m_Player.vehicle.StartGliding();
        m_PlayerPhysics.SetPhysicsState(PlayerPhysics.PlayerPhysicsState.Gliding);
        Rigidbody playerRB = m_Player.GetComponent<Rigidbody>();
        if(playerRB.velocity.magnitude < m_GlideMinSpeed)
        {
            playerRB.velocity = m_Player.transform.forward * m_GlideMinSpeed;
        }
        //BackgroundMaterial.current.StartSharpLerpToBlack();
    }

    public void EndGliding()
    {
        m_Player.playerState = Player.PlayerState.Playing;
        m_Player.vehicle.EndGliding();
        m_PlayerPhysics.SetPhysicsState(PlayerPhysics.PlayerPhysicsState.RegularMoving);

        BackgroundMaterial.current.StartSharpLerp();
        AudioSystem.current.PlayEvent(AudioSystemEvents.GlideStopEventName);
        Destroy(this);
    }
}
