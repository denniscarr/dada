using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class D_brownMushroomFunction : D_Function {

	// Use this for initialization
	new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Use () {
		transform.parent.parent.parent.GetComponent <DepthOfField>().enabled = !transform.parent.parent.parent.GetComponent <DepthOfField>().enabled;
	}
}
