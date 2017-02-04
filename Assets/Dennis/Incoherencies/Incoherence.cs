using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incoherence : MonoBehaviour {

	public IncoherenceController myController;

	// How high this object's incoherence has to be before this effect becomes active.
	public float threshold;

	// How often this incoherence is applied, in seconds.
	public float frequencyMin;
	public float frequencyMax;
	float currentFrequency;

	float timeSinceLastIncoherence;


	protected void Start () {
		// Find and get a reference to my incoherence controller
		myController = MiscFunctions.FindGameObjectInRoot(transform.root, "Incoherence Controller").GetComponent<IncoherenceController>();

		// Add this incoherence to its controller's list
		myController.incoherencies.Add(gameObject);

		// Get the current frequency of expression
		currentFrequency = MapIncoherence(frequencyMax, frequencyMin);
	}


	protected void Update() {

		// If this game object does not have enough incoherence to pass the threshold, stop.
		if (myController.incoherence < threshold) {
			return;
		}

		currentFrequency = MapIncoherence(frequencyMax, frequencyMin);
			
		// See if it is time to express my incoherence
		if (timeSinceLastIncoherence >= currentFrequency) {
			print("Got past");

			ExpressIncoherence();
			timeSinceLastIncoherence = 0.0f;
		}

		timeSinceLastIncoherence += Time.time;
	}


	// Remaps incoherence from between 0 and 1 to between any two values.
	public float MapIncoherence(float min, float max) {
		return MiscFunctions.Map(myController.incoherence, 0f, 1f, min, max);
	}


	public virtual void ExpressIncoherence() {
		print("Expressed (Base)");
	}
}
