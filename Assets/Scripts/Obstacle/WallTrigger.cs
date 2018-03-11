using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger : MonoBehaviour {
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Player.current.playerState == Player.PlayerState.Playing)
        {
            transform.SendMessageUpwards("HitOnWall");
        }
    }
}
