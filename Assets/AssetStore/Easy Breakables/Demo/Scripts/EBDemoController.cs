using UnityEngine;
using System.Collections;

public class EBDemoController : MonoBehaviour {

	public GameObject panelPrefab;
	public GameObject panelRigidbodyPrefab;
	public GameObject glassPrefab;
	public GameObject glassRigidbodyPrefab;
	public GameObject fencePrefab;
	public GameObject fenceRigidbodyPrefab;
	public GameObject towerPrefab;

	float force = 70f;

	void Update() {
		if(Input.GetMouseButtonDown(0)) {
			GameObject inst = (GameObject)Instantiate (panelRigidbodyPrefab, transform.position, transform.rotation * Quaternion.Euler(0,90,0));
			inst.GetComponent <Rigidbody>().AddForce (transform.forward * force, ForceMode.Impulse);
		}
		if(Input.GetMouseButtonDown(1)) {
			GameObject inst = (GameObject)Instantiate (fenceRigidbodyPrefab, transform.position, transform.rotation * Quaternion.Euler(0,90,0));
			inst.GetComponent <Rigidbody>().AddForce (transform.forward * force, ForceMode.Impulse);
		}
		if(Input.GetMouseButtonDown (2)) {
			RaycastHit hit;
			if(Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit)) {
				if(hit.collider.tag == "Breakable") {
					hit.collider.gameObject.GetComponent<EasyBreakable>().Damage (hit.point + (hit.normal * 0.25f), force);
				}
			}
		}

		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			RaycastHit h;
			if(Physics.Raycast (transform.position, transform.forward, out h, 9999999f)) {
				Instantiate (panelPrefab, h.point, Quaternion.Euler (0, transform.eulerAngles.y + 90, 0));
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			RaycastHit h;
			if(Physics.Raycast (transform.position, transform.forward, out h, 9999999f)) {
				Instantiate (panelRigidbodyPrefab, h.point, Quaternion.Euler (0, transform.eulerAngles.y + 90, 0));
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			RaycastHit h;
			if(Physics.Raycast (transform.position, transform.forward, out h, 9999999f)) {
				Instantiate (glassPrefab, h.point, Quaternion.Euler (0, transform.eulerAngles.y + 90, 0));
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha4)) {
			RaycastHit h;
			if(Physics.Raycast (transform.position, transform.forward, out h, 9999999f)) {
				Instantiate (glassRigidbodyPrefab, h.point, Quaternion.Euler (0, transform.eulerAngles.y + 90, 0));
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha5)) {
			RaycastHit h;
			if(Physics.Raycast (transform.position, transform.forward, out h, 9999999f)) {
				Instantiate (fencePrefab, h.point, Quaternion.Euler (0, transform.eulerAngles.y + 90, 0));
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha6)) {
			RaycastHit h;
			if(Physics.Raycast (transform.position, transform.forward, out h, 9999999f)) {
				Instantiate (fenceRigidbodyPrefab, h.point, Quaternion.Euler (0, transform.eulerAngles.y + 90, 0));
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha7)) {
			RaycastHit h;
			if(Physics.Raycast (transform.position, transform.forward, out h, 9999999f)) {
				Instantiate (towerPrefab, h.point, Quaternion.Euler (0, transform.eulerAngles.y + 90, 0));
			}
		}

		//if(

		if(Input.GetKey (KeyCode.Alpha9)) {
			force -= Time.deltaTime * 40;
		}
		if(Input.GetKey (KeyCode.Alpha0)) {
			force += Time.deltaTime * 40;
		}
	}

	void OnGUI () {
		GUI.Label (new Rect(10,10,500,20), "Force: " + force + " (Adjust with 9 and 0)");
		GUI.Box (new Rect(Screen.width / 2 - 5, Screen.height / 2 - 5, 10,10), "O");

		GUI.Label (new Rect(10,30,500,20), "LMB shoots a panel. RMB shoots a fence. MMB shoots with a raycast.");
		GUI.Label (new Rect(10,50,500,20), "1 spawns a panel, 2 spawns a rigidbody panel");
		GUI.Label (new Rect(10,70,500,20), "3 spawns a glass panel, 4 spawns a rigidbody glass panel");
		GUI.Label (new Rect(10,90,500,20), "5 spawns a fence, 6 spawns a rigidbody fence");
		GUI.Label (new Rect(10,110,500,20), "Press 7 to spawn a tower.");
	}

}
