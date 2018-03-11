using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToDecreasingSpeed : MonoBehaviour {

    public Vector3 from;
    public Vector3 to;

    float totalDist;

    public float maxSpeed;
    public float minSpeed;
    public float minSpeedThreshold;

    public bool reached;

    Quaternion initialRot;

    public void resetAnim()
    {
		enabled = true;
        totalDist = (to - from).magnitude;
        transform.rotation = Quaternion.Euler(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180));
        initialRot = transform.rotation;
        reached = false;
    }

    public void moveToword()
    {
        float lerpValue = (totalDist - (to - transform.position).magnitude) / totalDist;
        transform.rotation = Quaternion.Lerp(initialRot, Quaternion.identity, lerpValue);
        float speed = Mathf.Lerp(maxSpeed, minSpeed, lerpValue);
        Vector3 pos = Vector3.MoveTowards(transform.position, to, speed*Time.deltaTime);
        transform.position = pos;
        if (pos == to)
        {
            reached = true;
            transform.rotation = Quaternion.identity;
        }
    }

	void Update () {
        if (!reached)
			moveToword();
		else
			enabled = false;
	}
}
