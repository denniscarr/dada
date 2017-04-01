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
	public bool equipped = false;
	Transform equippedItem;

	// or perhaps chain quest -- number of items to steal
	// this, then this, then this, etc
	// or maybe you have to equip it and use it

	void Start (){

		finder = GameObject.Find ("QuestManager").GetComponent<QuestFinderScript> ();
		builder = GameObject.Find ("QuestManager").GetComponent<QuestBuilderScript> ();
		equipFind = Camera.main.GetComponent<EquippableFinder> ();
		manager = GameObject.Find ("QuestManager").GetComponent<QuestManager> ();

	}

	void FixedUpdate () {
		equippedItem = equipFind.equippedObject;

		if (this.transform == equippedItem.transform && !equipped) {
			// add some text differences here
			equipped = true;
		} else if (equippedItem.transform == null && equipped) {
			equipped = false;
		}

		if (equipped && equippedItem.transform != null) {
			//EndQuest();
		}
			
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

		// put it on the parent object
		CopyComponent (this, parentObject);

		spawnNote ();

	}

	Component CopyComponent (Component original, GameObject destination){
		System.Type type = original.GetType ();
		Component copy = destination.AddComponent(type);

		System.Reflection.FieldInfo[] fields = type.GetFields ();
		foreach (System.Reflection.FieldInfo field in fields) {
			field.SetValue (copy, field.GetValue(original));
		}
		return copy;
	}

	public void spawnNote(){
		// make the questit note
		visorNode = GameObject.Find ("UpperNode").GetComponent<Transform>();
		questItNote = Instantiate(Resources.Load("QuestItNote", typeof (GameObject))) as GameObject;
		questItNote.transform.position = visorNode.transform.position;

		// make the actual text appear
		Canvas questCanvas = questItNote.GetComponentInChildren<Canvas>();
		questCanvas.transform.parent = questItNote.gameObject.transform;
		Text questText = questCanvas.GetComponentInChildren<Text> ();
		questText.text = description;
	}

	public void questTextSpawn(){
		// put the text of the quest right over the object
		textSpawn = Instantiate (Resources.Load("TextSpawn", typeof(GameObject))) as GameObject;
		textSpawn.transform.parent = parentObject.transform;
		textSpawn.transform.position = new Vector3 (positionX, positionY, positionZ);
		text = textSpawn.GetComponent<TextMesh> ();
		text.text = (description);

	}

	// change these interactions to make them more interesting and meaningful
	public void EndQuest(){
		//PickupQuest theCurrentQuest = parentObject.GetComponent<PickupQuest>();
		StealQuest theCurrentQuest = parentObject.GetComponent<StealQuest>();

		text.text = ("donezo");
		progress = Quest.QuestProgress.COMPLETE;

		if (Input.GetMouseButton(0)){
			Destroy (parentObject);
			manager.currentQuestList.Remove (theCurrentQuest);
		}

		GameObject.Find ("Bathroom Sink").GetComponentInChildren<D_starryExpolsion>().Explosion();
	}

// public void to finish quest {
//reference the current quest on the object
//change the text
//change the progress
//destroy at some point
//}
}