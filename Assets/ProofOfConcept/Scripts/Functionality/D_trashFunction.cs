using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_trashFunction : D_Function {
	public GameObject confetti;
	public AudioClip confettiClip;
	private AudioSource confettiSource;
	// Use this for initialization
	new void Start () {
		base.Start ();
		confettiSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		Instantiate (confetti, transform.position, Quaternion.identity);
		confettiSource.PlayOneShot (confettiClip);
	}
}
