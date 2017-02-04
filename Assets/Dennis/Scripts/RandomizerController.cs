using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class sets up and controls the randomizable properties of the attached object.
public class RandomizerController : MonoBehaviour {

	// This float represents the magnitude with which the randomized effects are applied to this object.
	// It's actual affect will depend on the scripts that we write later. Should range from 0 to 1.
	public float randomness = 0.0f;

	// A list that will hold references to this gameObject and all child gameObjects
	List<GameObject> selfAndChildren;

	// A list of all of my randomizers
	List<GameObject> myRandomizers;

	void Start ()
	{
		// Add this game object and all child objects to selfAndChildren
		selfAndChildren = new List<GameObject>();
		selfAndChildren.Add(gameObject);
		for (int i = 0; i < transform.childCount; i++) {
			selfAndChildren.Add(transform.GetChild(i).gameObject);
		}

		// Go through selfAndChildren game objects and add various randomizer scripts based on what components are currently attached to those objects.
		foreach(GameObject go in selfAndChildren)
		{
			if (go.GetComponent<Rigidbody>() != null && go.GetComponent<RigidbodyRandomizer>() == null) {
				go.AddComponent<RigidbodyRandomizer>();
			}
		}
	}

	public void UpdateRandomizerList() {
		
	}

	public void ModifyRandomness(float randomnessToAdd) {
		randomness += randomnessToAdd;
		randomness = Mathf.Clamp(randomness, 0f, 1f);
	}
}
