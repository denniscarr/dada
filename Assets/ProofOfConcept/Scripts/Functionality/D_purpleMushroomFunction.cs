using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class D_purpleMushroomFunction : D_Function {

    GameObject playerCamera;

    // Use this for initialization
    new void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    public override void Use()
    {
        base.Use();
        {
            playerCamera.GetComponent<Grayscale>().enabled = !playerCamera.GetComponent<Grayscale>().enabled;
        }
    }
}
