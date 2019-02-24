using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_starryExpolsion : D_Function {

	//public GameObject bathSink;
	public GameObject[] explosionParticle;
	public AudioClip explosionSound;
	public float radius = 5.0F;
	public float power = 50.0F;
	public float fuseTime = 5f;
	public float bathSinkSpeed = 100f;
	public KeyCode useBathSink = KeyCode.Mouse0;
	public float fragmentCount = 10f;
	// Use this for initialization
	new void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use();

		while (transform.parent.GetComponent<Rigidbody> ().isKinematic == true) {
			transform.parent.SetParent (null);
			GetDropped();
		}
		GetComponentInParent<Rigidbody> ().velocity = transform.forward * bathSinkSpeed;
		Invoke("Explosion", fuseTime);
	}

	public void Explosion()
	{
        transform.parent.BroadcastMessage("Use");

		//Instantiate particle system and add force
		for (int i=0; i<fragmentCount; i++){
			Instantiate (explosionParticle[Random.Range(0, explosionParticle.Length)], transform.position, Quaternion.identity);}
		Services.AudioManager.Play3DSFX (explosionSound, transform.position, 1f, 1f);
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere (explosionPos, radius);
		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody> ();

			if (rb != null)
				rb.AddExplosionForce (power, explosionPos, radius, 3.0F);

		}
	}
}
