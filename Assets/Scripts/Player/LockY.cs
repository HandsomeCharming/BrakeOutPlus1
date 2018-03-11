using UnityEngine;
using System.Collections;

public class LockY : MonoBehaviour {

	public float y;

	void Update () {
		Vector3 pos = transform.position;
		pos.y = y;
		transform.position = pos;
	}
}
