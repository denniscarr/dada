using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_trashFunction : D_Function {
	public GameObject confetti;

	// Use this for initialization
	new void Start () {
		base.Start ();

	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		Instantiate (confetti, transform.position, Quaternion.identity);
	
	}
}
