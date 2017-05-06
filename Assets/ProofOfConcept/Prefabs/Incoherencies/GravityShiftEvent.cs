using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityShiftEvent : IncoherenceEvent {

    float maxMultiplier = 60f;   // The maximum thing that gravity can turn into.
    float returnSpeed = 0f; // How quickly we should return to normal gravity.

    Vector3 savedGravity; // The gravity before my last shift.
    Vector3 newGravity;

    float lerpValue = 0;


    new void Start()
    {
        base.Start();
    }


    new void Update()
    {
        base.Update();

        if (active)
        {
            Perform();
        }
    }


    public GravityShiftEvent()
    {
        instantaneous = false;
    }


    public override void Initiate()
    {
        base.Initiate();

        // Save gravity.
        savedGravity = Physics.gravity;

        maxMultiplier = Random.Range(0f, MyMath.Map(Services.IncoherenceManager.globalIncoherence, 0f, 1f, 0f, 100f));

        returnSpeed = Random.Range(0.01f, 0.5f);

        // Randomize gravity
        newGravity = Random.insideUnitSphere * maxMultiplier;

        // Make sure gravity doesn't ever just throw you upwards.
        //newGravity.y = Mathf.Clamp(newGravity.y, 1f, maxMultiplier);
        Physics.gravity = newGravity;

        active = true;
    }


    public override void Perform()
    {
        base.Perform();

        lerpValue += returnSpeed * Time.deltaTime;

        // Lerp gravity back to original value.
        Physics.gravity = Vector3.Lerp(newGravity, savedGravity, lerpValue);

        if (lerpValue >= 0.99f)
        {
            Physics.gravity = savedGravity;
            lerpValue = 0;
            active = false;
        }
    }
}
