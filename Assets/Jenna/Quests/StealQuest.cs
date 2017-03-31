using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// LOCATION: QUEST MANAGER
// LOCATION: QUEST GAMEOBJECT (description serves as "giving of quest")

public class StealQuest : Quest {

	// find the object
	public GameObject parentObject;
	Transform equippedObject;

	// scripts
	[HideInInspector]
	QuestFinderScript finder;
	[HideInInspector]
	QuestBuilderScript builder;
	[HideInInspector]
	public EquippableFinder equipFind;
	[HideInInspector]
	public QuestManager manager;
	[HideInInspector]
	public QuestObject questobjectscript;

	// text
	PickupQuest pickup;	//using this to spawn quest text
	public GameObject textSpawn;
	public TextMesh text;

	// for the questit note
	public GameObject questItNote;
	Transform visorNode;

	// for text spawning
	float positionX;
	float positionY;
	float positionZ;

	// finishing the quest
	// info here


	// or perhaps chain quest -- number of items to steal
	// this, then this, then this, etc

	void Start (){

		finder = GameObject.Find ("QuestManager").GetComponent<QuestFinderScript> ();
		builder = GameObject.Find ("QuestManager").GetComponent<QuestBuilderScript> ();
		equipFind = Camera.main.GetComponent<EquippableFinder> ();
		manager = GameObject.Find ("QuestManager").GetComponent<QuestManager> ();

	}

	void MakeStealQuest(Quest type){

		parentObject = builder.objeto;
		questobjectscript = parentObject.GetComponent<QuestObject> ();
		// insert paramaters for completion here

		// store the transform for later text spawning
		positionX = parentObject.transform.position.x;
		positionY = parentObject.transform.position.y + 1;
		positionZ = parentObject.transform.position.z;

		// create title to appear. THIS IS THE QUEST OBJECTIVE.
		title = ("Equip" + " " + parentObject.name); 

		// set the ID based on what point in the queue it is
		// note: there's probably a more efficient way to do this, pls lmk if so
		id = (QuestManager.questManager.questList.Count);

		// add to the list of available quests on the parent object
		questobjectscript.receivableQuestIDs.Add(id);

		// check if the parent gameobject has quest slots available
		manager.CheckAvailableQuests (questobjectscript);

		// check if THIS quest is available, and if so, make it available
		manager.RequestAvailableQuest (id);										
		progress = Quest.QuestProgress.AVAILABLE;

		// give it a description eh
		// can make this more interesting later
		description = title;

//		// put it on the parent object
//		CopyComponent (this, parentObject);
//
//		spawnNote ();

	}

// void FixedUpdate(){
// check to see if conditions have been met
// if they haven't, keep checking
// if they have, run the "finish quest" script (whatever it's called) 
//}

// public void to build quest () {
// get the object
// get the quest object script

// position storage for text spawning

// title
// id
// check available quest slots
// if yes, add this to the request available quest
// progress available
// give description
// put it on the parent object

// copy the component
//}

// copy component function

// public void to finish quest {
//reference the current quest on the object
//change the text
//change the progress
//destroy at some point
//}
}