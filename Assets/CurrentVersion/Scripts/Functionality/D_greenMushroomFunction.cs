using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using DG.Tweening;

public class D_greenMushroomFunction : D_Function {

    GameObject playerCamera;
	float origVigValue;
	float origChromAbValue;
	public float timeToReset = 40f;
	VignetteAndChromaticAberration vignetteEffect;
	bool beingUsed;

    // Use this for initialization
    new void Start()
    {
        base.Start();
		beingUsed = false;
        playerCamera = Services.Player.GetComponentInChildren<Camera>().gameObject;
		vignetteEffect = playerCamera.GetComponent<VignetteAndChromaticAberration> ();
		origVigValue = vignetteEffect.intensity;
		origChromAbValue = vignetteEffect.chromaticAberration;
	}

    // Update is called once per frame
    public override void Use()
    {
		if (!beingUsed) {
			base.Use ();
			print ("Green mushroom function triggered");
			if (transform.parent.GetComponentInChildren<InteractionSettings> ().carryingObject == Services.Player.transform) {
				beingUsed = true;
				transform.parent.GetComponent<Collider> ().enabled = false;
				transform.parent.GetComponent<Renderer> ().enabled = false;
				vignetteEffect.enabled = !vignetteEffect.enabled;
				FadeBack (timeToReset);
			} else { 
				//eaten by NPC
				Destroy (gameObject.transform.parent.gameObject);

			}
		}
    }

	void FadeBack(float t) {

		DOTween.To(
			()=> vignetteEffect.intensity,
			x=> vignetteEffect.intensity = x, 
			0f, t).OnComplete(BackToNormal);
		DOTween.To(
			()=> vignetteEffect.chromaticAberration,
			x=> vignetteEffect.chromaticAberration = x, 
			0f, t);
	}

    void BackToNormal()
    {
		vignetteEffect.enabled = !vignetteEffect.enabled;
		vignetteEffect.intensity = origVigValue;
		vignetteEffect.chromaticAberration = origChromAbValue;

		Destroy (transform.parent.gameObject);
    }
}
