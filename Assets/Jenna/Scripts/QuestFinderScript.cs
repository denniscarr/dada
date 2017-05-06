using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LOCATION: QUEST MANAGER

public class QuestFinderScript : MonoBehaviour
{
	GameObject[] objects;
	// for seeing if they're interactable
	[HideInInspector]
	private List<GameObject> interactables = new List<GameObject> ();
	[HideInInspector]
	public List<GameObject> questItems = new List<GameObject> ();

	// for grail quest
	public Quest grailQuest;

	// removal from lists
	private List<GameObject> questItemsToRemove = new List<GameObject>();
	private List<GameObject> pickupsToRemove = new List<GameObject>();
	private List<GameObject> equipsToRemove = new List<GameObject>();
	private List<Quest> questsToRemove = new List<Quest>();

	// for seeing if they have QuestObject script
	[HideInInspector]
	public List<GameObject> hasObjectScript = new List<GameObject> ();

	// for seeing if they can be picked up
	public List<GameObject> pickups = new List<GameObject> ();

	// for seeing if they can be equipped
	public List<GameObject> equippables = new List<GameObject>();

	// ints bc ya never know
	int interactablesSize;
	int objectsSize;
//	public int currentQuests;
//	public int levelNum;
//	[HideInInspector]
//	public LevelManager levelman;
//	[HideInInspector]
//	public QuestManager questman;

	public void FindQuests (){

		// find the items in the scene and add them to a list of questable items
		interactables = new List<GameObject> ();
		objects = GameObject.FindObjectsOfType<GameObject> ();
		objectsSize = objects.Length;

		// loop through the items in the scene
		for (int i = 0; i < objectsSize; i++) {
			InteractionSettings iset = objects [i].GetComponentInChildren<InteractionSettings> ();
			if (iset != null) {
				GameObject iset1 = iset.transform.parent.gameObject;
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

                            // can it be picked up? Add it to the list!
							if (iset.ableToBeCarried == true && go.GetComponent<PickupQuest>() == null && go.GetComponent<QuestObject>() == null && !go.name.Contains("QuestItNote")){
								if (!pickups.Contains (go)) {
                                    //Debug.Log(go.name);
									pickups.Add (go);
								}
							}

							if (ranger >= 0.5f) {
								if (!equippables.Contains (go)) {
									equippables.Add (go);
								}
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

	void Update(){
		foreach (GameObject item in questItems) {
			if (item == null) {
				questItemsToRemove.Add (item);
			}
		}


		foreach (GameObject pickup in pickups) {
			if (pickup == null) {
				pickupsToRemove.Add (pickup);
			}
		}

		foreach (GameObject equippable in equippables) {
			if (equippable == null) {
				equipsToRemove.Add (equippable);
			}
		}
			

		foreach (Quest questy in QuestManager.questManager.questList) {
			if (questy == null) {
				questsToRemove.Add(questy);
			}
		}
			
		ClearNullListItems ();
	}

	void ClearNullListItems(){
		foreach (Quest q in questsToRemove) {
			QuestManager.questManager.questList.Remove (q);
		}

		foreach (GameObject pickup in pickupsToRemove) {
			pickups.Remove (pickup);
		}

		foreach (GameObject equippable in equipsToRemove) {
			equippables.Remove (equippable);
		}

		foreach (GameObject questItem in questItemsToRemove) {
			questItems.Remove (questItem);
		}

		questsToRemove.Clear ();
		pickupsToRemove.Clear ();
		equipsToRemove.Clear ();
		questItemsToRemove.Clear ();

	}
}