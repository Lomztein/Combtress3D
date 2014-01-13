using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	
	public GameObject[] enemyTypes;
	public GameObject[] bossTypes;
	private StatsManager stats;
	public float[] spawnFrequency;

	// Use this for initialization
	void Start () {
		stats = GameObject.FindGameObjectWithTag("Stats").GetComponent<StatsManager>();
		NextWave ();
		spawnFrequency = new float[enemyTypes.Length];

		int index = 0;
		foreach (float frequency in spawnFrequency) {
			spawnFrequency[index] = 200;
			index++;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (stats.waveStarted == true) {
			int enemyNumber = -1;
			foreach (GameObject newEnemy in enemyTypes) {
				enemyNumber++;
				if (enemyNumber * 5 <= stats.wave) {
					if (Mathf.Round (Random.Range (0,spawnFrequency[enemyNumber])) == 1) {
						Instantiate(newEnemy,new Vector3(Random.onUnitSphere.x * 800, transform.position.y,Random.onUnitSphere.z * 800),Quaternion.identity);
					}
				}
			}
		}
	}

	void EndWave () {
		stats.waveStarted = false;
		Invoke ("NextWave",5);
	}

	void NextWave () {
		stats.waveStarted = true;
		Invoke ("EndWave",60);
		stats.wave++;
		//Debug.Log ("New wave: "+stats.wave.ToString());
		int index = -1;
		foreach (float frequency in spawnFrequency) {
			index++;
			if (index * 5 <= stats.wave && frequency > 25) {
				spawnFrequency[index] -= 1 * stats.difficulty;
			}
		}
	}

	void OnGUI () {
		if (stats.debugMode == true) {
			int index = 0;
			foreach (float frequency in spawnFrequency) {
				GUI.Label( new Rect (100, 10 + index * 20,Screen.width, 20),enemyTypes[index].name +":"+frequency.ToString());
				index++;
			}
		}
	}
}
