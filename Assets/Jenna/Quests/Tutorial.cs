using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialState{
	PICKUP_NOTE = 0,
	PICKUP_VISOR = 1,
	USE_PLATFORM = 2,
	DRAG_NOTE_IN = 3,
	THROW_NOTE_OUT = 4,
	PRESS_TAB = 5,
	EQUIP_GUN = 6,
	USE_GUN = 7,
	DROP_GUN = 8,
	SKIP_TUTORIAL = 9,
}

public class Tutorial : Quest {

	public TutorialState state = TutorialState.PICKUP_NOTE;

	Writer platformWriter;
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
	Text QuestItNoreText;

	// temporary items
	GameObject visor;
	GameObject textSpawn;
	GameObject questItNote2;
	Text txtInfo;
	GameObject colostomyBag;
//	public GameObject inRoomItem;
//	public GameObject worldItem;

//	// bools for the things having been done
//	public bool visorEquipped;
//	public bool roomEntered;
//	public bool thingMoved;
//	public bool visorExited;
//	public bool tabPressed;
//	public bool thingUsedOrMoved;
//	public bool itemAdded;
//	public bool jumpedOff;
//	public bool finished;

	Transform player;

	GameObject go_AK12;

	MouseControllerNew mouseControllerNew;

	//Writer writer;

	int numPressTab = 0;

	void Start () {
		if(state == TutorialState.SKIP_TUTORIAL){
			this.enabled = false;
			GameObject.FindObjectOfType<LevelManager>().isTutorialCompleted = true;
			return;
		}
		GetComponent<QuestManager>().enabled =false;
		GetComponent<QuestFinderScript>().enabled = false;
		GetComponent<QuestBuilderScript>().enabled = false;
		GetComponent<PickupQuest>().enabled = false;
		
		mouseControllerNew = GameObject.FindObjectOfType<MouseControllerNew>();
		//writer = mouseControllerNew.writer;

		player = GameObject.Find("Player").transform;
		platformWriter = GameObject.Find("Viewing Platform").AddComponent<Writer>();
		// finding the quest manager
		manager = GameObject.Find ("QuestManager").GetComponent<QuestManager> ();

		// player controller
		controller = GameObject.Find("PlayerInRoom");
		controller.AddComponent<QuestObject> ();


		questItNote = Instantiate(Resources.Load("QuestItNote", typeof (GameObject))) as GameObject;
		Transform visorNode = GameObject.Find("INROOMOBJECTS").transform;
		questItNote.transform.parent = visorNode.transform;
		questItNote.transform.localPosition = new Vector3(
			Random.Range(-1.3f, 4.1f),
			Random.Range(1f, 4.1f),
			2.5f);

		questItNote.transform.localRotation = Quaternion.Euler(new Vector3(
			0f,
			0f,
			Random.Range(-1f, 1f)));
		//QuestItNoreText = questItNote.GetComponentInChildren<Text>();
		//QuestItNoreText.text = "Press TAB 5 times";
		questItNote.GetComponentInChildren<QuestItNoteFunction>().StickToScreen();

		// quest it note
//		questItNote = Instantiate(Resources.Load ("QuestItNote")) as GameObject;
//		//questItNote.transform.localScale *= 2;
//		questItNote.transform.position = controller.transform.position + controller.transform.forward*3;
//
//		// spawn text over the questitnote so they know to go to it
//		textSpawn = Instantiate (Resources.Load("TextSpawn") as GameObject);
//		textSpawn.transform.position = new Vector3 (controller.transform.position.x, controller.transform.position.y + 10, controller.transform.position.z)
//			+ controller.transform.forward;
//		
//		
//		TextMesh textSpawnText = textSpawn.GetComponent<TextMesh> ();
//		textSpawnText.text = "CLICK THE NOTE!";
		//textSpawn.transform.parent = questItNote.transform;

		// put the words on the note
		QuestItNoreText = questItNote.GetComponentInChildren<Text> ();
		QuestItNoreText.text = "Pick up your visor. That gray thing over there.";		// lmao silly and redundant

		// quest info itself
		title = ("Tutorial");
		id = (666);
		description = ("Get through the tutorial.");

		GameObject level = GameObject.Find ("Level 0");
		float highPoint = level.GetComponent<Level> ().highestPoint;

		visor = Instantiate (Resources.Load ("Visor", typeof(GameObject))) as GameObject;
		visor.transform.position = new Vector3 (level.transform.position.x, level.transform.position.y + highPoint, level.transform.position.z);
		//questItNote.transform.position = visor.transform.position;
		visor.transform.localScale *= 1;
		visor.SetActive (false);

		// interaction settings, rip soon
		intSet = visor.GetComponentInChildren<InteractionSettings> ();

		controls = controller.GetComponent<PlayerControllerNew> ();


	}

	void Update() {
		switch(state){
		case TutorialState.PICKUP_NOTE:OnPickUpNote();break;
		case TutorialState.PICKUP_VISOR:OnPickUpVisor();break;
		case TutorialState.USE_PLATFORM:OnUsePlatform();break;
		case TutorialState.DRAG_NOTE_IN:OnDragNoteIn();break;
		case TutorialState.THROW_NOTE_OUT:OnThrowNoteOut();break;
		case TutorialState.PRESS_TAB:OnPressTab();break;
		case TutorialState.EQUIP_GUN:OnEquipGun();break;
		case TutorialState.USE_GUN:OnUseGun();break;
		case TutorialState.DROP_GUN:OnDropGun();break;
		}


		//questItNote.transform.position = 
		//questItNote.transform.LookAt(controller.transform,Vector3.forward);
		// if visor is equipped, then...you know, destroy everything and move to the next thing
		//EquippableFinder finder = GameObject.Find("FindEquip").GetComponent<EquippableFinder>();
		//Transform player = GameObject.Find("Player").GetComponent<Transform>();
//		// if it's equipped and you walk around, do the thing
//		if (visorEquipped &&
//			(Input.GetKeyDown(KeyCode.W) ||
//				Input.GetKeyDown(KeyCode.A) ||
//				Input.GetKeyDown(KeyCode.S) ||
//				Input.GetKeyDown(KeyCode.D)) && !roomEntered) {
//			enterRoom ();
//		}

//		// great, now exit the visor and explore the world
//		if (thingMoved && roomEntered && !tabPressed) {
//			if (controls.mode == ControlMode.ZOOM_IN_MODE) {
//				if (Input.GetKeyDown (KeyCode.Tab)) {
//					interactWithAThing ();
//				}
//			}
//		}
//
//		// this nullifies the chance that tab has been pressed early because, like,
//		//tabpressed can only be true if you've interacted with a thing
//		if (tabPressed) {
//			if (!worldItem == null) {
//				text.text = ("Now add" + " " + worldItem.name + " " + "to your inventory. Grab it, press tab, and drop.");
//
//				if (worldItem.GetComponentInChildren<InteractionSettings> ().IsEquipped) {
//					manager.allQuestsCompleted = true;
//					text.text = "You've learned the basics. Now jump off the world. Go on.";
//				}
//			}
//		}
	}

	void OnPickUpNote(){
		//if (questItNote.transform.parent == GameObject.Find ("Equip Reference").transform) {
			visor.SetActive (true);
			state = TutorialState.PICKUP_VISOR;
		//}
	}

	void OnPickUpVisor(){
		if (intSet._carryingObject == player) {
			Debug.Log("tutorial -- pick up visor");
			// yay! step one done
			//visorEquipped = true;
			state = TutorialState.USE_PLATFORM;

			// destroy it bc its now useless
			Destroy(visor);
			Destroy(textSpawn); // for good measure

			// change the text
			QuestItNoreText.text = "Find and click the observation platform.";

			// stick the note to the screen
			questItNote.
                GetComponentInChildren<QuestItNoteFunction>().StickToScreen();

			controls.Mode = ControlMode.ZOOM_OUT_MODE;
			//platformWriter.WriteAtPoint("Click me to revert to visor mode",platformWriter.transform.position+new Vector3(0,0,1));

		}
	}

	void OnUsePlatform(){
		mouseControllerNew.writer.WriteAtPoint("Find and click the observation platform.", mouseControllerNew.textPosition);
		if (Input.GetMouseButtonDown(0)){ // if left button pressed...
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
				if(hit.collider.transform.parent.name.Equals("Viewing Platform")){
					state = TutorialState.DRAG_NOTE_IN;
					QuestItNoreText.text = "Drag me into the visor with your mouse.";
				}
				// the object identified by hit.transform was clicked
				// do whatever you want
			}
		}
	}

	void OnDragNoteIn(){
		mouseControllerNew.writer.WriteAtPoint("Drag the note into the visor with your mouse.", mouseControllerNew.textPosition);
		if(questItNote.transform.parent.name.Equals("INROOMOBJECTS")){
			state = TutorialState.THROW_NOTE_OUT;
			QuestItNoreText.text = "Drag me out of your visor into the world.";
			//questItNote.GetComponentInChildren<QuestItNoteFunction>().StickToScreen();//useless?
		}
	}

	void OnThrowNoteOut(){
		mouseControllerNew.writer.WriteAtPoint("Drag note out of your visor into the world.", mouseControllerNew.textPosition);
		Debug.Log(questItNote.transform.parent);
		if(questItNote.transform.parent == null){
			state = TutorialState.PRESS_TAB;
			Destroy(questItNote);
			mouseControllerNew.writer.WriteAtPoint("Press TAB 5 times.", mouseControllerNew.textPosition);
			//text.text = "Press TAB 5 times";
			// make the questit note
			Transform visorNode = GameObject.Find("INROOMOBJECTS").transform;
			questItNote = Instantiate(Resources.Load("QuestItNote", typeof (GameObject))) as GameObject;
			questItNote.transform.parent = visorNode.transform;
			questItNote.transform.localPosition = new Vector3(
				Random.Range(-1.3f, 4.1f),
				Random.Range(1f, 4.1f),
				2.5f);

			questItNote.transform.localRotation = Quaternion.Euler(new Vector3(
				0f,
				0f,
				Random.Range(-1f, 1f)));
			QuestItNoreText = questItNote.GetComponentInChildren<Text>();
			QuestItNoreText.text = "Press TAB 5 times.";
			questItNote.GetComponentInChildren<QuestItNoteFunction>().StickToScreen();

			//questItNote.GetComponentInChildren<QuestItNoteFunction>().StickToScreen();
			//controller
		}
	}

	void OnPressTab(){
		mouseControllerNew.writer.WriteAtPoint("Press TAB "+ (5 - numPressTab).ToString() +" times.", mouseControllerNew.textPosition);
		if(Input.GetKeyDown(KeyCode.Tab)){
			numPressTab ++;
			int rest = 5 - numPressTab;
			if(rest > 0){
				
				QuestItNoreText.text = "Press TAB "+ rest.ToString() +" times.";
			}else{
				state = TutorialState.EQUIP_GUN;//go_AK12 = Resources.Load("Pickups/AK12") as GameObject;
				go_AK12 = Instantiate(Resources.Load<GameObject>("Pickups/AK12"));
				go_AK12.transform.position = player.transform.position + player.transform.forward*3;
				//instantiate gun
				//mouseControllerNew.writer.WriteAtPoint("Press TAB 5 times", mouseControllerNew.textPosition);
				QuestItNoreText.text = "Left click to equip the guns.";
			}


		}

	}

	void OnEquipGun(){
		if(go_AK12.GetComponentInChildren<InteractionSettings>().IsEquipped){
			state = TutorialState.USE_GUN;
			QuestItNoreText.text = "Right click to use the gun.";
		}


	}

	void OnUseGun(){
		if(go_AK12.GetComponentInChildren<InteractionSettings>().IsEquipped
			&& Input.GetMouseButtonDown(1)){
			state = TutorialState.DROP_GUN;
			QuestItNoreText.text = "Press G to drop the gun.";
		}
	}

	void OnDropGun(){
		if(!go_AK12.GetComponentInChildren<InteractionSettings>().IsEquipped){
			Debug.Log("Tutorial is done.");
			GameObject.FindObjectOfType<LevelManager>().isTutorialCompleted = true;
			//Destroy(questItNote);
			QuestItNoreText.text = "Be brave to jump off.";
			this.enabled = false;
		}
	}




	/// <summary>
	/// jenna's great work below
	/// </summary>

	void visorPickedUp(){

		// yay! step one done
		//visorEquipped = true;

		// destroy it bc its now useless
		Destroy(visor);
		Destroy (textSpawn); // for good measure

		// change the text
		QuestItNoreText.text = "Click the platform to go back.";

		// stick the note to the screen
		questItNote.GetComponentInChildren<QuestItNoteFunction>().StickToScreen();

		controls.Mode = ControlMode.ZOOM_OUT_MODE;
	}

	void enterRoom(){
		Debug.Log("enter room");

		// mark off the bool
		//roomEntered = true;

		GameObject fakeVisor = GameObject.Find ("FakeVisor");
		// spawn something for the player to use
		colostomyBag = Instantiate (Resources.Load("Pickups/Colostomy Bag")) as GameObject;
		colostomyBag.transform.position = new Vector3 (fakeVisor.transform.position.x, fakeVisor.transform.position.y - 2, fakeVisor.transform.position.z);

		//inRoomItem = colostomyBag;

		// spawn a new quest it note for good measure
		questItNote2 = Instantiate(Resources.Load("QuestItNote")) as GameObject;
		questItNote2.transform.position = fakeVisor.transform.position;
//		questItNote2.transform.parent = platform.transform;
		questItNote2.GetComponentInChildren<Text> ().text = "Now that you've walked around, try to move the Colostomy Bag. Click.";

		// change the text
		QuestItNoreText.text = "Now that you've walked around, try to move the Colostomy Bag. Click.";

		// change the in room text, too
		txtInfo = GameObject.Find("txtInfo").GetComponent<Text>();
		txtInfo.text = "Click to move bag.";

	}

	void nowExit(){
		// mark off the bool
		//thingMoved = true;

		//inRoomItem = null;

		Destroy (questItNote2);
		txtInfo.text = "Click platform. Press tab.";

		// change the text
		QuestItNoreText.text = "Click on the viewing platform and press tab.";

	}

	void interactWithAThing(){

		//mark off the bool
		//tabPressed = true;

		controls.Mode = ControlMode.ZOOM_OUT_MODE;

		// instantiate an item and put it somewhere else
		GameObject newThing = Instantiate(Resources.Load("Pickups/Bathroom Sink", typeof(GameObject))) as GameObject;
		newThing.transform.position = new Vector3 (this.transform.position.x - 5, this.transform.position.y + 15, this.transform.position.z + 10);

		// change the text
		QuestItNoreText.text = "Find something in the world. Press right mouse button to interact.";

	}

	void OnCollisionEnter (Collision col){
		if (col.gameObject.name == "Killzone") {
			if (manager.allQuestsCompleted) {
//				jumpedOff = true;
//				if (jumpedOff == true) {
//					finished = true;
//					if (finished == true) {
//						Destroy (this);
//					}
//				}
			}
		}
	}
}
