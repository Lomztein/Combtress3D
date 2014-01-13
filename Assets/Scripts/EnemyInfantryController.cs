using UnityEngine;
using System.Collections;

public class EnemyInfantryController: MonoBehaviour {
	
	public float maxSpeed;
	public Transform target;
	private Transform targetPointer;
	public float range;
	public GameObject newEquip;
	public GameObject equip;
	private GameObject[] targets;
	private Transform weaponPos;
	StatsManager stats;
	Quaternion newLerp;
	float speedY;
	CharacterController charController;


	// Use this for initialization
	void Start () {
		stats = GameObject.FindGameObjectWithTag("Stats").GetComponent<StatsManager>();
		targets = stats.freindlies;
		target = GetNearestTaggedObject(targets);
		charController = GetComponent<CharacterController>();
		targetPointer = transform.FindChild ("TargetPointer");
		weaponPos = targetPointer.FindChild ("WeaponPos");
		newLerp = transform.rotation;
		SpawnEquipment();
	}

	void SpawnEquipment () {
		if (equip) {
			Destroy(equip);
			equip = null;
		}
		equip = (GameObject)Instantiate(newEquip,weaponPos.position,weaponPos.rotation);
		equip.transform.parent = targetPointer;
		newEquip = null;
	}
	
	// Update is called once per framemy
	void Update () {

		//Rotation
		Quaternion newRot = new Quaternion(transform.rotation.x,targetPointer.rotation.y,transform.rotation.z,targetPointer.rotation.w);
		newLerp = Quaternion.Lerp (newLerp,newRot,5*Time.deltaTime);
		transform.rotation = newLerp;

		//Movement
		float forwardSpeed = maxSpeed;

		speedY += Physics.gravity.y * Time.deltaTime;

		Vector3 speed = new Vector3 ( 0, speedY, forwardSpeed);

		if (target) {
			targetPointer.LookAt (target.position);
			if (Vector3.Distance(transform.position,target.position) > range) {
				speed = transform.rotation * speed;
			}else{
				speed = new Vector3 (0,0,0);
				equip.SendMessage("Fire");
			}
			target = GetNearestTaggedObject(targets);
		}

		charController.Move(speed * Time.deltaTime);
		target = GetNearestTaggedObject(stats.freindlies);

	}

	Transform GetNearestTaggedObject(GameObject[] type) {

		Transform nearestObj = null;
		
		if (type.Length > 0) {
			
			float nearestDistanceSqr = Mathf.Infinity;
			GameObject[] taggedGameObjects = type;
			
			// loop through each tagged object, remembering nearest one found
			if (taggedGameObjects[0] != null) {
				foreach (GameObject obj in taggedGameObjects) {
					
					Vector3 objectPos = obj.transform.position;
					float distanceSqr = (objectPos - transform.position).sqrMagnitude;
					
					if (distanceSqr < nearestDistanceSqr) {
						nearestObj = obj.transform;
						nearestDistanceSqr = distanceSqr;
					}
				}
			}
		}
		return nearestObj;
	}
}
