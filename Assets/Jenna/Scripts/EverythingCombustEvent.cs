using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EverythingCombustEvent : IncoherenceEvent {

	new void Start()
    {
        base.Start();

        threshold = 0.6f;
        instantaneous = true;
    }


    public override void Initiate()
    {
        // Find all game objects.
        foreach (InteractionSettings intSet in FindObjectsOfType<InteractionSettings>())
        {
            if (Random.value > 0.5f)
            {
                intSet.heat += 100;
            }
        }
    }
}
