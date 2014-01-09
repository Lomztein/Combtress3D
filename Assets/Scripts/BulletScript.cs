using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
	
	public float damage;
	public string armorType;
	public float life;
	public GameObject hitParticle;
	HealthScript colHealth;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (life > 0) {
			life -= Time.deltaTime;
		}else{
			Destroy (gameObject);
		}
	}
	void OnCollisionEnter(Collision col) {
		Destroy(gameObject);
		colHealth = col.gameObject.GetComponent<HealthScript>();
		if (colHealth) {
			Debug.Log ("Bullet hit "+col.gameObject.name+" with "+colHealth.armorType+" armor, dealing "+damage+" "+armorType+" damage.");
			if (colHealth.armorType == armorType) {
				colHealth.hull -= damage;
			}else{
				colHealth.hull -= damage/5;
			}
		}
	}
}
