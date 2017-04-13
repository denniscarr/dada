using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSettings : MonoBehaviour {

    bool _ableToBeCarried = false;
	public bool ableToBeCarried
    {
        get
        {
            if ((!IsNPC && MyMath.LargestCoordinate(transform.parent.GetComponent<Collider>().bounds.extents) < 0.3f) || Random.value > 0.9999f)
            {
                return true;
            }

            else
            {
				return false;
            }
        }
    }	// Whether the object is able to be carried.
	public bool usable;	// Whether the object is usable.
	public bool canBeUsedAsSoundSource; // Whether the object can be used as a sound source.
	public bool canBeUsedForQuests; // Whether this object can be used for quests.
	public bool carryingObjectCarryingObject;	// If carrying object is holding me
    public Transform _carryingObject;
	public Transform carryingObject // If I am being held, this is the object that is holding me.
    {
        get { return _carryingObject; }
        set
        {
			if (value == null){ _carryingObject = null;return;}

//			if (value.name == "Mouse") {
//				value = Services.Player;
//			}

            // If the thing that picked me up was the player, increase my incoherence.
            if (value.name == "Player")
            {
				if (transform.parent.GetComponentInChildren<IncoherenceController>() != null){
					transform.parent.GetComponentInChildren<IncoherenceController>().incoherenceMagnitude
                        += Services.IncoherenceManager.interactionIncrease;
				}
            }

            else
            {
                // If the thing that picked me up was an NPC, increase that NPC's incoherence to my incoherence.
				if (transform.parent.GetComponent<IncoherenceController>() != null && transform.parent.GetComponent<IncoherenceController>().incoherenceMagnitude > value.GetComponent<IncoherenceController>().incoherenceMagnitude)
                {
                    value.GetComponent<IncoherenceController>().incoherenceMagnitude = GetComponent<IncoherenceController>().incoherenceMagnitude;
                }

                // Otherwise, if the NPC that picked me up has a higher incoherence, increase my incoherence to match theirs.
                else
                {
                    if (GetComponent<IncoherenceController>() != null) GetComponent<IncoherenceController>().incoherenceMagnitude = value.GetComponent<IncoherenceController>().incoherenceMagnitude;
                }
            }

            _carryingObject = value;
        }
    }
    public bool IsInVisor
    {
        get
        {
            if (transform.parent.parent != null && transform.parent.parent.name == "UpperNode")
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }   // Whether this object is currently in the player's visor.
    public bool IsNPC
    {
        get
        {
            if (transform.parent.GetComponentInChildren<NPC>() != null)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }

    public Vector3 equipPosition;
    public Vector3 equipRotation;

    [HideInInspector] public Vector3 savedScale;

	[HideInInspector] public Transform originalParent;


	void Start()
    {
        savedScale = transform.parent.localScale;
		originalParent = transform.parent.parent;

        _carryingObject = null;

        if (IsInVisor)
        {
            originalParent = null;
            Transform saved = transform.parent.parent;
            savedScale = new Vector3(1, 1, 1);
        }
    }
}
