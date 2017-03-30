using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_toiletFunction : D_Function {
	private AudioSource toiletSource;
	public AudioClip toiletClip;
	// Use this for initialization
	new void Start () {
		base.Start ();
		toiletSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		toiletSource.PlayOneShot (toiletClip);
	}
}
