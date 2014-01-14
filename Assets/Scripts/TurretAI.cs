using UnityEngine;
using System.Collections;

public class TurretAI : MonoBehaviour {

	public float range = 100;
	public float firerate;
	public float reloadSpeed;
	public Transform target;
	public GameObject bulletType;
	public float bulletSpeed = 2000;
	public int bulletAmount = 1;
	private GameObject closest;
	public GameObject stand;
	public GameObject standPrefab;
	public string armorType;
	public string bulletArmorType;
	public float damage;
	private Transform[] muzzles;
	public bool reloaded = true;
	public float inaccuracy;
	public GameObject fireParticle;
	private Transform model;
	private Transform modelPivot;
	public float startingRange;
	public float startingDamage;
	public float startingFirerate;
	public float startingHull;
	public string fireMode = "Instant";
	public float sequenceTime;
	public float turnSpeed;
	public string bulletPhysics;
	public string faction;
	private GameObject[] enemies;
	private Transform nearestObj;
	private HealthScript healthScript;
	private StatsManager stats;
	private FreindlyScript fscript;
	public Vector3 randomPos;
	//private int muzzleSequence = 0;
	//private float fireTime = 0;
	private Transform aimTarget;

	void Start () {

		//Get transforms
		Invoke ("LookRandom",2 + Random.Range (0,3));
		stats = GameObject.FindGameObjectWithTag("Stats").GetComponent<StatsManager>();
		fscript = GetComponent<FreindlyScript>();
		faction = fscript.faction;

		healthScript = GetComponent<HealthScript>();

		modelPivot = transform.FindChild("TurretPivot");
		aimTarget = modelPivot.FindChild ("AimTarget");
		model = modelPivot.FindChild ("Model");
		int muzzleNumber = 0;
		muzzles = new Transform[model.childCount];
		foreach (Transform child in model.transform) {
			muzzles[muzzleNumber] = child;
			muzzleNumber++;
		}

		startingRange = range;
		startingDamage = damage;
		startingFirerate = firerate;
		startingHull = healthScript.hull;

		//enemies = stats.enemies.ToArray (typeof(GameObject)) as GameObject[];
		if (!stand) {
			stand = (GameObject)Instantiate(standPrefab,transform.position,Quaternion.identity);
			transform.parent = stand.transform;
			transform.parent.position += new Vector3 (0,0.68f,0);
		}

		if (armorType != "") {
			if (armorType == "light") {
				stand.transform.localScale = new Vector3 (0.5f,0.5f,0.5f);
				transform.localScale = new Vector3 (2,2,2);
			}
			if (armorType == "medium") {
				stand.transform.localScale = new Vector3 (1f,1f,1f);
				transform.localScale = new Vector3 (1,1,1);
			}
			if (armorType == "heavy") {
				stand.transform.localScale = new Vector3 (2f,2f,2f);
				transform.localScale = new Vector3 (0.5f,0.5f,0.5f);
			}
		}else{
			Debug.Log ("Turret doesn't have a type!");
		}
	}

	void Update () {

		Collider[] currentColliders = Physics.OverlapSphere(transform.position,range);
		int enemyNumber = 0;
		int amountOfEnemies = 0;
		foreach (Collider col in currentColliders) {
			if (col.gameObject.tag == "Enemy") {
				amountOfEnemies++;
			}
		}
		enemies = new GameObject[amountOfEnemies];
		foreach (Collider enemyCol in currentColliders) {
			if (enemyCol.gameObject.tag == "Enemy") {
				enemies[enemyNumber] = enemyCol.gameObject;
				enemyNumber++;
			}
		}
		
		if (target == null) {
			target = GetNearestTaggedObject (enemies);
			aimTarget.LookAt(randomPos);
		}else{
			if (Vector3.Distance (transform.position,target.position) > range) {
				target = GetNearestTaggedObject (enemies);
			}
			Vector3 targetCenter = target.GetComponent<MeshRenderer>().bounds.center;
			aimTarget.LookAt(targetCenter);
			RaycastHit hit;
			Physics.Raycast(model.transform.position,target.position - model.transform.position,out hit);
			if (hit.transform != target) {
				target = GetNearestTaggedObject (enemies);
			}
		}

		Quaternion newRot = Quaternion.Lerp (modelPivot.rotation,aimTarget.rotation,turnSpeed * Time.deltaTime);
		modelPivot.rotation = newRot;

		if (target != null) {
			RaycastHit hit;
			if (Physics.Raycast(muzzles[0].position, muzzles[0].forward, out hit, range)) {
				//Debug.Log (hit.transform.name);
				if (hit.transform == target) {
					Fire ();
				}
			}
		}
	}

	void OnDrawGizmos () {
		//Gizmos.DrawSphere (aimHit.point,0.5f);
		//Gizmos.DrawLine (muzzles[0].position,aimHit.point);
		Gizmos.DrawWireSphere (transform.position,range);
	}

	void Fire () {

		//Debug.Log (name + "fired a " + bulletType.name);

		if (fireMode == "Instant") {
			if (Vector3.Distance (transform.position,target.position) < range) {
				if (reloaded == true) {
				foreach (Transform muzzle in muzzles) {
					int internalAmount = bulletAmount;
						while (internalAmount > 0) {
							GameObject lastBullet = (GameObject)Instantiate(bulletType,muzzle.position,muzzle.rotation);
							Instantiate (fireParticle,muzzle.position,muzzle.rotation);
							lastBullet.rigidbody.AddForce(muzzle.forward * bulletSpeed);
							lastBullet.rigidbody.AddForce (muzzle.right * Random.Range (-inaccuracy,inaccuracy));
							lastBullet.rigidbody.AddForce (muzzle.up * Random.Range (-inaccuracy,inaccuracy));
							BulletScript lastScript = lastBullet.GetComponent<BulletScript>();
							lastScript.armorType = bulletArmorType;
							lastScript.damage = damage;
							lastScript.bulletType = bulletPhysics;
							lastScript.parentUnit = transform;
							lastScript.range = range;
							lastScript.target = target;
							lastScript.speed = bulletSpeed;
							lastScript.faction = faction;
							internalAmount--;
						}
					}
					reloaded = false;
					Invoke ("Reload",firerate);
				}
			}
		}
		//Different fire modes, enable or rewrite fire code later
/*		if (fireMode == "Random") {
			if (Vector3.Distance (transform.position,target.position) < range) {
				if (reloaded == true) {
					Transform muzzle = muzzles[Random.Range (0,muzzles.Length)];
					int internalAmount = bulletAmount;
					while (internalAmount > 0) {
						GameObject lastBullet = (GameObject)Instantiate(bulletType,muzzle.position,muzzle.rotation);
						Instantiate (fireParticle,muzzles[muzzleSequence].position,muzzle.rotation);
						lastBullet.rigidbody.AddForce(muzzles[muzzleSequence].forward * bulletSpeed);
						lastBullet.rigidbody.AddForce (muzzles[muzzleSequence].right * Random.Range (-inaccuracy,inaccuracy));
						lastBullet.rigidbody.AddForce (muzzles[muzzleSequence].up * Random.Range (-inaccuracy,inaccuracy));
						BulletScript lastScript = lastBullet.GetComponent<BulletScript>();
						lastScript.armorType = bulletArmorType;
						lastScript.damage = damage;
						lastScript.bulletType = bulletPhysics;
						lastScript.parentUnit = transform;
						lastScript.range = range;
						lastScript.target = target;
						lastScript.speed = bulletSpeed;
						internalAmount--;
					}
				reloaded = false;
				Invoke ("Reload",firerate);
				}
			}
		}

		if (fireMode == "Sequence") {
			if (Vector3.Distance (transform.position,target.position) < range) {
				if (reloaded == true) {
					muzzleSequence = 0;
					int internalAmount = bulletAmount;
					while (internalAmount > 0) {
						GameObject lastBullet = (GameObject)Instantiate(bulletType,muzzles[muzzleSequence].position,muzzles[muzzleSequence].rotation);
						Instantiate (fireParticle,muzzles[muzzleSequence].position,muzzles[muzzleSequence].rotation);
						lastBullet.rigidbody.AddForce(muzzles[muzzleSequence].forward * bulletSpeed);
						lastBullet.rigidbody.AddForce (muzzles[muzzleSequence].right * Random.Range (-inaccuracy,inaccuracy));
						lastBullet.rigidbody.AddForce (muzzles[muzzleSequence].up * Random.Range (-inaccuracy,inaccuracy));
						BulletScript lastScript = lastBullet.GetComponent<BulletScript>();
						lastScript.armorType = bulletArmorType;
						lastScript.damage = damage;
						lastScript.bulletType = bulletPhysics;
						lastScript.parentUnit = transform;
						lastScript.range = range;
						lastScript.target = target;
						lastScript.speed = bulletSpeed;
						internalAmount--;
					}
					reloaded = false;
					//Debug.Log ("STOP!");
					Invoke ("Reload",firerate);
					muzzleSequence++;
					if (sequenceTime != 0) {
						fireTime = sequenceTime;
						Invoke ("FireSequence",fireTime);
					}else{
						fireTime = firerate/muzzles.Length;
						Invoke ("FireSequence",fireTime);
					}	
				}
			}
		}
	}

	void FireSequence () {
		int internalAmount = bulletAmount;
		while (internalAmount > 0) {
			GameObject lastBullet = (GameObject)Instantiate(bulletType,muzzles[muzzleSequence].position,muzzles[muzzleSequence].rotation);
			Instantiate (fireParticle,muzzles[muzzleSequence].position,muzzles[muzzleSequence].rotation);
			lastBullet.rigidbody.AddForce(muzzles[muzzleSequence].forward * bulletSpeed);
			lastBullet.rigidbody.AddForce (muzzles[muzzleSequence].right * Random.Range (-inaccuracy,inaccuracy));
			lastBullet.rigidbody.AddForce (muzzles[muzzleSequence].up * Random.Range (-inaccuracy,inaccuracy));
			BulletScript lastScript = lastBullet.GetComponent<BulletScript>();
			lastScript.armorType = bulletArmorType;
			lastScript.damage = damage;
			lastScript.bulletType = bulletPhysics;
			lastScript.parentUnit = transform;
			lastScript.range = range;
			lastScript.target = target;
			lastScript.speed = bulletSpeed;
			internalAmount--;
		}
		if (muzzles.Length > muzzleSequence+1) {
			muzzleSequence++;
			Debug.Log (fireTime);
			Invoke ("FireSequence",fireTime);
		}else{
			muzzleSequence = 0;
		}*/
	}

	void Reload () {
		reloaded = true;
	}

	Transform GetNearestTaggedObject(GameObject[] type) {
		
		if (type.Length > 0) {
			
			float nearestDistanceSqr = Mathf.Infinity;
			GameObject[] taggedGameObjects = type;
			nearestObj = null;
			
			// loop through each tagged object, remembering nearest one found
			if (taggedGameObjects[0] != null) {
				foreach (GameObject obj in taggedGameObjects) {
					
					Vector3 objectPos = obj.transform.position;
					float distanceSqr = (objectPos - transform.position).sqrMagnitude;

					RaycastHit hit;
					Physics.Raycast(model.transform.position,objectPos - model.transform.position,out hit);
					if (distanceSqr < nearestDistanceSqr && hit.transform.gameObject == obj) {
//						Debug.Log (hit.transform.name);
						nearestObj = obj.transform;
						nearestDistanceSqr = distanceSqr;
					}
				}
			}
		}
		return nearestObj;
	}

	void LookRandom () {
		Invoke ("LookRandom",2 + Random.Range (0,3));
			if (!target) {
			randomPos = new Vector3 (transform.position.x + Random.onUnitSphere.x,aimTarget.position.y + Random.Range(-0.2f,0.2f),transform.position.z + Random.onUnitSphere.z);
		}
	}

	void Upgrade (string upgradeType) {
		if (upgradeType == "Sell") {
			Destroy(transform.parent.gameObject);
			stats.credits += fscript.value;
		}
	}
}