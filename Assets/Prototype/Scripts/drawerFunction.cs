using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawerFunction : MonoBehaviour {
	public GameObject drawer;
	public GameObject steam;
	public KeyCode useDrawer = KeyCode.Mouse0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (drawer.transform.parent != null && Input.GetKey (useDrawer)) {
			Instantiate (steam, transform.position, Quaternion.identity);

		}
	}
}
