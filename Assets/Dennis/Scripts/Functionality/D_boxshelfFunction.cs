using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_boxshelfFunction : D_Function {
	public AudioClip[] greeters;
	public AudioSource boxshelfAudio; 
	// Use this for initialization
	new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		boxshelfAudio.PlayOneShot(greeters[Random.Range(0, 2)]);
	}
}
