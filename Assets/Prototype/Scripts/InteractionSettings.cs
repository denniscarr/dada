using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSettings : MonoBehaviour {

	public bool ableToBeCarried
    {
        get
        {
            if (IsNPC)
            {
                return false;
            }

            else if (MyMath.LargestCoordinate(transform.parent.GetComponent<Collider>().bounds.extents) < 4f)
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

    bool IsNPC = true;

    // USED FOR CATCHING ON FIRE
    GameObject firePrefab;
    public bool onFire = false;
    float _heat = 0f;
    public float heat
    {
        get
        {
            return _heat;
        }

        set
        {
            if (value >= 1f && !onFire)
            {
                CatchFire();
            }

            value = Mathf.Clamp01(value);
            _heat = value;
        }
    }// When it gets to 1 I catch on fire.

    public Vector3 equipPosition;
    public Vector3 equipRotation;

    [HideInInspector] public Vector3 savedScale;

	[HideInInspector] public Transform originalParent;


	void Awake()
    {
        firePrefab = Resources.Load("Contagious Fire") as GameObject;

        savedScale = transform.parent.localScale;
		originalParent = transform.parent.parent;

        _carryingObject = null;

        if (transform.parent.GetComponentInChildren<NPC>() == null)
        {
            IsNPC = false;
        }

        else
        {
            IsNPC = true;
        }

        if (IsInVisor)
        {
            originalParent = null;
            Transform saved = transform.parent.parent;
            savedScale = new Vector3(1, 1, 1);
        }
    }


    private void Update()
    {
        if (transform.parent.GetComponentInChildren<NPC>() == null)
        {
            IsNPC = false;
        }

        else
        {
            IsNPC = true;
        }

        if (heat > 0)
        {
            heat -= 0.01f * Time.deltaTime;
        }
    }


    void CatchFire()
    {
        onFire = true;

        if (IsNPC)
        {
            transform.parent.GetComponentInChildren<NPC>().CatchOnFire();
        }

        GameObject fire = Instantiate(firePrefab);
        fire.transform.SetParent(transform.parent);
        fire.transform.localPosition = Vector3.zero;
    }
}
