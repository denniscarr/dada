using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// LOCATION: QUEST MANAGER
// LOCATION: QUEST GAMEOBJECT (description serves as "giving of quest")

public class PickupQuest : Quest {

	// finding object
	public GameObject parentObject;

	// scripts
	[HideInInspector]
	public QuestFinderScript qfs;
	[HideInInspector]
	public QuestBuilderScript builder;
	[HideInInspector]
	public QuestManager manager;
	public QuestObject objectScript;

	// for text
	public GameObject textSpawn;
	public TextMesh text;

	// for the questit note
	public GameObject questItNote;
	Transform visorNode;

	// for finishing quest
	public int requiredPickups;
	public int numberofPickups;
	public bool pickedUp;

	float positionX;
	float positionY;
	float positionZ;

	void Start () {

		// find referenced materials
		builder = gameObject.GetComponent<QuestBuilderScript> ();
		qfs = gameObject.GetComponent<QuestFinderScript> ();
		manager = gameObject.GetComponent<QuestManager> ();

	}

	public void FixedUpdate(){
//	 check to see if the thing has been picked up
//	 if so YAY FINISH

		if (parentObject != null && parentObject.GetComponentInChildren<InteractionSettings>() != null)
		{
			if (parentObject.GetComponentInChildren<InteractionSettings> ().carryingObject != null &&
			    parentObject.GetComponentInChildren<InteractionSettings> ().carryingObject.name == "Player" &&
			    !pickedUp) {
				numberofPickups++;
				text.text = ("Picked up " + numberofPickups.ToString () + " " + "times");

				if (numberofPickups >= requiredPickups) {
					text.text = ("Click to remove me!!");
					FinishQuest ();
				}

				pickedUp = true;
			
			} else if (parentObject.GetComponentInChildren<InteractionSettings> ().carryingObject == null) {
				pickedUp = false;
			}
		}
	}

	public void makeTheQuest(Quest type){

		parentObject = builder.objeto;
		objectScript = parentObject.GetComponent<QuestObject> ();
		requiredPickups = Random.Range (1, 5);

		// store the transform for later text spawning
		positionX = parentObject.transform.position.x;
		positionY = parentObject.transform.position.y + 1;
		positionZ = parentObject.transform.position.z;

		// create title to appear. THIS IS THE QUEST OBJECTIVE.
		title = ("Pick up" + " " + parentObject.name); 

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
		description = (title + " " + requiredPickups.ToString() + "x");

		// put it on the parent object
		CopyComponent (this, parentObject);

		spawnNote ();
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

	public void spawnNote(){
		// make the questit note
		visorNode = GameObject.Find ("UpperNode").GetComponent<Transform>();
		questItNote = Instantiate(Resources.Load("QuestItNote", typeof (GameObject))) as GameObject;
		questItNote.transform.position = visorNode.transform.position;

		// make the actual text appear
		Canvas questCanvas = questItNote.GetComponentInChildren<Canvas>();
		questCanvas.transform.parent = questItNote.gameObject.transform;
		Text questText = questCanvas.GetComponentInChildren<Text> ();
		questText.text = title;
	}

	public void questTextSpawn(){
		// put the text of the quest right over the object
		textSpawn = Instantiate (Resources.Load("TextSpawn", typeof(GameObject))) as GameObject;
		textSpawn.transform.parent = parentObject.transform;
		textSpawn.transform.position = new Vector3 (positionX, positionY, positionZ);
		text = textSpawn.GetComponent<TextMesh> ();
		text.text = (description);
		
	}

	public void FinishQuest(){
		
		PickupQuest theCurrentQuest = parentObject.GetComponent<PickupQuest>();

		text.text = ("donezo");
		progress = Quest.QuestProgress.COMPLETE;

		if (Input.GetMouseButton(0)){
			Destroy (parentObject);
			manager.currentQuestList.Remove (theCurrentQuest);
		}
	}

	public override void CheckStatus() {
		// check if done on active quests

	}
}
