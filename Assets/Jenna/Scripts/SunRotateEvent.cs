using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SunRotateEvent : IncoherenceEvent
{
    Color newLightingColor = Color.white;
    Color oldLightingColor;
    GameObject sun;

    new void Start()
    {
        base.Start();

        sun = GameObject.Find("Sun");

        instantaneous = false;
    }


    public override void Initiate()
    {
        base.Initiate();

        sun.transform.DORotate(new Vector3(sun.transform.rotation.eulerAngles.x, Random.Range(0f, 360f), sun.transform.eulerAngles.z), Random.Range(1f, 10f), RotateMode.Fast);

        sun.GetComponent<Light>().DOBlendableColor(Random.ColorHSV(), Random.Range(1f, 10f));

        active = true;
    }


    public override void Perform()
    {
        
    }
}
