using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_chairFunction : D_Function {
	public float chairSpeed = 2000f;
	public Vector3 rotateSpeed;//set the euler angle velocity of this chair
	private Rigidbody rb;
	public float chairTorque = 100f;
	// Use this for initialization
	new void Start () {
		base.Start ();
		rb = GetComponentInParent<Rigidbody> ();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		while (transform.parent.GetComponent<Rigidbody> ().isKinematic == true) {
			transform.parent.SetParent (transform.root);
            GetDropped();
		}

		rb.AddForce(transform.right * chairSpeed);

		Debug.Log("chair thrown");
		Invoke ("Spin", 0.5f);
		Invoke ("Spin", 0.6f);
		Invoke ("Spin", 0.7f);
		Invoke ("Spin", 0.8f);
		Invoke ("Spin", 0.9f);
		Invoke ("Spin", 1f);
	}

	void Spin() {
		Quaternion deltaRotation = Quaternion.Euler (rotateSpeed * Time.deltaTime);
		rb.MoveRotation(rb.rotation * deltaRotation);
		//float turn = Input.GetAxis ("Horizontal");
		//rb.AddTorque (transform.up * chairTorque * turn);
		print ("torque applied");
		//transform.Rotate (Vector3.right * Time.deltaTime * 3);
	}
}
