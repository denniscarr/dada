using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soulParticle : MonoBehaviour {
	private ParticleSystem particleOfSoul;
	private List<ParticleCollisionEvent> collisionEvents;
	// Use this for initialization
	void Start () {
		particleOfSoul = GetComponent<ParticleSystem> ();
		collisionEvents = new List<ParticleCollisionEvent> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnParticleCollision (GameObject futureNPC) {
		int numCollisionEvents = particleOfSoul.GetCollisionEvents (futureNPC, collisionEvents);
		int i = 0;
		while (i < numCollisionEvents) {
			print (futureNPC.name);
		}

	}
}
