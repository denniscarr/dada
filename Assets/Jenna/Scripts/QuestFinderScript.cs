using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// do i need to make a property for this...
// like a get (object.getcomponent) --> return bool true else return false
// and then a set if object.getcomponent return bool true add to list
// else don't

// but why or how is that different than a for loop?

// note to self: make a function to check automatically if something ain't on a list
// and if it ain't, add it, else don't

public class QuestFinderScript : MonoBehaviour
{
	GameObject[] objects;
	// for seeing if they're interactable
	public List<GameObject> interactables = new List<GameObject> ();
	public List<GameObject> questItems = new List<GameObject> ();
	int interactablesSize;
	int objectsSize;

	// for seeing if they have QuestObject script
	public List<GameObject> hasObjectScript = new List<GameObject> ();

	// to talk abot this in other scripts
	public bool canBeUsed = false;

	void Start ()
	{

		// find the items in the scene and add them to a list of questable items
		interactables = new List<GameObject> ();
		objects = GameObject.FindObjectsOfType<GameObject> ();
		objectsSize = objects.Length;

		// loop through the items in the scene
		for (int i = 0; i < objectsSize; i++) {
			InteractionSettings iset = objects [i].GetComponentInChildren<InteractionSettings> ();
			if (iset != null) {
				GameObject iset1 = iset.gameObject.transform.parent.gameObject;
				//interactables.Add (iset.gameObject);
				if (!interactables.Contains (iset1)) {
					interactables.Add (iset1);
				}
				// all right, so if it's on the list, calculate a random float
				// to determine whether or not it can be used for quests
				if (interactables.Contains (iset1)) {
					float ranger = Random.Range (0f, 1f);
					if (ranger > 0.5f) {
						iset.canBeUsedForQuests = true;
						canBeUsed = true;
					} else {
						iset.canBeUsedForQuests = false;
						canBeUsed = false;
					}

					foreach (GameObject go in interactables) {
						if (iset.canBeUsedForQuests == true) {
						
							questItems.Add (go);

							//add quest object script
							QuestObject quo = go.GetComponent<QuestObject> ();
							if (quo != null) {
								if (!hasObjectScript.Contains (go)) {
									hasObjectScript.Add (go);
								}
							} else {
								go.gameObject.AddComponent<QuestObject> ();
								if (!hasObjectScript.Contains (go)) {
									hasObjectScript.Add (go);
								}
							}
						}
					}
				}
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