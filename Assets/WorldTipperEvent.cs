using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WorldTipperEvent : IncoherenceEvent {

	new void Start()
    {
        base.Start();

        instantaneous = true;
        threshold = 0.4f;
    }


    public override void Initiate()
    {
        base.Initiate();

        // Get a new rotation for the world.
        Vector3 newRotation = new Vector3(
            Random.Range(-35f, 35f),
            Random.Range(-35f, 35f),
            Random.Range(-35f, 35f)
            );

        Transform level = FindObjectOfType<Level>().transform;

        level.DORotate(newRotation, Random.Range(1f, 10f), RotateMode.Fast);
    }
}
