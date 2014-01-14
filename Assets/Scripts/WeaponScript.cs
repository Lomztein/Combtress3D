using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

//	public float weight;
	public bool pickedUp;
	public float range = 100;
	public float firerate;
	public float reloadSpeed;
	public GameObject bulletType;
	public float bulletSpeed = 2000;
	public int bulletAmount = 1;
	public string bulletArmorType;
	public float damage;
	public Transform[] muzzles;
	public bool reloaded = true;
	public float inaccuracy;
	public GameObject fireParticle;
	private Transform model;
	public string fireMode = "Instant";
//	public float sequenceTime;
	public string bulletPhysics = "Direct";
	private StatsManager stats;
//	private int muzzleSequence = 0;
//	private float fireTime = 0;
	BoxCollider[] colShape;
	private Vector3 startingPos;
	private Transform parentUnit;
	public string faction;
	public bool aimingSight;
	public Vector3 aimOffset;
	public Vector3 targetPos;

	// Use this for initialization
	void Start () {

		parentUnit = transform.parent.parent;
		startingPos = transform.position - parentUnit.position;
	
		colShape = GetComponents<BoxCollider>();
		if (pickedUp == false) {
			rigidbody.isKinematic = false;
			foreach (BoxCollider box in colShape) {
				box.enabled = true;
			}
		}else{
			if (transform.parent.tag == "Freindly" || transform.parent.tag == "MainCamera") {
				faction = "Freindly";
			}else{
				faction = "Enemy";
			}
		}

		model = transform.FindChild("Model");
		int muzzleNumber = 0;
		muzzles = new Transform[model.childCount];
		foreach (Transform child in model.transform) {
			muzzles[muzzleNumber] = child;
			muzzleNumber++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (aimingSight == true) {
			Vector3.Lerp (transform.position,parentUnit.position + aimOffset,10 * Time.deltaTime);
		}else{
			Vector3.Lerp (transform.position,parentUnit.position + startingPos,10 * Time.deltaTime);
		}
	}

	public void Fire () {
		if (fireMode == "Instant") {
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

	void Reload () {
		reloaded = true;
	}
}
