using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour {

	// A reference to this randomizer's controller.
	public RandomizerController myController;

	void Start() {

		/* FIND MY CONTROLLER */

		myController = MiscFunctions.FindGameObjectInRoot(transform.root, "Randomizer Controller").GetComponent<RandomizerController>();
	}
}
