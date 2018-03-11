using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveToDecSpeedWithoutRot : MonoBehaviour {
    public delegate void MoveToReached(); // declare delegate type

    public MoveToReached callbackFct; // to store the function

    public Vector3 from;
    public Vector3 to;

    float totalDist;

    public float maxSpeed;
    public float minSpeed;
    public float minSpeedThreshold;

    public bool reached;

    public UnityEvent triggerReached;

    public void resetAnim()
    {
        totalDist = (to - from).magnitude;
        reached = false;
    }

    public void moveToword()
    {
        float lerpValue = (totalDist - (to - transform.position).magnitude) / totalDist;
        float speed = Mathf.Lerp(maxSpeed, minSpeed, lerpValue);
        Vector3 pos = Vector3.MoveTowards(transform.position, to, speed * Time.deltaTime);
        transform.position = pos;
        if (pos == to)
        {
            reached = true;
            if (GetComponent<Collider>() != null)
                GetComponent<Collider>().enabled = true;
            if (callbackFct != null)
                callbackFct.Invoke();
        }
    }

    void Update()
    {
		if (!reached)
			moveToword ();
		else
        {
            enabled = false;
        }
    }
}
