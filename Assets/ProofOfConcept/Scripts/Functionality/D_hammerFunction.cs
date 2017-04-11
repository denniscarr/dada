using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_hammerFunction : D_Function {
	//Animator hammer;
	// Use this for initialization
	new void Start () {
		base.Start ();
		//hammer = GetComponentInParent<Animator> ();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		//GetComponentInParent<Animation> ().Play();
		//hammer.SetBool("Hammer", true);
		print ("animation triggered");

	}
}
