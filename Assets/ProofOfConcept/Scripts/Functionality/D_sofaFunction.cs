using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_sofaFunction : D_Function {
	public AudioClip fart;
	public AudioSource sofaAudio;
	// Use this for initialization
	new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		sofaAudio.PlayOneShot(fart);
	}
}
