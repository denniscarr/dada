using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class D_wallMirrorFunction : D_Function {

	// Use this for initialization
	new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		print ("Wall mirror function triggered");
		transform.parent.parent.parent.GetComponent <Twirl>().enabled = !transform.parent.parent.parent.GetComponent <Twirl>().enabled;
	}
}
