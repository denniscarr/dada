using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using DG.Tweening;

public class D_brownMushroomFunction : D_Function {

    GameObject playerCamera;
	public float timeUntilRevert;
	float originalAperture;
	bool beingUsed;


    // Use this for initialization
    new void Start()
    {
        base.Start();
		beingUsed = false;
        playerCamera = Services.Player.GetComponentInChildren<Camera>().gameObject;
		originalAperture = playerCamera.GetComponent<DepthOfField> ().aperture;
    }

    // Update is called once per frame
    public override void Use()
    {
		if (!beingUsed) {
			base.Use ();
			if (transform.parent.GetComponentInChildren<InteractionSettings> ().carryingObject == Services.Player.transform) {
				//Debug.Log ("brown mushroom function activated");
				beingUsed = true;
				transform.parent.GetComponent<Renderer> ().enabled = false;
				transform.parent.GetComponent<Collider> ().enabled = false;
				playerCamera.GetComponent<DepthOfField> ().enabled = !playerCamera.GetComponent<DepthOfField> ().enabled;
				FadeBack (timeUntilRevert);
			} else {
				//eaten by NPC
				Destroy (transform.parent.gameObject);
			}
		}
    }

	void FadeBack(float t) {

		DOTween.To(
			()=> playerCamera.GetComponent<DepthOfField> ().aperture, 
			x=> playerCamera.GetComponent<DepthOfField> ().aperture = x, 
			0.2f, t).OnComplete(BackToNormal);
	}

    void BackToNormal()
    {
        playerCamera.GetComponent<DepthOfField>().enabled = !playerCamera.GetComponent<DepthOfField>().enabled;
		playerCamera.GetComponent<DepthOfField> ().aperture = originalAperture;
		Destroy (transform.parent.gameObject);
    }


}
