using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_tallLampFunction : D_Function {
	public AudioClip[] insults;

	// Use this for initialization
	new void Start () {
		base.Start ();
		base.useSFX = insults;
	}
	
	// Update is called once per frame
	public override void Use () {
		
		base.Use ();

	}
}
