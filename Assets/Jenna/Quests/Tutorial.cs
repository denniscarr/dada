using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : Quest {

	// NOTE: WHEN ALL QUESTS DONE, REMOVE THIS QUEST FROM MANAGER AND ALL OBJECTS

	// quest manager
	QuestManager manager;

	// player
	GameObject controller;

	// object script
	QuestObject objectScript;

	// interaction settings
	InteractionSettings intSet;

	// quest it note
	GameObject questItNote;
	Text text;

	// temporary items
	GameObject visor;
	GameObject textSpawn;

	// bools for the things having been done
	bool visorEquipped;
	bool roomEntered;
	bool thingMoved;
	bool visorExited;
	bool tabPressed;
	bool thingUsedOrMoved;
	bool itemAdded;
	bool finished;

	void Start () {

		// finding the quest manager
		manager = GameObject.Find ("QuestManager").GetComponent<QuestManager> ();

		// player controller
		controller = GameObject.Find ("PlayerControllerNew");
		controller.gameObject.AddComponent<QuestObject> ();

		// quest it note
		questItNote = Instantiate(Resources.Load ("QuestItNote")) as GameObject;
		questItNote.transform.position = new Vector3
			(controller.transform.position.x,
			controller.transform.position.y + 20,
			controller.transform.position.z + 10);

		// spawn text over the questitnote so they know to go to it
		textSpawn = Instantiate (Resources.Load("TextSpawn") as GameObject);
		TextMesh textSpawnText = textSpawn.GetComponent<TextMesh> ();
		textSpawnText.text = "CLICK THE NOTE!";
		textSpawn.transform.parent = questItNote.transform;

		// put the words on the note
		text = questItNote.GetComponentInChildren<Text> ();
		text.text = "Pick up your visor.";		// lmao silly and redundant

		// quest info itself
		title = ("Tutorial");
		id = (666);
		description = ("Get through the tutorial.");

		// visor, rip soon
		visor = GameObject.Find("visor");

		// interaction settings, rip soon
		intSet = visor.GetComponentInChildren<InteractionSettings> ();
		
	}

	void Update() {

		// if it's equipped, do the thing
		if (intSet.IsEquipped == true) {
			visorPickedUp ();
			Destroy (visor);
			Destroy (textSpawn);
		}
	}

	void visorPickedUp(){

		// yay! step one done
		visorEquipped = true;

		// change the text
		text.text = "Walk around your room. See the sights.";

		// stick the note to the screen
		questItNote.GetComponentInChildren<QuestItNoteFunction>().StickToScreen();


		PlayerControllerNew controls = controller.GetComponent<PlayerControllerNew> ();
		controls.InitZoomOutMode ();
	}
}
