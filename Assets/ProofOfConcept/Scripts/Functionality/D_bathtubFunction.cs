using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_bathtubFunction : D_Function {
	public float bathtubForce = 500f;
	public float bathtubSpeed = 1000f;
	// Use this for initialization
	new void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use();
		while (transform.parent.GetComponent<Rigidbody> ().isKinematic == true) {
			transform.parent.SetParent (null);
            GetDropped();
		}
		transform.parent.position = LOWER_EQUIP_REFERENCE.position + intSet.equipPosition;
		transform.parent.rotation = Quaternion.LookRotation(GameObject.Find("Player").transform.forward);
		GetComponentInParent<Rigidbody>().AddForce(transform.right * bathtubSpeed);
		Invoke ("vibrate", 3f);
	}

	void vibrate() {
		GetComponentInParent<Rigidbody> ().AddForce (transform.up * bathtubForce);
		Invoke ("vibrate2", 0.3f);
	}

	void vibrate2() {
		GetComponentInParent<Rigidbody> ().AddForce (-transform.right * bathtubForce);
		Invoke ("vibrate3", 0.3f);
	}

	void vibrate3() {
		GetComponentInParent<Rigidbody> ().AddForce (-transform.forward * bathtubForce);
	}
}
