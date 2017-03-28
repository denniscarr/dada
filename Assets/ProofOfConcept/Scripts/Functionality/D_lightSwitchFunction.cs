using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_lightSwitchFunction : D_Function {
	private bool lampSwitch = false;
	private Light lampLight; 
	// Use this for initialization
	new void Start () {
		base.Start ();
		lampSwitch = false;
		lampLight = transform.GetComponentInParent<Light>();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use();

		lampLight.enabled = !lampLight.enabled;
	}
}
