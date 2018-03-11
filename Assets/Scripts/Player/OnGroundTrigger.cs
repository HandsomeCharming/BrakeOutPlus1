using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundTrigger : MonoBehaviour {
    public Player parent;
	
    void OnTriggerEnter(Collider col)
    {
        parent.hitGround();
    }
}
