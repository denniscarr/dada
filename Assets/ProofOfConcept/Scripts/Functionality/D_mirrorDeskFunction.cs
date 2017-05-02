using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class D_mirrorDeskFunction : D_Function {
	private InteractionSettings[] items;
	private LineRenderer pullBeam;
	// Use this for initialization
	new void Start () {
		base.Start ();
		items = GameObject.FindObjectsOfType<InteractionSettings> ();
		pullBeam = gameObject.GetComponent<LineRenderer> ();
		pullBeam.enabled = false;
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		Transform summoning = items [Random.Range (0, items.Length)].transform.parent;
		print (summoning);
		pullBeam.enabled = true;
		pullBeam.SetPosition (0, transform.position);

		if (summoning.GetComponent<Rigidbody> () != null) {
			summoning.DOLocalMove (gameObject.transform.position, 2f);
			pullBeam.SetPosition (1, summoning.localPosition);
		}
	}
}
