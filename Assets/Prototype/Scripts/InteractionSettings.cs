using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSettings : MonoBehaviour {

	public bool ableToBeCarried;	// Whether the object is able to be carried.
	public bool usable;	// Whether the object is usable.
	public bool canBeUsedAsSoundSource;
	public bool canBeUsedForQuests;
	public bool carryingObjectCarryingObject;	// If carrying object is holding me
    public Transform _carryingObject;
	public Transform carryingObject // If I am being held, this is the object that is holding me.
    {
        get { return _carryingObject; }
        set
        {
            // If the thing that picked me up was the player, increase my incoherence.
            if (value.name == "Player")
            {
                transform.parent.GetComponentInChildren<IncoherenceController>().incoherenceMagnitude += Services.IncoherenceManager.interactionIncrease;
            }

            else
            {
                // If the thing that picked me up was an NPC, increase that NPC's incoherence to my incoherence.
                if (GetComponent<IncoherenceController>().incoherenceMagnitude > value.GetComponent<IncoherenceController>().incoherenceMagnitude)
                {
                    value.GetComponent<IncoherenceController>().incoherenceMagnitude = GetComponent<IncoherenceController>().incoherenceMagnitude;
                }

                // Otherwise, if the NPC that picked me up has a higher incoherence, increase my incoherence to match theirs.
                else
                {
                    GetComponent<IncoherenceController>().incoherenceMagnitude = value.GetComponent<IncoherenceController>().incoherenceMagnitude;
                }
            }

            _carryingObject = value;
        }
    }    

	[HideInInspector]
	public Vector3 savedScale;

	[HideInInspector]
	public Transform originalParent;

	void Start()
    {
		savedScale = transform.parent.localScale;
		originalParent = transform.parent.parent;
	}
}
