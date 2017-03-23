﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_starryExpolsion : D_Function {
	public GameObject bathSink;
	public GameObject explosionParticle;
	public float radius = 5.0F;
	public float power = 50.0F;
	public float fuseTime = 5f;
	public float bathSinkSpeed = 100f;
	public KeyCode useBathSink = KeyCode.Mouse0;
	// Use this for initialization
	new void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use();
		while (transform.parent.GetComponent<Rigidbody> ().isKinematic == true) {
			transform.parent.SetParent (null);
            transform.parent.BroadcastMessage("abandonItem", SendMessageOptions.DontRequireReceiver);
		}
		GetComponentInParent<Rigidbody>().AddForce(transform.right * bathSinkSpeed);
		Invoke("Explosion", fuseTime);
	}

	void Explosion()
	{

		//Instantiate particle system and add force
		Instantiate (explosionParticle, transform.position, Quaternion.identity);
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere (explosionPos, radius);
		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody> ();

			if (rb != null)
				rb.AddExplosionForce (power, explosionPos, radius, 3.0F);

		}
	}
}
