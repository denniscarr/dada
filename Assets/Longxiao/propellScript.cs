using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class propellScript : MonoBehaviour {
    public Transform ahahaha;
    public KeyCode throwKey = KeyCode.G;
    public int speed = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(throwKey))
        {
            //apply gravity once the object is being thrown
            ahahaha.GetChild(0).gameObject.GetComponent<Rigidbody>().useGravity = true;
            // apply force to the object being thrown
            ahahaha.GetChild(0).gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
            //unparent the object being thrown
            ahahaha.GetChild(0).parent = null;
        }
	}
}
