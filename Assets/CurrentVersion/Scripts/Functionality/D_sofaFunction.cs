using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_sofaFunction : D_Function {
	public AudioClip fart;

	//public AudioSource sofaAudio;
	// Use this for initialization
	new void Start ()
    {
        base.Start();
        base.useSFX = new AudioClip[1];
        base.useSFX[0] = fart;
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
	}
}
