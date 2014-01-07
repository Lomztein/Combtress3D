using UnityEngine;
using System.Collections;

public class ParticleScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("Kill",0.05f);
	}

	void Kill () {
		Destroy(gameObject);
	}
}