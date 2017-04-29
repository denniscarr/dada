using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_goheiFunction : D_Function {
	public GameObject[] pelletPrefab;
	public int pelletCount = 8;
	public float pelletVelocity = 150f;
	public float spreadFactor = 0.1f;
	// Use this for initialization
	new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		for (int i = 0; i < pelletCount; i++) {
			Quaternion pelletRotation = transform.rotation;
			pelletRotation.x += Random.Range (-spreadFactor, spreadFactor);
			pelletRotation.y += Random.Range (-spreadFactor, spreadFactor);
			pelletRotation.z += Random.Range (-spreadFactor, spreadFactor);
			GameObject pellet = Instantiate (pelletPrefab [Random.Range (0, pelletPrefab.Length - 1)], transform.position, pelletRotation);
			pellet.GetComponent<Rigidbody> ().velocity = transform.forward * pelletVelocity;
		}
	}
}
