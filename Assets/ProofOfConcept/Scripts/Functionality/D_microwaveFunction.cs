using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_microwaveFunction : D_Function {
	public GameObject ringWave;
	public Transform emitter;
	// Use this for initialization
	new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		Instantiate (ringWave, emitter.transform.right, Quaternion.identity);
	}
}
