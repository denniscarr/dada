using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class D_longxiaoGunFunction : D_Function {
	private LineRenderer line;
	private Vector3 zero;
	public AudioClip shrinkSound;
	// Use this for initialization
	new void Start () {
		base.Start ();
		line = gameObject.GetComponent<LineRenderer> ();
		line.enabled = false;
		zero = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		line.enabled = true;
		Vector3 pos = transform.position;
		Ray eliminationRay = new Ray (pos, transform.forward);
		Debug.DrawRay (eliminationRay.origin, eliminationRay.direction * 10f, Color.cyan);
		RaycastHit Hit;
		line.SetPosition (0, eliminationRay.origin);

		if (Physics.Raycast (eliminationRay, out Hit, 100f)) {
			line.SetPosition (1, Hit.point);
			Services.AudioManager.Play3DSFX (shrinkSound, Hit.point, 1f, 1f);
			Hit.transform.DOScale(zero, 1f);
			Destroy (Hit.transform.gameObject, 5f);
		}
		else {
			line.SetPosition(1, eliminationRay.GetPoint(100));	
		} 
		Invoke ("BeamOff", 0.1f);
	}

	public void BeamOff() {
		line.enabled = false;
	}


}
