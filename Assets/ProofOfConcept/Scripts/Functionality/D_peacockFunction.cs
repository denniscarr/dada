using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_peacockFunction : D_Function {
	
	new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();

        foreach(Collider collider in Physics.OverlapSphere(transform.parent.position, 10f))
        {
            if (collider.GetComponent<MyFirstPersonController>() != null)
            {
                collider.GetComponent<MyFirstPersonController>().ForcedJump(35f);
            }

            else if (collider.GetComponent<Rigidbody>() != null)
            {
                collider.GetComponent<Rigidbody>().AddForce(Vector3.up * 100f, ForceMode.Impulse);
            }
        }
	}
}
