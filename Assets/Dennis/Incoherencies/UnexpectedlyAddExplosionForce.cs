using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnexpectedlyAddExplosionForce : Incoherence {

	Rigidbody rb;

	// The force applied for each explosion.
	public float forceMin;
	public float forceMax;

	// The radius of each explosion.
	public float rangeMin;
	public float rangeMax;


	void Start () {
		base.Start();
		rb = transform.root.GetComponentInChildren<Rigidbody>();
	}


	public override void ExpressIncoherence() {
		base.ExpressIncoherence();
		print("Expressed (Sub)");
		rb.AddExplosionForce(MapIncoherence(forceMin, forceMax), transform.position, MapIncoherence(rangeMin, rangeMax));
	}
}
