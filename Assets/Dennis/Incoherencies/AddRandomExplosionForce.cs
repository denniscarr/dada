using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRandomExplosionForce : Incoherence {

	Rigidbody rigidbody;

	void Start () {
		myController.AddIncoherencyToList(gameObject);

		rigidbody = transform.root.GetComponentInChildren<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
