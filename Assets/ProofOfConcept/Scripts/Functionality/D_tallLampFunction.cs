using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_tallLampFunction : D_Function {
	public AudioClip[] insults;
	public AudioSource tallLampAudio;
	// Use this for initialization
	new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		tallLampAudio.PlayOneShot(insults[Random.Range(0, 2)]);
	}
}
