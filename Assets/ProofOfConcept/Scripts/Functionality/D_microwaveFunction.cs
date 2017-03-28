using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_microwaveFunction : D_Function {
	//public GameObject ringWave;
	private ParticleSystem ring;
	//public Transform emitter;
	// Use this for initialization
	new void Start () {
		base.Start ();
		ring = GetComponent<ParticleSystem> ();
		var em = ring.emission;
		em.enabled = false;
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		//Instantiate (ringWave, emitter.transform.right, Quaternion.identity);
		var em = ring.emission;
		em.enabled = !em.enabled;
	}
}
