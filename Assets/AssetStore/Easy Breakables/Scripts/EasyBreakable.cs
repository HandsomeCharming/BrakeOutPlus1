using UnityEngine;
using System.Collections;

public class EasyBreakable : MonoBehaviour {

	public bool debug = false; //Whether to show debugging information

	Vector3 lastPosition; //The last impact position
	float lastDamage; //Last impact damage
	float lastMass; //Last impact mass, used only my OnCollisionEnter. Any manual impact by Damage() sets this to 1
	Collider coll; //This object's collider.
	MeshRenderer mesh; //This object's mesh.
	Rigidbody rigidb; //This object's rigidbody, it is not required, but it checks in case it has one.
	Vector3 startPosition; //The starting object's position;
	Quaternion startRotation; //The starting object's rotation;

	//Current strength of the breakable
	public float strength = 100f;
	float startingStrength = 100f;

	public bool kinematic = false; //Change this to true if you with to have the rigidbody kinematic(has some benefits over no rigidbody)

	bool broken = false;


	public MeshRenderer[] externalMeshes; //Assign any meshes that are not on the base gameobject here!

	public GameObject[] damageLevel1prefabs;
	GameObject damageLevel1;
	Rigidbody[] damageLevel1rigidbodies;
	public float damageLevel1damage = 25f;

	public GameObject[] damageLevel2prefabs;
	GameObject damageLevel2;
	Rigidbody[] damageLevel2rigidbodies;
	public float damageLevel2damage = 60f;

	public GameObject[] damageLevel3prefabs;
	GameObject damageLevel3;
	Rigidbody[] damageLevel3rigidbodies;
	
	public bool dontDespawn = false; //Select this if you do not want the debris to despawn.
	public float timeToDespawn = 20f; //How long to wait until the debris disables itself for increased performance?
	float despawnTimer = 0f;
	bool despawned = false;

	public bool rigidbodyDamage = true; //Whether this breakable can be damaged by flying rigidbodies
	//How much multiply the velocity of a rigidbody to get the final damage
	//Whether to filter damaging rigidbodies by tag
	public float minImpactDamageToHurt = 2.5f; //A rigidbody needs to deal at least this amount of damage to hurt the breakable.
	public bool tagFilter = true;
	public string[] tags; //The tags which can damage the breakable

	//Adjust these if necessary
	public float breakEffectForce = 5f;
	public float breakEffectRadius = 1f;
	public float physicsMultiplier = 1f;

	public bool useAudio = false; //Whether to use audio for the script
	AudioSource source; //The object's audiosource
	public AudioClip[] hitSounds;
	public AudioClip[] breakSounds;
	public float minDamageForSound = 2.5f;
	public float minPitch = 0.8f;
	public float maxPitch = 1.2f;

	void Start() {



		startingStrength = strength; //Needed to reset.
		//If these are not available on the gameobject, EasyBreakable will ignore them.
		coll = GetComponent<Collider>();
		mesh = GetComponent<MeshRenderer>();
		rigidb = GetComponent<Rigidbody>();

		Validate ();

		startPosition = transform.position;
		startRotation = transform.rotation;

		//Generate the damage level gameobjects in the Start function so we do not have to instantiate while playing
		Regenerate ();


	}


	void Validate() {

		//This part of code checks if everything is set up correctly.

		if(strength <= 0) {
			Debug.Log (gameObject.name + " The strength is 0. This breakable will break immediately!");
		}

		if(damageLevel1prefabs.Length <= 0) {
			Debug.LogError (gameObject.name + " No Damage Level 1 prefabs. Disabling...");
			this.enabled = false;
		}

		if(damageLevel2prefabs.Length <= 0) {
			Debug.LogError (gameObject.name + " No Damage Level 2 prefabs. Disabling...");
			this.enabled = false;
		}

		if(damageLevel3prefabs.Length <= 0) {
			Debug.LogError (gameObject.name + " No Damage Level 3 prefabs. Disabling...");
			this.enabled = false;
		}

		if(tagFilter) {
			if(tags.Length <= 0) {
				Debug.LogWarning(gameObject.name + " Tag Filter is on but there are no tags assigned. Disabling Tag Filter...");
				tagFilter = false;
			}
		}

		if(useAudio) {
			source = GetComponent<AudioSource>();
			if(source == null) {
				Debug.LogError(gameObject.name + " No AudioSource on this object, disabling sound.");
				useAudio = false;
			}

			if(hitSounds.Length <= 0) {
				Debug.LogWarning(gameObject.name + "Sound is on but there are no hit sounds. Sound is still enabled.");
			}
			if(breakSounds.Length <= 0) {
				Debug.LogWarning(gameObject.name + "Sound is on but there are no break sounds. Sound is still enabled.");
			}
		}

	}


	void Update() {
		if(broken) {
			if(!dontDespawn) {

				//This is a very simple timer which will check if it should disable the broken pieces for increased performance.

				despawnTimer += Time.deltaTime;
				if(despawnTimer >= timeToDespawn) {
					if(!despawned) {
						damageLevel1.SetActive (false);
						damageLevel2.SetActive (false);
						damageLevel3.SetActive (false);
						despawned = true;
					}
				}
			}
		}
	}

	void OnCollisionEnter(Collision collision) {
		bool valid = false;

		if(tagFilter) { //Only proceed further if a tag is in the filter list if the filter is enabled.
			foreach(string t in tags) {
				if(t == collision.collider.tag) {
					valid = true;
					break; //Breaking because there is no need continuing the loop if we found a match.
				}
			}
		} else {
			valid = true;
		}

		if(valid) { //Running the collision code further only if we found a match or filter is off
			float massIfAny = 1f; //Get the mass of the collision rigidbody here if it has any so we dont get NullRefs

			if(collision.collider.attachedRigidbody != null) {
				massIfAny = collision.collider.attachedRigidbody.mass;
			}

			if(Vector3.Magnitude (collision.relativeVelocity * physicsMultiplier * massIfAny) >= minImpactDamageToHurt) {

				if(debug) {
					Debug.Log (gameObject.name + " hit by rigidbody " + collision.gameObject.name + " with velocity " + collision.relativeVelocity + " and mass " + massIfAny + " Collision normal " + collision.contacts[0].normal);
				}

				Damage (collision.contacts[0].point - (collision.contacts[0].normal * 0.25f), Vector3.Magnitude (collision.relativeVelocity * physicsMultiplier * massIfAny));
				lastMass = massIfAny;
			}
		}
	}


	public void Damage (float damage) {
		Damage (Vector3.zero, damage); //Just an overload for lazy people or situations where you can't get the position.
	}

	public void Damage (Vector3 pos, float damage) {

		//THE POSITION NEEDS TO BE IN WORLD COORDINATES
		//Call this void if you, for example, want to damage the breakable with a raycast from a gun.
		strength -= damage;
		lastPosition = pos;
		lastDamage = damage;
		lastMass = 1f;


		if(debug) {
			Debug.Log (gameObject.name + " Applying damage: " + damage + " at position " + pos.ToString ());
		}


		if(strength <= 0) {
			Break ();
			if(useAudio) {
				if(breakSounds.Length > 0) { //No exceptions in case there are no sounds/no need for a sound
					source.pitch = Random.Range (minPitch, maxPitch);
					source.PlayOneShot (breakSounds[Random.Range (0, breakSounds.Length)]);
				}
			}
		} else {
			if(useAudio) {
				if(hitSounds.Length > 0) { //Again, no exceptions if no sounds assigned
					if(damage >= minDamageForSound) {
						source.pitch = Random.Range (minPitch, maxPitch);
						source.PlayOneShot (hitSounds[Random.Range (0, hitSounds.Length)]);
					}
				}
			}
		}
	}

	void Break () {

		//Do not call this function without any context about damage.

		if(!broken) { //Only activate if object is not broken yet
			//Disabling the healthy object
			broken = true;
			if(coll != null) {
				coll.enabled = false;
			}
			if(mesh != null) {
				mesh.enabled = false;
			}
			foreach(MeshRenderer m in externalMeshes) {
				m.enabled = false;
			}

			despawnTimer = 0f;
			despawned = false;

			//Comparing the last hit damage and enabling the correct object.
			if(lastDamage > damageLevel2damage) {
				//Use damage level 3
				damageLevel3.SetActive (true);
				if(debug) {
					Debug.Log (gameObject.name + " Breaking with level 3, with force " + (lastDamage * breakEffectForce * lastMass).ToString() + " at " + lastPosition.ToString ());
				}
				foreach(Rigidbody b in damageLevel3rigidbodies) {
					if(rigidb != null) {
						b.velocity = rigidb.velocity;
						b.angularVelocity = rigidb.angularVelocity;
					}

					b.AddExplosionForce (lastDamage * breakEffectForce * lastMass, lastPosition, 1f * breakEffectRadius);


				}
			} else if(lastDamage > damageLevel1damage) {
				//Use damage level 2
				damageLevel2.SetActive (true);
				if(debug) {
					Debug.Log (gameObject.name + " Breaking with level 2, with force " + (lastDamage * breakEffectForce * lastMass).ToString() + " at " + lastPosition.ToString ());
				}
				foreach(Rigidbody b in damageLevel2rigidbodies) {
					if(rigidb != null) {
						b.velocity = rigidb.velocity;
						b.angularVelocity = rigidb.velocity;
					}

					b.AddExplosionForce (lastDamage * breakEffectForce * lastMass, lastPosition, 1f * breakEffectRadius);

				}
			} else {
				//Use damage level 1
				damageLevel1.SetActive (true);
				if(debug) {
					Debug.Log (gameObject.name + " Breaking with level 1, with force " + (lastDamage * breakEffectForce * lastMass).ToString() + " at " + lastPosition.ToString ());
				}
				foreach(Rigidbody b in damageLevel1rigidbodies) {
					if(rigidb != null) {
						b.velocity = rigidb.velocity;
						b.angularVelocity = rigidb.velocity;
					}

					b.AddExplosionForce (lastDamage * breakEffectForce * lastMass, lastPosition, 1f * breakEffectRadius);

				}
			}

			if(rigidb != null) {
				rigidb.isKinematic = true;
				//We disable the rigidbody last so we dont lose any velocity and the break effect looks more convincing.
			}

		}
	}

	public void Regenerate () { //This void is public, just in case you want to reset the breakable from some other script

		//First we make sure we destroy the old breakables.
		if(damageLevel1 != null) {
			Destroy (damageLevel1);
		}
		if(damageLevel2 != null) {
			Destroy (damageLevel2);
		}
		if(damageLevel3 != null) {
			Destroy (damageLevel3);
		}

		//Then we make sure to reset the breakable to it's starting state
		strength = startingStrength;
		broken = false;
		if(coll != null) {
			coll.enabled = true;
		}
		despawnTimer = 0f;
		despawned = false;
		if(mesh != null) {
			mesh.enabled = true;
		}
		foreach(MeshRenderer m in externalMeshes) {
			m.enabled = true;
		}
		if(rigidb != null) {
			if(!kinematic) {
				rigidb.isKinematic = false;
			} else {
				rigidb.isKinematic = true;
			}
			//Stop any movement
			rigidb.velocity = Vector3.zero;
			rigidb.angularVelocity = Vector3.zero;
		}

		//Reset the object's rotation and position
		transform.position = startPosition;
		transform.rotation = startRotation;

		//Regenerate the destruction prefabs
		damageLevel1 = (GameObject)Instantiate (damageLevel1prefabs[Random.Range (0, damageLevel1prefabs.Length)], Vector3.zero, Quaternion.identity);
		damageLevel1rigidbodies = damageLevel1.GetComponentsInChildren <Rigidbody>();
		damageLevel1.transform.SetParent (transform, false);
		damageLevel1.SetActive (false);
		
		damageLevel2 = (GameObject)Instantiate (damageLevel2prefabs[Random.Range (0, damageLevel2prefabs.Length)], Vector3.zero, Quaternion.identity);
		damageLevel2rigidbodies = damageLevel2.GetComponentsInChildren <Rigidbody>();
		damageLevel2.transform.SetParent (transform, false);
		damageLevel2.SetActive (false);
		
		damageLevel3 = (GameObject)Instantiate (damageLevel3prefabs[Random.Range (0, damageLevel2prefabs.Length)], Vector3.zero, Quaternion.identity);
		damageLevel3rigidbodies = damageLevel3.GetComponentsInChildren <Rigidbody>();
		damageLevel3.transform.SetParent (transform, false);
		damageLevel3.SetActive (false);

	}

}
