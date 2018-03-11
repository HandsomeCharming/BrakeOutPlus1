using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotateByY : MonoBehaviour {


    public float rotateRate;

    void Update()
    {
        transform.Rotate(Vector3.up, rotateRate * Time.deltaTime);
    }
}
