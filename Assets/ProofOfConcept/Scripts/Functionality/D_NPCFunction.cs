using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_NPCFunction : D_Function {
	public AudioClip[] scream;
	// Use this for initialization
	new void Start () {
		base.Start();
		base.useSFX = scream;
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use();
	}
}
