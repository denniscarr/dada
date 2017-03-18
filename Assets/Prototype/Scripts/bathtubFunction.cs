using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bathtubFunction : MonoBehaviour {
	public GameObject bathtub;
	public float bathtubForce = 500f;
	public float bathtubSpeed = 1000f;
	public KeyCode useBathtub = KeyCode.Mouse0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (bathtub.transform.parent != null && Input.GetKeyDown (useBathtub)) {
			GetComponent<Collider>().enabled = true;
			GetComponent<Rigidbody>().useGravity = true;
			GetComponent<Rigidbody>().AddForce(transform.right * bathtubSpeed);
			transform.SetParent(null);
			Invoke ("vibrate", 3f);
		}
	}

	void vibrate() {
		GetComponent<Rigidbody> ().AddForce (transform.up * bathtubForce);
		GetComponent<Rigidbody> ().AddForce (-transform.up * bathtubForce);
	}
}
