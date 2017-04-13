using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_TVFunction : D_Function {
	private float timeSpeed;
	// Use this for initialization
	new void Start () {
		base.Start ();
		timeSpeed = Random.Range (0.1f, 10f);
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		Time.timeScale = timeSpeed;
	}
}
