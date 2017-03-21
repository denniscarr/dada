using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LOCATION: QUEST MANAGER

public class QuestFinderScript : MonoBehaviour
{
	GameObject[] objects;
	// for seeing if they're interactable
	private List<GameObject> interactables = new List<GameObject> ();
	public List<GameObject> questItems = new List<GameObject> ();
	int interactablesSize;
	int objectsSize;

	// for seeing if they have QuestObject script
	public List<GameObject> hasObjectScript = new List<GameObject> ();

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
				if (!interactables.Contains (iset1)) {
					interactables.Add (iset1);
				}
				// all right, so if it's on the list, calculate a random float
				// to determine whether or not it can be used for quests
				if (interactables.Contains (iset1)) {
					float ranger = Random.Range (0f, 1f);
					if (ranger > 0.5f) {
						iset.canBeUsedForQuests = true;
					} else {
						iset.canBeUsedForQuests = false;
					}

					foreach (GameObject go in interactables) {
						if (iset.canBeUsedForQuests == true) {
						
							if (!questItems.Contains (go)) {
								questItems.Add (go);
							}

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
				}
			}
		}
	}