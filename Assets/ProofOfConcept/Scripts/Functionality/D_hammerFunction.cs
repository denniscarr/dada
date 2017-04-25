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
		Vector3 pos = LOWER_EQUIP_REFERENCE.position + intSet.equipPosition + transform.localPosition;
		Ray hammerRay = new Ray (pos, t_player.forward);
		Debug.DrawRay (hammerRay.origin, hammerRay.direction * 10f, Color.red);
		RaycastHit hit;
		if (Physics.Raycast(hammerRay, out hit, 10))
		{
			GameObject hammerHit = hit.collider.gameObject;
            if (hammerHit.name == "GROUND") return;
			hammerHit.transform.localScale -= new Vector3 (-flattenMultiplier, flattenMultiplier, -flattenMultiplier);
			//hammerHit.transform.DOScale -= new Vector3 (-flattenMultiplier, flattenMultiplier, -flattenMultiplier);
		}
		//GetComponentInParent<Animation> ().Play();
		//hammer.SetBool("Hammer", true);
	}
}
