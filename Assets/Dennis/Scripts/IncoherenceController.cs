using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class sets up and controls the randomizable properties of the attached object.
public class IncoherenceController : MonoBehaviour {

	// This float represents the magnitude with which the randomized effects are applied to this object.
	// It's actual affect will depend on the scripts that we write later. Should range from 0 to 1.
	public float incoherence = 0.0f;

	// A list of all of my randomizers
	public List<GameObject> incoherencies;

	void ModifyIncoherence(float amount) {
		incoherence += amount;
		amount = Mathf.Clamp(amount, 0f, 1f);
	}
}
