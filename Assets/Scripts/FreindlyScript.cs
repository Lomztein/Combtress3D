using UnityEngine;
using System.Collections;
[RequireComponent(typeof(HealthScript))]

public class FreindlyScript : MonoBehaviour {

	public float value;
	public int level;
	public float exp;
	public float rangeUpgradeCount;
	public float damageUpgradeCount;
	public float firerateUpgradeCount;
	public float hullUpgradeCount;
	public string faction = "Freindly";
	StatsManager stats;

	void Start () {

		stats = GameObject.FindGameObjectWithTag("Stats").GetComponent<StatsManager>();
		stats.credits -= value;
	}
}