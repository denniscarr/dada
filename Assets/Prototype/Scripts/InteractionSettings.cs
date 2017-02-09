using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSettings : MonoBehaviour {

	public bool ableToBeCarried;	// Whether the object is able to be carried.
	public bool usable;	// Whether the object is usable.

	Vector3 savedScale;
	Vector3 SavedScale {
		get;
	}

	void Start() {
		savedScale = transform.localScale;
	}
}
