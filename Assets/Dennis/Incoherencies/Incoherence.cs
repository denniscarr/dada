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

	protected Vector3 ModifyVector3(float maxChange) {
		float magFactor = MapIncoherenceMagnitude (0, maxChange);
		return Random.insideUnitSphere * Random.Range(-magFactor, magFactor);
	}

	protected Vector3 ModifyVector3(float maxChange, bool stayPositive) {
		if (stayPositive) {
			float magFactor = MapIncoherenceMagnitude (0, maxChange);
			return Random.insideUnitSphere * magFactor;
		} else {
			return ModifyVector3 (maxChange);
		}
	}

	protected bool ModifyBool(bool boolToModify) {
		float rand = Random.Range (0f, 1f);
		if (rand > myController.incoherenceMagnitude) {
			print ("change bool failed");
			return boolToModify;
		} else {
			print ("change bool succeeded");
			return !boolToModify;
		}
	}

	protected Quaternion ModifyQuaternion(Vector3 eulers, float maxChange) {
		float magFactor = MapIncoherenceMagnitude (0, maxChange);
		Vector3 tempVect = eulers;
		tempVect += Random.insideUnitSphere * Random.Range(-magFactor, magFactor);
		return Quaternion.Euler (tempVect);
	}

	protected float ModifyFloat(float maxChange) {
		float magFactor = MapIncoherenceMagnitude (0, maxChange);
		return Random.Range(-magFactor, magFactor);
	}


	// This function contains the stuff that actually happens when this incoherence is expressed.
	// What it actually does depends on the extending script.
	public virtual void ExpressTimedIncoherence(float magnitude) {
	}
}
