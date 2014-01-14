using UnityEngine;
using System.Collections;
[RequireComponent (typeof(LineRenderer))]

public class BulletScript : MonoBehaviour {
	
	public float damage;
	public string bulletType;
	public string armorType;
	public Transform parentUnit;
	public float range;
	public float speed;
	public GameObject hitParticle;
	public Transform target;
	private HealthScript colHealth;
	public string faction;
	private Vector3 startingPos;

	// Use this for initialization
	void Start () {
		startingPos = transform.position;
		if (bulletType == "Direct") {
			rigidbody.useGravity = false;
		}
		if (bulletType == "Artillary") {
			rigidbody.useGravity = true;
		}
		if (bulletType == "Instant") {
			RaycastHit hit;
			if (Physics.Raycast(transform.position,transform.forward, out hit, range)) {
				colHealth = hit.transform.gameObject.GetComponent<HealthScript>();
				if (colHealth) {
					Hit (hit.transform.gameObject);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		float distanceToParent = Vector3.Distance (startingPos,transform.position);
		if (distanceToParent > range) {
			Destroy(gameObject);
		}
	
		if (bulletType == "Homing") {
			if (target) {
				transform.LookAt (target.position);
				rigidbody.AddForce (transform.forward * speed/50);
			}else{
				Destroy (gameObject);
			}
		}
	}

	void OnTriggerEnter (Collider col) {
		//Debug.Log (col.gameObject.name);
		if (faction != col.gameObject.tag) {
			Destroy(gameObject);
			Hit (col.gameObject);
		}
	}

	void Hit (GameObject hitObject) {
		colHealth = hitObject.GetComponent<HealthScript>();
		if (colHealth) {
			//Debug.Log ("Bullet hit "+hitObject.name+" with "+colHealth.armorType+" armor, dealing "+damage+" "+armorType+" damage.");
			if (colHealth.armorType == armorType) {
				colHealth.hull -= damage;
			}else{
				colHealth.hull -= damage/5;
			}
		}
	}
}
