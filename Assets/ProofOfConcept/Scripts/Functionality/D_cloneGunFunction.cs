using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_cloneGunFunction : D_Function {
	private LineRenderer line;
	// Use this for initialization
	new void Start () {
		base.Start ();
		line = gameObject.GetComponent<LineRenderer> ();
		line.enabled = false;
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		line.enabled = true;

		Ray cloneRay = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		line.SetPosition (0, cloneRay.origin);

		if (Physics.Raycast (cloneRay, out hit, 100)) {
			GameObject cloneHit = hit.collider.gameObject;
			Instantiate (cloneHit, hit.transform.position, Quaternion.identity);
			line.SetPosition (1, hit.point);
		} else {
			line.SetPosition (1, cloneRay.GetPoint (100));
		}

		Invoke ("BeamOff", 0.1f);
	}

	 void BeamOff() {
		line.enabled = false;
	}
}
