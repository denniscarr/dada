using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeminiEvent : IncoherenceEvent {

    new void Start()
    {
        base.Start();

        instantaneous = true;
    }


    new void Initiate()
    {
        // Clone everything.
        foreach (InteractionSettings intSet in FindObjectsOfType<InteractionSettings>())
        {
            GameObject twin = Instantiate(intSet.transform.parent.gameObject, intSet.transform.parent.position + Random.insideUnitSphere * 2, Random.rotation);
            //twin.transform.localScale += Random.insideUnitSphere * 1;
        }
    }
}
