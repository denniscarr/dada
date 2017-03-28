using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_bedFunction : D_Function {
	public GameObject dweller; 
	// Use this for initialization
	new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		Instantiate (dweller, transform.position, Quaternion.identity);
	}
}
