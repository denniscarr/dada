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
		objects = GameObject.FindObjectsOfType<GameObject>();
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
		
			foreach (GameObject go in interactables){

				if (iset.canBeUsedForQuests == true) {
					Debug.Log (go + "can be used for quests");
					// I think the generator will have to be a get/set maybe, or a function,
					// not really sure, but it'll go here
					// it's hard because, like, the iset had to be nested in the for loop because reasons
					// so it'll be hard to access outside of this function.
					// I guess it'll all go in start for now, it's not like it can go anywhere else
					// might make load times kind of slow, but it'll be done before the player is there
					// what will be difficult, however, is making it reactive when new objects are added
					// but that's something I/we can handle later.
				}
			}
		}
	}

	public void Generator(GameObject quester, QuestObject quo, int questID){
		
	}
}
