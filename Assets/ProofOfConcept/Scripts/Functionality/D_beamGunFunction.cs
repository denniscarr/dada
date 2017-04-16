using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_beamGunFunction : D_Function {
	LineRenderer line;
	public GameObject spark;
	// Use this for initialization
	new void Start () {
		base.Start ();
		line = gameObject.GetComponent<LineRenderer> ();
		line.enabled = false;
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use();
		line.enabled = true;
		Ray beamRay = new Ray (transform.position, -transform.forward);
		RaycastHit Hit;
		line.SetPosition (0, beamRay.origin);
		if (Physics.Raycast(beamRay, out Hit, 100))
			line.SetPosition(1, Hit.point);
		else line.SetPosition (1, beamRay.GetPoint (100));
		Instantiate (spark, Hit.point, Quaternion.identity);

		Invoke ("BeamOff", 0.1f);
	}

	public void BeamOff() {
		line.enabled = false;
	}
	/*IEnumerator FireLaser(){
		line.enabled = true;
	}*/
}
