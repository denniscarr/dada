using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSettings : MonoBehaviour {

	public bool ableToBeCarried;	// Whether the object is able to be carried.
	public bool usable;	// Whether the object is usable.
	public bool canBeUsedAsSoundSource;
	public bool canBeUsedForQuests;
    public Transform carryingObject;    // If I am being held, this is the object that is holding me.

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
