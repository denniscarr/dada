using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using DG.Tweening;

public class D_purpleMushroomFunction : D_Function {

    GameObject playerCamera;
	ColorCorrectionCurves colorCorrection;
	float saturationOrigValue;
	public float timeToReset = 40f;
	bool beingUsed;



    // Use this for initialization
    new void Start()
    {
        base.Start();

        playerCamera = Services.Player.GetComponentInChildren<Camera>().gameObject;
		colorCorrection = playerCamera.GetComponent<ColorCorrectionCurves> ();
		saturationOrigValue = colorCorrection.saturation;
    }

    // Update is called once per frame
    public override void Use()
    {
		if (!beingUsed) {
			base.Use ();
			print ("Purple mushroom function triggered");
			if (transform.parent.GetComponentInChildren<InteractionSettings> ().carryingObject == Services.Player.transform) {
				beingUsed = true;
				colorCorrection.enabled = !colorCorrection.enabled;
				colorCorrection.saturation = 0f;
				transform.parent.GetComponent<Collider> ().enabled = false;
				transform.parent.GetComponent<Renderer> ().enabled = false;
				FadeBack (timeToReset);
			} else {
				//eaten by NPC
				Destroy (gameObject.transform.parent.gameObject);
			}
		}
    }

	void FadeBack(float t) {
		DOTween.To (
			() => colorCorrection.saturation,
			x => colorCorrection.saturation = x,
			1.0f, t).OnComplete(BackToNormal);
	}

    void BackToNormal() {
		colorCorrection.enabled = !colorCorrection.enabled;
		colorCorrection.saturation = saturationOrigValue;
		Destroy (transform.parent.gameObject);
    }
}
