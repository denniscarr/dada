using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_peacockFunction : D_Function {
	public AudioSource peacockSource;
	public AudioClip peacockClip;
	// Use this for initialization
	new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		peacockSource.PlayOneShot (peacockClip);
	}
}
