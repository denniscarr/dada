using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using DG.Tweening;


public class D_blueMushroomFunction : D_Function {

    GameObject playerCamera;
	public float timeUntilRevert;



	// Use this for initialization
	new void Start () {
		base.Start ();

        playerCamera = Services.Player.GetComponentInChildren<Camera>().gameObject;
	}

    // Update is called once per frame
    public override void Use() {
        base.Use();
		//print ("Blue mushroom function triggered");


		if (transform.parent.GetComponentInChildren<InteractionSettings> ().carryingObject == Services.Player.transform) { 
			playerCamera.GetComponent<ColorCorrectionCurves> ().enabled = !playerCamera.GetComponent<ColorCorrectionCurves> ().enabled;
			FadeBack (timeUntilRevert);
			transform.parent.GetComponent<Renderer> ().enabled = false;
			transform.parent.GetComponent<Collider> ().enabled = false;
			//Invoke("BackToNormal", timeUntilRevert);
		} else { 
			//eaten by NPC, destroy immediately
			Destroy (gameObject.transform.parent.gameObject);

		}
	}

    void BackToNormal()
    {
        playerCamera.GetComponent<ColorCorrectionCurves>().enabled = !playerCamera.GetComponent<ColorCorrectionCurves>().enabled;
		playerCamera.GetComponent<ColorCorrectionCurves> ().saturation = 4f; 
		Destroy (gameObject.transform.parent.gameObject, 2f);
    }

	void FadeBack(float t) {
		
		DOTween.To(
			()=> playerCamera.GetComponent<ColorCorrectionCurves>().saturation, 
			x=> playerCamera.GetComponent<ColorCorrectionCurves>().saturation = x, 
			0.5f, t).OnComplete(BackToNormal);
	}
}
