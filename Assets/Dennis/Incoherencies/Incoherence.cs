using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incoherence : MonoBehaviour {

	public IncoherenceController myController;

	void Start () {

		// Find and get a reference to my incoherence controller
		myController = MiscFunctions.FindGameObjectInRoot(transform.root, "Incoherence Controller").GetComponent<IncoherenceController>();
	}
}
