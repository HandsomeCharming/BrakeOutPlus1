using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDetectTrigger : MonoBehaviour {

    FloorMesh m_Floor;

	// Use this for initialization
	void Awake () {
        m_Floor = GetComponentInParent<FloorMesh>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            m_Floor.OnPlayerEnter();
        }
    }
}
