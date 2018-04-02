using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundKeepDistance : MonoBehaviour {

    Vector3 dist;

	// Use this for initialization
	void Start () {
        if (Player.current != null)
        {
            dist = transform.position - Player.current.transform.position;
        }
        else
        {
            Destroy(this);
        }
	}
	
	void Update () {
        if(Player.current != null)
            transform.position = Player.current.transform.position + dist;
    }
}
