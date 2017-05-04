using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class D_hammerFunction : D_Function {
	//Animator hammer;
	// Use this for initialization
	public float flattenMultiplier = 10f;

	new void Start () {
		base.Start ();
		//hammer = GetComponentInParent<Animator> ();
		//Vector3 fwd = transform.TransformDirection(Vector3.forward);
	}
	
	// Update is called once per frame
	public override void Use () {
		//Vector3 fwd = transform.TransformDirection(Vector3.forward);
		base.Use ();
		Vector3 pos = transform.position;
		Ray hammerRay = new Ray (pos, transform.forward);
		Debug.DrawRay (hammerRay.origin, hammerRay.direction * 10f, Color.red);
		RaycastHit hit;
		if (Physics.Raycast(hammerRay, out hit, 10))
		{
			GameObject hammerHit = hit.collider.gameObject;
            float originalYScale = hammerHit.transform.localScale.y;
            if (hammerHit.name == "GROUND") return;

            Vector3 newScale = hammerHit.transform.localScale - new Vector3(-flattenMultiplier, flattenMultiplier, -flattenMultiplier);
            if (newScale.y < newScale.x * 0.001f) newScale.y = newScale.x * 0.001f;
            if (hammerHit.GetComponentInChildren<InteractionSettings>() != null) hammerHit.GetComponentInChildren<InteractionSettings>().savedScale = newScale;
            hammerHit.transform.localScale = newScale;
			//hammerHit.transform.DOScale -= new Vector3 (-flattenMultiplier, flattenMultiplier, -flattenMultiplier);
		}
		//GetComponentInParent<Animation> ().Play();
		//hammer.SetBool("Hammer", true);
	}
}
