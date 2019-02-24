using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using DG.Tweening;

public class D_yellowMushroomFunction : D_Function {

    GameObject playerCamera;
	Bloom bloom;
	float bloomOrigIntensity;
	public float timeToReset = 40f;
	bool beingUsed;

    // Use this for initialization
    new void Start()
    {
        base.Start();
		beingUsed = false;
        playerCamera = Services.Player.GetComponentInChildren<Camera>().gameObject;
		bloom = playerCamera.GetComponent<Bloom> ();
		bloomOrigIntensity = bloom.bloomIntensity;
    }

    // Update is called once per frame
    public override void Use()
    {
		if(!beingUsed) {
        	base.Use();
			if (transform.parent.GetComponentInChildren<InteractionSettings> ().carryingObject == Services.Player.transform) {
				beingUsed = true;
				bloom.enabled = !bloom.enabled;
				transform.parent.GetComponent<Collider> ().enabled = false;
				transform.parent.GetComponent<Renderer> ().enabled = false;
				FadeBack (timeToReset);
			} else {
				//eaten by NPC
				Destroy (transform.parent.gameObject);
			}
		}
    }

	void FadeBack(float t) {
		DOTween.To (
			() => bloom.bloomIntensity,
			x => bloom.bloomIntensity = x,
			0f, t
		).OnComplete (BackToNormal); 
	}

    void BackToNormal()
    {
        bloom.enabled = !bloom.enabled;
		bloom.bloomIntensity = bloomOrigIntensity;
		Destroy (transform.parent.gameObject);
    }
}
