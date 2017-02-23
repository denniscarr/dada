using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// do i need to make a property for this...
// like a get (object.getcomponent) --> return bool true else return false
// and then a set if object.getcomponent return bool true add to list
// else don't

// but why or how is that different than a for loop?

public class QuestFinderScript : MonoBehaviour {
	GameObject[] objects;
	public List<GameObject> interactables = new List<GameObject> ();
	int interactablesSize;
	int objectsSize;

//	public InteractionSettings interactionSettings;

	void Start(){

		// find the items in the scene and add them to a list of questable items
		interactables = new List<GameObject> ();
		objects = GameObject.FindGameObjectsWithTag ("questable");
		objectsSize = objects.Length;

		// loop through the items in the scene
		for (int i = 0; i < objectsSize; i++) {

			InteractionSettings iset = objects [i].GetComponentInChildren<InteractionSettings> ();
			if (iset != null) {
				interactables.Add (iset.gameObject);

				// all right, so if it's on the list, calculate a random float
				// to determine whether or not it can be used for quests
				if (interactables.Contains (iset.gameObject)) {
					float ranger = Random.Range (0f, 1f);
					if (ranger > 0.5f) {
						iset.canBeUsedForQuests = true;
					} else {
						iset.canBeUsedForQuests = false;
					}
				}
			}
		}
	}
}
