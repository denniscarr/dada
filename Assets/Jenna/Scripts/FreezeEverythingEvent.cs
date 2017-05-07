using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEverythingEvent : IncoherenceEvent {

	new void Start()
    {
        base.Start();
    }


    public override void Initiate()
    {
        base.Initiate();

        foreach (InteractionSettings intSet in FindObjectsOfType<InteractionSettings>())
        {
            if (intSet.transform.parent.GetComponent<Rigidbody>() != null) intSet.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
