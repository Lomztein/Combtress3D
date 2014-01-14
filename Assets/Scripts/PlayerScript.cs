using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	
	StatsManager stats;
	HealthScript health;
	RaycastHit hit;
	Vector3 aimHitPoint;
	public int weaponIndex = 0;
	public GameObject newEquip;
	public GameObject equip;
	private Transform weaponPos;
	public GameObject[] turrets;
	public GameObject[] units;
	public GameObject[] weapons;
	public int[] weaponBought;

	// Use this for initialization
	void Start () {
		stats = GameObject.FindGameObjectWithTag("Stats").GetComponent<StatsManager>();
		health = GetComponent<HealthScript>();
		weaponPos = Camera.main.transform.FindChild ("WeaponPos");
		SpawnEquipment();

		weaponBought = new int[weapons.Length];
	}

	void SpawnEquipment () {
		if (equip) {
			Destroy(equip);
			equip = null;
		}
		equip = (GameObject)Instantiate(newEquip,weaponPos.position,weaponPos.rotation);
		equip.transform.parent = Camera.main.transform;
		newEquip = null;
	}

	void OnGUI () {
		GUI.Box (new Rect(Screen.width/2 - 10,Screen.height/2 - 10,20,20),Mathf.Round (stats.fieldOfView).ToString());
		if (stats.debugMode == true) {
			GUI.Label (new Rect(10,10,Screen.width,20),"CREDITS: "+stats.credits);
			GUI.Label (new Rect(10,30,Screen.width,20),"WAVE: "+stats.wave);
			GUI.Label (new Rect(10,50,Screen.width,20),"HEALTH: "+health.hull);
			GUI.Label (new Rect(10,70,Screen.width,20),"WEAPON: "+equip.name);
			GUI.Label (new Rect(10,90,Screen.width,20),"TURRET: "+turrets[0].name);
			GUI.Label (new Rect(10,110,Screen.width,20),"WAVE STARTED: "+stats.waveStarted.ToString ());
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity)) {
			Debug.DrawRay (Camera.main.transform.position, Camera.main.transform.forward * Mathf.Infinity);
			aimHitPoint = hit.point;
		}

		if (Input.GetButtonDown ("Fire2")) {
			if (stats.credits >= turrets[0].GetComponent<FreindlyScript>().value) {
				GameObject newTurret = (GameObject)Instantiate(turrets[Random.Range (0,turrets.Length)],aimHitPoint,Quaternion.identity);
				Debug.Log (newTurret.name);
			}
		}
		if (Input.GetButton ("Fire1")) {
			equip.SendMessage("Fire");
		}

		if (Input.GetButtonDown("ChangeWeapon")) {
			ChangeWeapon (Input.GetButtonDown("ChangeWeapon"));
		}
	}

	void ChangeWeapon (bool value) {
	//
	}


	void OnDrawGizmos () {
		Gizmos.DrawSphere (aimHitPoint,0.5f);
		Gizmos.DrawLine (Camera.main.transform.position,aimHitPoint);
	}
}
