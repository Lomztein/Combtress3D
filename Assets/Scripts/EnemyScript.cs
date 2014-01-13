using UnityEngine;
using System.Collections;
[RequireComponent(typeof(HealthScript))]

public class EnemyScript : MonoBehaviour {

	private StatsManager stats;
	public string faction = "Enemy";
	public float value;

	void Start () {
		stats = GameObject.FindGameObjectWithTag("Stats").GetComponent<StatsManager>();
	}

	void OnDestroy () {
		stats.credits += value;
	}
}