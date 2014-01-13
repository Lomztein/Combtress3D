using UnityEngine;
using System.Collections;

public class SunScript : MonoBehaviour {

	public float sunSpeed;
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (transform.rotation.x);
		transform.Rotate (sunSpeed * Time.deltaTime,0,0);
	}
}
