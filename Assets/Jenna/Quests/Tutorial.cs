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

	// controller also
	PlayerControllerNew controls;

	// quest it note
	GameObject questItNote;
	Text text;

	// temporary items
	GameObject visor;
	GameObject textSpawn;
	GameObject inRoomItem;
	GameObject worldItem;

	// bools for the things having been done
	bool visorEquipped;
	bool roomEntered;
	bool thingMoved;
	bool visorExited;
	bool tabPressed;
	bool thingUsedOrMoved;
	bool itemAdded;
	bool jumpedOff;
	bool finished;

	void OnPostRender () {

		//change the text on top of the screen to "press tab"
		// or just do a text spawn that says "press tab"
		// need to change locations of text too

		// finding the quest manager
		manager = GameObject.Find ("QuestManager").GetComponent<QuestManager> ();

		// player controller
		//controller = GameObject.Find ("PlayerControllerNew");
		controller = GameObject.Find("PlayerInRoom");
		//Debug.Log (controller.name);
		controller.AddComponent<QuestObject> ();
//		GameObject nodes = GameObject.Find ("UpperNode");
//		nodes.SetActive (false);


//		controls = controller.GetComponent<PlayerControllerNew> ();
//		//Debug.Log (controls.gameObject);
//		//controls.InitZoomInMode ();
//		//controls.ZoomOutUpdate ();
//		controls.mode = ControlMode.ZOOM_OUT_MODE;

		// quest it note
		questItNote = Instantiate(Resources.Load ("QuestItNote")) as GameObject;
		questItNote.transform.position = new Vector3
			(controller.transform.position.x,
			controller.transform.position.y + 20,
			controller.transform.position.z + 0.5f);

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
		intSet = visor.GetComponentInChildren<InteractionSettings> ().GetComponent<InteractionSettings>();

		controls = controller.GetComponent<PlayerControllerNew> ();
		//Debug.Log (controls.gameObject);
		//controls.InitZoomInMode ();
		//controls.ZoomOutUpdate ();
		//controls.mode = ControlMode.ZOOM_OUT_MODE;

		inRoomItem = GameObject.Find ("Colostomy Bag (1)");
	}

	void Update() {

		// if it's equipped, do the thing
		if (intSet.IsEquipped == true && visor.activeInHierarchy) {
			visorPickedUp ();
			Destroy (visor);
			Destroy (textSpawn);
		}

		// if it's equipped and you walk around, do the thing
		if (visorEquipped &&
			(Input.GetKeyDown(KeyCode.W) ||
				Input.GetKeyDown(KeyCode.A) ||
				Input.GetKeyDown(KeyCode.S) ||
				Input.GetKeyDown(KeyCode.D))
		&& !roomEntered) {
			enterRoom ();
		}

		// once you've entered the room and picked up the thing
		if (roomEntered) {
			if (inRoomItem.GetComponentInChildren<InteractionSettings> ().carryingObjectCarryingObject) {
				nowExit ();
			}
		}

		// great, now exit the visor and explore the world
		if (thingMoved && roomEntered && !tabPressed) {
			if (controller.GetComponent<PlayerControllerNew> ().mode == ControlMode.ZOOM_IN_MODE) {
				if (Input.GetKeyDown (KeyCode.Tab)) {
					interactWithAThing ();
				}
			}
		}

		// this nullifies the chance that tab has been pressed early because, like,
		//tabpressed can only be true if you've interacted with a thing
		if (tabPressed) {
			if (!worldItem == null) {
				text.text = ("Now add" + " " + worldItem.name + " " + "to your inventory. Grab it, press tab, and drop.");

				if (worldItem.GetComponentInChildren<InteractionSettings> ().IsEquipped) {
					manager.allQuestsCompleted = true;
					text.text = "You've learned the basics. Now jump off the world. Go on.";
				}
			}
		}
	}

	void visorPickedUp(){

		// yay! step one done
		visorEquipped = true;

		// change the text
		text.text = "Walk around your room. See the sights. Take your time.";

		// stick the note to the screen
		questItNote.GetComponentInChildren<QuestItNoteFunction>().StickToScreen();

		//controls.InitZoomOutMode ();
		controller.SetActive(true);
	}

	void enterRoom(){

		// mark off the bool
		roomEntered = true;

		// change the text
		text.text = "Now that you've walked around, try to move the Colostomy Bag.";
	}

	void nowExit(){

		// mark off the bool
		thingMoved = true;

		// change the text
		text.text = "Click on the viewing platform and press tab.";

	}

	void interactWithAThing(){

		//mark off the bool
		tabPressed = true;

		// instantiate an item and put it somewhere else
		GameObject newThing = Instantiate(Resources.Load("Pickups/Bathroom Sink", typeof(GameObject))) as GameObject;
		newThing.transform.position = new Vector3 (this.transform.position.x - 5, this.transform.position.y + 15, this.transform.position.z + 10);

		// change the text
		text.text = "Find something in the world. Press right mouse button to interact.";

	}

	void OnCollisionEnter (Collision col){
		if (col.gameObject.name == "Killzone") {
			if (manager.allQuestsCompleted) {
				jumpedOff = true;
				if (jumpedOff = true) {
					finished = true;
					if (finished = true) {
						Destroy (this);
					}
				}
			}
		}
	}
}
