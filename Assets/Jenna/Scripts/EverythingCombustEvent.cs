using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EverythingCombustEvent : IncoherenceEvent {

	new void Start()
    {
        base.Start();

        instantaneous = true;
    }


    public override void Initiate()
    {
        // Find all game objects.
        foreach (InteractionSettings intSet in FindObjectsOfType<InteractionSettings>())
        {
            intSet.heat += 100;
        }
    }
}
