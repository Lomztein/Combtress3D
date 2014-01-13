using UnityEngine;
using System.Collections;

public class CrateScript : MonoBehaviour {

	public float creditsValue;
	public float healthValue;
	StatsManager stats;
	HealthScript health;

	void Start () {
		stats = GameObject.FindGameObjectWithTag("Stats").GetComponent<StatsManager>();
	}

	void OnCollisionStay (Collision col) {
		Debug.Log (col.gameObject.tag);
		if (col.gameObject.tag == "Player") {
			Destroy(gameObject);
			health = col.gameObject.GetComponent<HealthScript>();
			if (health != null) {
				if (health.hull != health.maxHull) {
					if (healthValue != 0) {
						health.hull += health.maxHull * 0.25f;
					}else{
						health.hull += healthValue;
					}
				}else{
					if (creditsValue != 0) {
						stats.credits += stats.wave * 25;
					}else{
						stats.credits += creditsValue;
					}
				}
			}
		}
	}
}