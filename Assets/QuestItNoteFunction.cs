using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItNoteFunction : D_Function {

	// Use this for initialization
	new void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	new void Update () {

		base.Update ();

		Quaternion newRotation = Quaternion.Euler (new Vector3 (0.0f, Services.Player.transform.rotation.eulerAngles.y, Services.Player.transform.rotation.eulerAngles.z));
		Debug.Log (newRotation.eulerAngles);
		transform.parent.rotation = newRotation;
	}

	public override void Use ()
	{
		base.Use ();
		Debug.Log ("used");
		if (transform.parent.parent.name == "UpperNode") {
			Services.Player.transform.parent.BroadcastMessage ("StopHoldingItemInMouse");
		} else {
			transform.parent.parent = null;
		}
		GetComponentInParent<Rigidbody> ().isKinematic = !GetComponentInParent<Rigidbody> ().isKinematic;
	}
}
