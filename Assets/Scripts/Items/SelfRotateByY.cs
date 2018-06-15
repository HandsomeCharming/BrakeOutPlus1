using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotateByY : MonoBehaviour {

	const float deltaTime = 1.0f / 60.0f;
    public float rotateRate;

    void Update()
    {
		float delta = Time.deltaTime == 0 ? deltaTime : Time.deltaTime;
        transform.Rotate(Vector3.up, rotateRate * delta);
    }
}
