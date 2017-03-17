using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupQuest : Quest {

	// finding object
	public GameObject objectToPickUp;
	public int timesPressed;
	public GameObject parentObject;

	// scripts
	public QuestFinderScript qfs;
	public QuestBuilderScript builder;
	public QuestManager manager;
	public QuestObject objectScript;

	void Start () {

		// find referenced materials
		builder = gameObject.GetComponent<QuestBuilderScript> ();
		qfs = gameObject.GetComponent<QuestFinderScript> ();
		manager = gameObject.GetComponent<QuestManager> ();
		timesPressed = 0;
	}

	public void makeTheQuest(Quest type){

		parentObject = builder.objeto;
		objectScript = parentObject.GetComponent<QuestObject> ();

		// set the title based on random strings from the Quest Builder Script
		title = (builder.verbs[builder.currentVerb] + " " + builder.nouns[builder.currentNoun]);

		// set the ID based on what point in the queue it is
		// note: there's probably a more efficient way to do this, pls lmk if so
		id = (QuestManager.questManager.questList.Count);

		// add to the list of available quests on the parent object
		objectScript.receivableQuestIDs.Add(id);

		// check if the parent gameobject has quest slots available
		manager.CheckAvailableQuests (objectScript);

		// check if THIS quest is available, and if so, make it available
		manager.RequestAvailableQuest (id);										
		progress = Quest.QuestProgress.AVAILABLE;

		// give it a description eh
		// can make this more interesting later during tweaking/juicing stages
		description = title;

		// once done, add to list
		// once added to list, destroy...or maybe add something to the queue or something?
		// HOW TO ADD THIS SCRIPT TO THE PARENTOBJECT?

		CopyComponent (this, parentObject);
	}

	// method to copy alla this shit on the pickupquest on the quest object generated
	// in questbuilderscript
	Component CopyComponent (Component original, GameObject destination){
		System.Type type = original.GetType ();
		Component copy = destination.AddComponent(type);

		System.Reflection.FieldInfo[] fields = type.GetFields ();
		foreach (System.Reflection.FieldInfo field in fields) {
			field.SetValue (copy, field.GetValue(original));
		}
		return copy;
	}

	// when this is uncommented, it fucks with the "progress" mechanism, so -- have to figure out a new way to determine
	// if a thing has been completed or not
//	void OnMouseDown(){
//		timesPressed++;
//		if (timesPressed > 3) {
//			//progress = Quest.QuestProgress.COMPLETE;
//			//manager.CheckCompletedQuests (this);
//		}
//	}

	public override void CheckStatus() {
		// check if done on active quests

	}
}
