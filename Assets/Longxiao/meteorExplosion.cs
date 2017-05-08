using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteorExplosion : MonoBehaviour {
	public GameObject[] explosion;
	private float explosionScale;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision collision) {
		explosionScale = Random.Range (10, 15);
		GameObject impact = Instantiate (explosion [Random.Range (0, explosion.Length)], transform.position, Quaternion.identity) as GameObject;
		//impact.transform.localScale = new Vector3 (explosionScale, explosionScale, explosionScale);
		ParticleSystem impactParticle = impact.GetComponent<ParticleSystem>();
		var impactPSMA = impactParticle.main;
		//impactParticle.main.startSizeMultiplier = impactParticle.main.startSizeMultiplier * explosionScale;
		impactPSMA.startSizeMultiplier = impactPSMA.startSizeMultiplier*explosionScale;
		if (collision.gameObject.GetComponentInChildren<InteractionSettings>() != null)
		{
			collision.gameObject.GetComponentInChildren<InteractionSettings>().heat += 1f;
		}
	}

}
