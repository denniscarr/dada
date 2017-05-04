using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_drawerFunction : D_Function {
	public GameObject steam;
	// Use this for initialization
	new void Start () {
		base.Start();
	}

	// Update is called once per frame
	public override void Use () {
		base.Use ();
		Vector3 pos = transform.position;
		Instantiate (steam, pos, Quaternion.identity);
	}
}
