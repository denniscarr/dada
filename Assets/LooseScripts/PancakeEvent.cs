using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeEvent : IncoherenceEvent {


    new void Start()
    {
        base.Start();

        threshold = 0.6f;
        instantaneous = true;
    }


    public override void Initiate()
    {
        base.Initiate();

        // Turn stuff into pancakes.
        foreach (InteractionSettings intSet in FindObjectsOfType<InteractionSettings>())
        {
            bool bigX = Random.value < 0.5f;
            bool bigY = Random.value < 0.5f;
            bool bigZ = Random.value < 0.5f;

            Vector3 newScale = intSet.transform.parent.localScale;

            if (bigX) newScale.x *= 50f;
            else newScale.x *= 0.01f;
            if (bigY) newScale.y *= 50f;
            else newScale.y *= 0.01f;
            if (bigZ) newScale.z *= 50f;
            else newScale.z *= 0.01f;

            intSet.transform.parent.localScale = newScale;
        }
    }
}
