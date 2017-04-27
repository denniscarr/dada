using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_smallTableFunction : D_Function {
	private Transform smallTable;
	public float torque = 100;
	private Rigidbody rb;
	// Use this for initialization
	new void Start () {
		base.Start ();
		rb = GetComponentInParent<Rigidbody> ();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();

		rb.AddTorque (transform.up * torque);
		Invoke ("torque1", 0.2f);
		Transform smallTable = GetComponentInParent<Transform> ();
		smallTable.transform.Rotate (Vector3.up, 10*Time.deltaTime);
		print ("table function triggered");
	}

	void torque1 () {
		rb.AddTorque (transform.up * torque);
		Invoke ("torque2", 0.2f);
	}

	void torque2 () {
		rb.AddTorque (transform.up * torque);
		Invoke ("torque3", 0.2f);
	}

	void torque3 () {
		rb.AddTorque (transform.up * torque);
		Invoke ("torque4", 0.2f);
	}

	void torque4 () {
		rb.AddTorque (transform.up * torque);
		Invoke ("torque5", 0.2f);
	}

	void torque5 () {
		rb.AddTorque (transform.up * torque);
		Invoke ("torque6", 0.2f);
	}

	void torque6 () {
		rb.AddTorque (transform.up * torque);
	}
}
