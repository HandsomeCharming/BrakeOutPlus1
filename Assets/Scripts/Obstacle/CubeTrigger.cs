using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        print("cubetrigger");
        if (other.gameObject.CompareTag("Player") && Player.current.playerState == Player.PlayerState.Playing)
        {
            transform.SendMessageUpwards("HitOnCube");
        }
    }
}
