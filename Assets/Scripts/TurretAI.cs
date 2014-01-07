using UnityEngine;
using System.Collections;

public class TurretAI : MonoBehaviour {

	public float range;
	public float reloadSpeed;
	public Transform target;
	public GameObject bulletType;
	public float bulletSpeed;
	public int bulletAmount;
	private int internalAmount;
	private GameObject closest;
	public GameObject stand;
	public GameObject standPrefab;
	public string armorType;
	public string bulletArmorType;
	public float damage;
	public Vector3 weaponOffset;
	public GameObject player;
	public Transform muzzle;
	public bool reloaded = true;
	public float inaccuracy;
	public GameObject fireParticle;

	void Start () {
		GetNearestTaggedObject ();
		stand = (GameObject)Instantiate(standPrefab,transform.position,Quaternion.identity);
		transform.parent = stand.transform;
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

	Transform GetNearestTaggedObject() {
		
		float nearestDistanceSqr = Mathf.Infinity;
		GameObject[] taggedGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
		Transform nearestObj = null;
		
		// loop through each tagged object, remembering nearest one found
		foreach (GameObject obj in taggedGameObjects) {
			
			Vector3 objectPos = obj.transform.position;
			float distanceSqr = (objectPos - transform.position).sqrMagnitude;
			
			if (distanceSqr < nearestDistanceSqr) {
				nearestObj = obj.transform;
				nearestDistanceSqr = distanceSqr;
			}
		}
		
		return nearestObj;
	}

	void Update () {
		if (target == null) {
			target = GetNearestTaggedObject ();
		}else{
			if (Vector3.Distance (transform.position,target.position) > range) {
				target = GetNearestTaggedObject ();
			}

			transform.LookAt(target.transform.position);
		}
		//if (Vector3.Distance (transform.position,target.position) < range) {
			if (reloaded == true) {
				GameObject lastBullet = (GameObject)Instantiate(bulletType,muzzle.position,muzzle.rotation);
				Instantiate (fireParticle,muzzle.position,muzzle.rotation);
				lastBullet.rigidbody.AddForce(muzzle.forward * bulletSpeed);
				lastBullet.rigidbody.AddForce (muzzle.right * Random.Range (-inaccuracy,inaccuracy));
				lastBullet.rigidbody.AddForce (muzzle.up * Random.Range (-inaccuracy,inaccuracy));
				reloaded = false;
				Invoke ("Reload",reloadSpeed);
		}
		//}
	}
	void Reload () {
		reloaded = true;
	}
}