using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class D_greenMushroomFunction : D_Function {

    GameObject playerCamera;

    // Use this for initialization
    new void Start()
    {
        base.Start();

        playerCamera = Services.Player.GetComponentInChildren<Camera>().gameObject;
    }

    // Update is called once per frame
    public override void Use()
    {
        base.Use();
		print ("Green mushroom function triggered");
        if (transform.parent.GetComponentInChildren<InteractionSettings>().carryingObject == Services.Player.transform)
        {
            playerCamera.GetComponent<VignetteAndChromaticAberration>().enabled = !playerCamera.GetComponent<VignetteAndChromaticAberration>().enabled;
            Invoke("BackToNormal", 60f);
        }
    }

    void BackToNormal()
    {
        playerCamera.GetComponent<VignetteAndChromaticAberration>().enabled = !playerCamera.GetComponent<VignetteAndChromaticAberration>().enabled;
    }
}
