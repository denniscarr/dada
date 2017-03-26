using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bookFunction : D_Function {
	public AudioClip[] poems;
	public AudioSource bookSource;
	// Use this for initialization
	new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		bookSource.PlayOneShot (poems [Random.Range (0, 4)]);
	}
}
