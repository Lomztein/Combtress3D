using UnityEngine;
using System.Collections;

public class StatsManager : MonoBehaviour {

	public float credits;
	public int wave;
	public int difficulty;
	public bool debugMode;
	public float fieldOfView;
	public bool waveStarted;

	private GameObject player;
	public GameObject[] freindlies;
	
	public float[] consumedPower;
	public float[] maxPower;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		FindFreindlies();
		Invoke ("FindFreindlies",5);
	}
	
	// Update is called once per frame
	void Update () {

		if (!player || Input.GetButton ("Reload")) {
			Application.LoadLevel ("c3d_main_battleground");
		}

		fieldOfView += Input.GetAxis ("Mouse ScrollWheel");

		Camera.main.fieldOfView = fieldOfView;
		if (Input.GetButtonDown ("ToggleDebug")) {
			if (debugMode == false) {
				debugMode = true;
			}else{
				debugMode = false;
			}
		}
	}

	void FindFreindlies () {
		Invoke ("FindFreindlies",5);
		GameObject[] newObjects = GameObject.FindGameObjectsWithTag("Freindly");
		freindlies = new GameObject[newObjects.Length];
		int index = 0;
		foreach (GameObject newObj in newObjects) {
			freindlies[index] = newObj;
			index++;
		}
	}
}
