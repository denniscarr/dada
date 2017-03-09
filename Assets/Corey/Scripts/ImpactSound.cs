using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSound : MonoBehaviour {

	public AudioClip collisionSound;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision other) {

		CS_AudioManager.Instance.Play3DSFX (collisionSound, transform.position);

	}
}
