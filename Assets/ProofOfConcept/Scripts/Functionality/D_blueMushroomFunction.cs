using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using DG.Tweening;


public class D_blueMushroomFunction : D_Function {

    GameObject playerCamera;
	public float timeUntilRevert;
	ColorCorrectionCurves colorCurves;
	float origSaturation;
	bool beingUsed;


	// Use this for initialization
	new void Start () {
		base.Start ();

		beingUsed = false;
        playerCamera = Services.Player.GetComponentInChildren<Camera>().gameObject;
		colorCurves = playerCamera.GetComponent<ColorCorrectionCurves> ();
		origSaturation = colorCurves.saturation;
	}

    // Update is called once per frame
    public override void Use() {

		if (!beingUsed) {
			base.Use ();
			//print ("Blue mushroom function triggered");


			if (transform.parent.GetComponentInChildren<InteractionSettings> ().carryingObject == Services.Player.transform) { 
				colorCurves.enabled = !colorCurves.enabled;
				beingUsed = true;
				FadeBack (timeUntilRevert);
				transform.parent.GetComponent<Renderer> ().enabled = false;
				transform.parent.GetComponent<Collider> ().enabled = false;
				//Invoke("BackToNormal", timeUntilRevert);
			} else { 
				//eaten by NPC, destroy immediately
				Destroy (gameObject.transform.parent.gameObject);

			}
		}
	}

    void BackToNormal()
    {
		colorCurves.enabled = !colorCurves.enabled;
		colorCurves.saturation = 4f; 
		Destroy (gameObject.transform.parent.gameObject, 2f);
    }

	void FadeBack(float t) {
		
		DOTween.To(
			()=> colorCurves.saturation, 
			x=> colorCurves.saturation = x, 
			0.5f, t).OnComplete(BackToNormal);
	}
}
