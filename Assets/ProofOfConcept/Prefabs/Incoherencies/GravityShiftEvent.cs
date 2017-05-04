using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityShiftEvent : IncoherenceEvent {

    public float maxMultiplier = 60f;
    Vector3 savedGravity; // The gravity before my last shift.


    public GravityShiftEvent()
    {
        instantaneous = false;
    }


    public override void Initiate()
    {
        base.Initiate();

        savedGravity = Physics.gravity;
        Vector3 newGravity = Random.insideUnitSphere * maxMultiplier;
        newGravity.y = Mathf.Clamp(newGravity.y, 1f, maxMultiplier);
        Physics.gravity = newGravity;
        active = true;
    }


    public override void Perform()
    {
        // 
    }
}
