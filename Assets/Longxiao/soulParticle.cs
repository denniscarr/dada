using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soulParticle : MonoBehaviour {
	private ParticleSystem particleOfSoul;
	private List<ParticleCollisionEvent> collisionEvents;
	public GameObject soul;
	// Use this for initialization
	void Start () {
		particleOfSoul = GetComponent<ParticleSystem> ();
		collisionEvents = new List<ParticleCollisionEvent> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnParticleCollision (GameObject futureNPC) {
		//int numCollisionEvents = particleOfSoul.GetCollisionEvents (futureNPC, collisionEvents);
		//int i = 0;
		//while (i < numCollisionEvents) {
		//	print (futureNPC.name);
		//}
		if (futureNPC.name != "GROUND" && futureNPC.GetComponentInChildren<NPC>() == null) {
			print (futureNPC.name +" needs a soul");
			Instantiate (soul, futureNPC.transform.position, Quaternion.identity, futureNPC.transform);
//			soul.transform.parent = futureNPC.transform;
		}

	}
}
