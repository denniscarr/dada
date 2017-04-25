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
	GameObject questItNote2;
	Text txtInfo;
	GameObject colostomyBag;
	public GameObject inRoomItem;
	public GameObject worldItem;

	// bools for the things having been done
	public bool visorEquipped;
	public bool roomEntered;
	public bool thingMoved;
	public bool visorExited;
	public bool tabPressed;
	public bool thingUsedOrMoved;
	public bool itemAdded;
	public bool jumpedOff;
	public bool finished;

	void Start () {

		// finding the quest manager
		manager = GameObject.Find ("QuestManager").GetComponent<QuestManager> ();

		// player controller
		controller = GameObject.Find("PlayerInRoom");
		controller.AddComponent<QuestObject> ();

		// quest it note
		questItNote = Instantiate(Resources.Load ("QuestItNote")) as GameObject;

		// spawn text over the questitnote so they know to go to it
		textSpawn = Instantiate (Resources.Load("TextSpawn") as GameObject);
		textSpawn.transform.position = new Vector3 (controller.transform.position.x, controller.transform.position.y + 10, controller.transform.position.z + 3);
		TextMesh textSpawnText = textSpawn.GetComponent<TextMesh> ();
		textSpawnText.text = "CLICK THE NOTE!";
		//textSpawn.transform.parent = questItNote.transform;

		// put the words on the note
		text = questItNote.GetComponentInChildren<Text> ();
		text.text = "Pick up your visor. That gray thing over there.";		// lmao silly and redundant

		// quest info itself
		title = ("Tutorial");
		id = (666);
		description = ("Get through the tutorial.");

		GameObject level = GameObject.Find ("Level 0");
		float highPoint = level.GetComponent<Level> ().highestPoint;

		visor = Instantiate (Resources.Load ("visor", typeof(GameObject))) as GameObject;
		visor.transform.position = new Vector3 (level.transform.position.x, level.transform.position.y + highPoint, level.transform.position.z);
		questItNote.transform.position = visor.transform.position;
		visor.SetActive (false);

		// interaction settings, rip soon
		intSet = visor.GetComponentInChildren<InteractionSettings> ().GetComponent<InteractionSettings>();

		controls = controller.GetComponent<PlayerControllerNew> ();
	}

	void Update() {

		// if visor is equipped, then...you know, destroy everything and move to the next thing
		//EquippableFinder finder = GameObject.Find("FindEquip").GetComponent<EquippableFinder>();
		Transform player = GameObject.Find("Player").GetComponent<Transform>();

		if (questItNote.transform.parent == GameObject.Find ("Equip Reference").transform && visorEquipped == false) {
			visor.SetActive (true);
		}

		if (intSet._carryingObject == player) {
			visorPickedUp();
		}

		// if it's equipped and you walk around, do the thing
		if (visorEquipped &&
			(Input.GetKeyDown(KeyCode.W) ||
				Input.GetKeyDown(KeyCode.A) ||
				Input.GetKeyDown(KeyCode.S) ||
				Input.GetKeyDown(KeyCode.D)) && !roomEntered) {
			enterRoom ();
		}

		// once you've entered the room and picked up the thing
		if (roomEntered) {
			//if (inRoomItem.GetComponentInChildren<InteractionSettings> ().carryingObjectCarryingObject) {
			//if (inRoomItem != null && txtInfo.text == "Click to move bag."){
			GameObject inroomObjects = GameObject.Find("INROOMOBJECTS");
			if (colostomyBag.transform.parent == inroomObjects.transform || questItNote2.transform.parent == inroomObjects.transform){
				if (Input.GetKeyDown (KeyCode.Tab)) {
					nowExit ();
				}
			}
		}

		// great, now exit the visor and explore the world
		if (thingMoved && roomEntered && !tabPressed) {
			if (controls.mode == ControlMode.ZOOM_IN_MODE) {
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

		// destroy it bc its now useless
		Destroy(visor);
		Destroy (textSpawn); // for good measure

		// change the text
		text.text = "Walk around your room. See the sights. Take your time.";

		// stick the note to the screen
		questItNote.GetComponentInChildren<QuestItNoteFunction>().StickToScreen();

		controls.mode = ControlMode.ZOOM_OUT_MODE;
	}

	void enterRoom(){

		// mark off the bool
		roomEntered = true;

		GameObject fakeVisor = GameObject.Find ("FakeVisor");
		// spawn something for the player to use
		colostomyBag = Instantiate (Resources.Load("Pickups/Colostomy Bag")) as GameObject;
		colostomyBag.transform.position = new Vector3 (fakeVisor.transform.position.x, fakeVisor.transform.position.y - 2, fakeVisor.transform.position.z);

		inRoomItem = colostomyBag;

		// spawn a new quest it note for good measure
		questItNote2 = Instantiate(Resources.Load("QuestItNote")) as GameObject;
		questItNote2.transform.position = fakeVisor.transform.position;
//		questItNote2.transform.parent = platform.transform;
		questItNote2.GetComponentInChildren<Text> ().text = "Now that you've walked around, try to move the Colostomy Bag. Click.";

		// change the text
		text.text = "Now that you've walked around, try to move the Colostomy Bag. Click.";

		// change the in room text, too
		txtInfo = GameObject.Find("txtInfo").GetComponent<Text>();
		txtInfo.text = "Click to move bag.";

	}

	void nowExit(){
		// mark off the bool
		thingMoved = true;

		inRoomItem = null;

		Destroy (questItNote2);
		txtInfo.text = "Click platform. Press tab.";

		// change the text
		text.text = "Click on the viewing platform and press tab.";

	}

	void interactWithAThing(){

		//mark off the bool
		tabPressed = true;

		controls.mode = ControlMode.ZOOM_OUT_MODE;

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
				if (jumpedOff == true) {
					finished = true;
					if (finished == true) {
						Destroy (this);
					}
				}
			}
		}
	}
}
