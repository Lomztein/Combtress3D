using UnityEngine;
using System.Collections;

public class ParticleScript : MonoBehaviour {

	public float life = 0.05f;

	// Use this for initialization
	void Start () {
		Invoke ("Kill",life);
	}

	void Kill () {
		Destroy(gameObject);
	}
}