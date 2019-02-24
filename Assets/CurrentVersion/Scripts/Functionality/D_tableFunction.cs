using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_tableFunction : D_Function {
	public GameObject[] items;
	// Use this for initialization
	new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		Instantiate (items [Random.Range (0, 22)], transform.position, Quaternion.identity);
		print ("table spawn");
	}
}
