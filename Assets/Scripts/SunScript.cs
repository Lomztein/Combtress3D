using UnityEngine;
using System.Collections;

public class SunScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (transform.rotation.x);
		transform.Rotate (1f * Time.deltaTime,0,0);
	}
}
