using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

	public string armorType;
	public float hull;
	public float maxHull;
	public float maxRegen;
	public float regenSpeed;
	public GameObject debris;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (hull <= 0) {
			Destroy(gameObject);
		}
	}
}
