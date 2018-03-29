using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotateByAxis : MonoBehaviour {

    public Vector3 m_RotSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(m_RotSpeed * Time.deltaTime);
	}
}
