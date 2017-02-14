using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class sets up, stores and controls the randomizable properties of the attached object.
// Every object in the game that we want to be randomized (ideally all of them) should have one of these.

public class IncoherenceController : MonoBehaviour {

	// This float represents the magnitude with which the randomized effects are applied to this object.
	// It's actual affect will depend on the scripts that we write later. Should range from 0 to 1.
	public float incoherenceProbability = 0.0f;
	public float incoherenceMagnitude = 0.0f;

	// How often this controller should check whether it should express an incoherence. (Seconds)
	public float howOftenToCheckProbability = 1f;
	float timeSinceLastCheck = 0f;

	// A list of all of the incoherencies controlled by this controller. Incoherencies will add themselves to this list.
	public List<GameObject> incoherencies;


	void Update() {

		// HACKY FOR PROTO
		float rando = Random.Range (0f, 1f);
		if (rando < 0.01) {
			incoherenceMagnitude += Random.Range (0.0001f, 0.01f);
			incoherenceProbability += Random.Range (0.0001f, 0.01f);
		}
	

		// Clamp incoherence probability and magnitude to between 0 and 1
		incoherenceProbability = Mathf.Clamp(incoherenceProbability, 0f, 1f);
		incoherenceMagnitude = Mathf.Clamp(incoherenceMagnitude, 0f, 1f);


		// See if I should express one of my timed incoherences.
		if (timeSinceLastCheck >= howOftenToCheckProbability)
		{
			float rand = Random.Range(0f, 1f);
			if (rand < incoherenceProbability) {
				
				// If so, choose a random incoherency from my list and tell it to express a timed incoherence
				if (incoherencies.Count > 0) {
					incoherencies[Random.Range(0, incoherencies.Count)].SendMessage("ExpressTimedIncoherence", incoherenceMagnitude);
				}

				// Reset probability to 0.
//				incoherenceProbability = 0f;
			}

			timeSinceLastCheck = 0f;
		}

		else {
			timeSinceLastCheck += Time.deltaTime;
		}

	}


	void LateUpdate() {
//		Vector3 clampPos = new Vector3 (Mathf.Clamp(transform.parent.position.x, -100f, 100f), Mathf.Clamp(transform.parent.position.y, 0f, 30f), Mathf.Clamp(transform.parent.position.z, -100f, 100f));
//		transform.parent.position = clampPos;
	}
		
}
