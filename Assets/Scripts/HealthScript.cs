using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

	public string armorType;
	public float hull;
	public float maxHull;
	public float maxRegen;
	public float regenSpeed;
	public GameObject debris;
	public bool invinsible;

	// Update is called once per frame
	void Update () {
		if (invinsible == true) {
			hull = maxHull;
		}

		if (hull <= 0) {
			if (invinsible == false) {
				Destroy(gameObject);
				if (debris) {
					Instantiate(debris,transform.position,Quaternion.identity);
				}
			}
		}

		if (hull > maxRegen) {
			hull += regenSpeed * Time.deltaTime;
		}
	}
}
