using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEverythingEvent : IncoherenceEvent {

    float timeToBeFrozen;
    float timeSinceFreeze;

	new void Start()
    {
        base.Start();
    }


    public override void Initiate()
    {
        base.Initiate();

        foreach (InteractionSettings intSet in FindObjectsOfType<InteractionSettings>())
        {
            if (intSet.transform.parent.name.Contains("Grail")) return;
            if (intSet.transform.parent.GetComponent<Rigidbody>() != null) intSet.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        timeToBeFrozen = Random.Range(1f, 10f);
        timeSinceFreeze = 0f;
    }


    public override void Perform()
    {
        base.Perform();

        timeSinceFreeze += Time.deltaTime;
        if (timeSinceFreeze >= timeToBeFrozen)
        {

            foreach (InteractionSettings intSet in FindObjectsOfType<InteractionSettings>())
            {
                if (intSet.transform.parent.GetComponent<Rigidbody>() != null) intSet.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }

            active = false;
        }
    }
}
