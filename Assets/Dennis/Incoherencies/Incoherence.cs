using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// An Incoherence is a class which causes random effects at a given interval. Any scripts which randomize objects should extend this script.

public class Incoherence : MonoBehaviour {

	// The controller which is in charge of this Incoherence
	public IncoherenceController myController;

	// How high this object's incoherence has to be before this effect becomes active.
	public float threshold;

	// How often instantaneous effects are applied.
	public float frequencyMin;
	public float frequencyMax;
	float currentFrequency;

	float timeSinceLastIncoherence;

	protected int numberOfTimedIncoherences;


	protected void Start ()
	{
		// Find and get a reference to my incoherence controller
		myController = MiscFunctions.FindGameObjectInRoot(transform.root, "Incoherence Controller").GetComponent<IncoherenceController>();

		// Add this incoherence to its controller's list
		myController.incoherencies.Add(gameObject);
	}

	// Remaps incoherence from between 0 and 1 to between any two values. Useful for setting inter
	public float MapIncoherenceMagnitude(float min, float max) {
		return MiscFunctions.Map(myController.incoherenceMagnitude, 0f, 1f, min, max);
	}


	// This function contains the stuff that actually happens when this incoherence is expressed.
	// What it actually does depends on the extending script.
	public virtual void ExpressTimedIncoherence(float magnitude) {
	}
}
