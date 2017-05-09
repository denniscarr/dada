using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public enum TutorialState{
	BEFORE_LAND		= -1,
	PURCHASE_VISOR 	= 0,
	PICKUP_VISOR	= 1,
	USE_PLATFORM	= 2,
	DRAG_NOTE_IN	= 3,
	THROW_NOTE_OUT	= 4,
	PRESS_TAB 		= 5,
	EQUIP_GUN		= 6,
	USE_GUN 		= 7,
	DROP_GUN		= 8,
	DRAG_GUN_IN		= 9,
	GRAIl_SPAWN 	= 10,
	SKIP_TUTORIAL	= 11,
}

public class Tutorial : Quest {

	private TutorialState state = TutorialState.BEFORE_LAND;

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

	Transform player;

	GameObject go_AK12;

	MouseControllerNew mouseControllerNew;

	GameObject grail;
	//Writer writer;

	int numPressTab = 0;

	void Start () {

		GetComponent<QuestManager>().enabled = false;
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

		// quest info itself
		title = ("Tutorial");
		id = (666);
		description = ("Get through the tutorial.");

		GameObject level = GameObject.Find ("Level 0");
		float highPoint = level.GetComponent<Level> ().highestPoint + 3f;

		visor = Instantiate (Resources.Load ("Visor", typeof(GameObject))) as GameObject;
		//Debug.Log(visor);
		visor.transform.position = new Vector3 (level.transform.position.x, level.transform.position.y + highPoint, level.transform.position.z);


		// interaction settings, rip soon
		intSet = visor.GetComponentInChildren<InteractionSettings> ();

		controls = controller.GetComponent<PlayerControllerNew> ();

	}

	void Update() {
		switch(state){
		case TutorialState.BEFORE_LAND:CheckTabPressingToSkipTutorial();break;
		case TutorialState.PURCHASE_VISOR:OnPurchaseVisor();break;
		case TutorialState.PICKUP_VISOR:OnPickUpVisor();break;
		case TutorialState.USE_PLATFORM:OnUsePlatform();break;
		case TutorialState.DRAG_NOTE_IN:OnDragNoteIn();break;
		case TutorialState.THROW_NOTE_OUT:OnThrowNoteOut();break;
		case TutorialState.PRESS_TAB:OnPressTab();break;
		case TutorialState.EQUIP_GUN:OnEquipGun();break;
		case TutorialState.USE_GUN:OnUseGun();break;
		case TutorialState.DROP_GUN:OnDropGun();break;
		case TutorialState.DRAG_GUN_IN:OnDragGun();break;
		case TutorialState.GRAIl_SPAWN:OnSpawnGrail();break;
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

	void InitFirstNode(){
		if(state == TutorialState.BEFORE_LAND){
			Debug.Log("first node init");


			OnDisappearComplete("Left click to purchase the visor. That grey thing over there.");
			state = TutorialState.PURCHASE_VISOR;
		}

	}

	void AddNewNote(string notes){
        //wait to add do tween
        QuestItNoreText.DOText("",0.1f).OnComplete(()=>OnDisappearComplete(notes));
		//MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
//		foreach(MeshRenderer mr in meshRenderers){
//			
//			mr.material.DOFade(0f,1.0f);// DOFade(0,1f).SetDelay(1f);
//		}
		//questItNote.GetComponent<s>

	}

	void OnDisappearComplete(string notes){
		if(questItNote){
			Destroy(questItNote);
		}
		questItNote = null;
		questItNote = Instantiate(Resources.Load("QuestItNote", typeof (GameObject))) as GameObject;
		Services.AudioManager.PlaySFX (Services.AudioManager.tutorialTones [Random.Range (0, Services.AudioManager.tutorialTones.Length)]);
		QuestItNoreText = questItNote.GetComponentInChildren<Text>();
		questItNote.GetComponentInChildren<QuestItNoteFunction>().StickToScreen();
        questItNote.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
        questItNote.transform.DOScale(Vector3.one, 0.4f);
		QuestItNoreText.DOText(notes,1f);
		//QuestItNoreText.text = notes;

	}

	void OnPurchaseVisor(){//buy
		if(CheckTabPressingToSkipTutorial()){
			return;
		}

		if (visor.GetComponentInChildren<InteractionSettings>().isOwnedByPlayer) {
			AddNewNote("Good. Now left click again to pick it up.");
			//QuestItNoreText.text = "Pick up your visor.";		// lmao silly and redundant
			state = TutorialState.PICKUP_VISOR;
		}
	}

	bool CheckTabPressingToSkipTutorial(){
		if(Input.GetKeyDown(KeyCode.Tab)){
			state = TutorialState.SKIP_TUTORIAL;

			// destroy it bc its now useless
			Destroy(visor);
			Destroy(textSpawn); // for good measure
			GameObject.FindObjectOfType<LevelManager>().isTutorialCompleted = true;

			Debug.Log("Skip tutorial");
			OnDisappearComplete("You skiped the tutorial. Let's jump to the next stage.");
			this.enabled = false;
			return true;
		}
		return false;
	}

	void OnPickUpVisor(){

		if (intSet._carryingObject == player) {
			// yay! step one done
//			state = TutorialState.USE_PLATFORM;
//
//			// destroy it bc its now useless
//			Destroy(visor);
//			Destroy(textSpawn); // for good measure
//
//			AddNewNote("Now find and click the observation platform.");
//
//			controls.Mode = ControlMode.ZOOM_OUT_MODE;
			//platformWriter.WriteAtPoint("Click me to revert to visor mode",platformWriter.transform.position+new Vector3(0,0,1));

		}
	}

	void TurnUpZoomOutMode(){
		state = TutorialState.USE_PLATFORM;

		// destroy it bc its now useless
		Destroy(visor);
		Destroy(textSpawn); // for good measure

		AddNewNote("Now find and click the observation platform.");

		controls.Mode = ControlMode.ZOOM_OUT_MODE;
	}

	void OnUsePlatform(){
		mouseControllerNew.writer.WriteAtPoint("Find and click the observation platform.", mouseControllerNew.textPosition);
		if (Input.GetMouseButtonDown(0)){ // if left button pressed...
			Ray ray = GameObject.Find("UpperCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
				if(hit.collider.transform.parent.name.Equals("Viewing Platform")){
					state = TutorialState.THROW_NOTE_OUT;
					mouseControllerNew.writer.WriteAtPoint("Drag this note out of your visor into the world.", mouseControllerNew.textPosition);
					AddNewNote("Drag this note out of your visor into the world.");
					//QuestItNoreText.text = "Drag note out of your visor into the world.";
				}

			}
		}
	}

	void OnDragNoteIn(){
		mouseControllerNew.writer.WriteAtPoint("Now drag this note back into your visor with the mouse.", mouseControllerNew.textPosition);
		if(Input.GetMouseButtonUp(0) && questItNote.transform.parent && questItNote.transform.parent.name.Equals("INROOMOBJECTS")){
			state = TutorialState.PRESS_TAB;
			//QuestItNoreText.text = "Press Tab 5 times";
			AddNewNote("Press Tab 5 times");
			//questItNote.GetComponentInChildren<QuestItNoteFunction>().StickToScreen();//useless?
		}
	}

	void OnThrowNoteOut(){
		mouseControllerNew.writer.WriteAtPoint("Drag this note into your visor with the mouse.", mouseControllerNew.textPosition);
		//Debug.Log(questItNote.transform.parent);
		if(questItNote.transform.parent == null){
			state = TutorialState.DRAG_NOTE_IN;
			//AddNewNote("Drag the note into your visor with the mouse.");
			//AddNewNote("Drag the note into the visor with your mouse.");
			QuestItNoreText.text = "Drag the note into your visor with the mouse.";
			//questItNote.GetComponentInChildren<QuestItNoteFunction>().StickToScreen();

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
				go_AK12.transform.position = player.transform.position + player.transform.forward*3 + Vector3.up*2f;
				AddNewNote("Purchase and equip the gun.");
				//QuestItNoreText.text = "Purchase and equip the gun.";
			}

		}

	}

	void OnEquipGun(){
		if(go_AK12.GetComponentInChildren<InteractionSettings>().IsEquipped){
			state = TutorialState.USE_GUN;
			AddNewNote("Right click on the gun to use it.");
			//QuestItNoreText.text = "Right click on the gun to use it.";
		}

	}

	void OnUseGun(){
		if(go_AK12.GetComponentInChildren<InteractionSettings>().IsEquipped
			&& Input.GetMouseButtonDown(1)){
			state = TutorialState.DROP_GUN;
			AddNewNote("Press G to drop the gun.");
			//QuestItNoreText.text = "Press G to drop the gun.";
		}
	}

	void OnDropGun(){
		if(!go_AK12.GetComponentInChildren<InteractionSettings>().IsEquipped){
			AddNewNote("Try to store the gun in your visor.");			
			//QuestItNoreText.text = "Try to store the gun in your visor.";
			state = TutorialState.DRAG_GUN_IN;
            //Destroy(questItNote);
           
		}
	}

	void OnDragGun(){
		if(Input.GetMouseButtonUp(0) && go_AK12.transform.parent && go_AK12.transform.parent.name.Equals("INROOMOBJECTS")){
			state = TutorialState.GRAIl_SPAWN;
			AddNewNote("Try to equip the grail.");	
			//QuestItNoreText.text = "Try to store the gun in your visor.";
		}
	}

	void OnSpawnGrail(){
		Debug.Log("Tutorial is done.");
		GameObject.FindObjectOfType<LevelManager>().isTutorialCompleted = true;
		GetComponent<QuestManager>().enabled = true;
		GetComponent<QuestFinderScript>().enabled = true;
		GetComponent<QuestBuilderScript>().enabled = true;
		GetComponent<PickupQuest>().enabled = true;
		//QuestItNoreText.text = "Pursue the grail.";
        GameObject.Find("Bootstrapper").GetComponent<GrailSpawner>().SpawnGrail();
		this.enabled = false;
	}


	/// <summary>
	/// jenna's great work below
	/// </summary>
//	void visorPickedUp(){
//
//		// yay! step one done
//		//visorEquipped = true;
//
//		// destroy it bc its now useless
//		Destroy(visor);
//		Destroy (textSpawn); // for good measure
//
//		// change the text
//		QuestItNoreText.text = "Click the platform to go back.";
//
//		// stick the note to the screen
//		questItNote.GetComponentInChildren<QuestItNoteFunction>().StickToScreen();
//
//		controls.Mode = ControlMode.ZOOM_OUT_MODE;
//	}

//	void enterRoom(){
//		Debug.Log("enter room");
//
//		// mark off the bool
//		//roomEntered = true;
//
//		GameObject fakeVisor = GameObject.Find ("FakeVisor");
//		// spawn something for the player to use
//		colostomyBag = Instantiate (Resources.Load("Pickups/Colostomy Bag")) as GameObject;
//		colostomyBag.transform.position = new Vector3 (fakeVisor.transform.position.x, fakeVisor.transform.position.y - 2, fakeVisor.transform.position.z);
//
//		//inRoomItem = colostomyBag;
//
//		// spawn a new quest it note for good measure
//		questItNote2 = Instantiate(Resources.Load("QuestItNote")) as GameObject;
//		questItNote2.transform.position = fakeVisor.transform.position;
////		questItNote2.transform.parent = platform.transform;
//		questItNote2.GetComponentInChildren<Text> ().text = "Now that you've walked around, try to move the Colostomy Bag. Click.";
//
//		// change the text
//		QuestItNoreText.text = "Now that you've walked around, try to move the Colostomy Bag. Click.";
//
//		// change the in room text, too
//		txtInfo = GameObject.Find("txtInfo").GetComponent<Text>();
//		txtInfo.text = "Click to move bag.";
//
//	}


//	void nowExit(){
//		// mark off the bool
//		//thingMoved = true;
//
//		//inRoomItem = null;
//
//		Destroy (questItNote2);
//		txtInfo.text = "Click platform. Press tab.";
//
//		// change the text
//		QuestItNoreText.text = "Click on the viewing platform and press tab.";
//
//	}
//
//	void interactWithAThing(){
//
//		//mark off the bool
//		//tabPressed = true;
//
//		controls.Mode = ControlMode.ZOOM_OUT_MODE;
//
//		// instantiate an item and put it somewhere else
//		GameObject newThing = Instantiate(Resources.Load("Pickups/Bathroom Sink", typeof(GameObject))) as GameObject;
//		newThing.transform.position = new Vector3 (this.transform.position.x - 5, this.transform.position.y + 15, this.transform.position.z + 10);
//
//		// change the text
//		QuestItNoreText.text = "Find something in the world. Press right mouse button to interact.";
//
//	}
//
//	void OnCollisionEnter (Collision col){
//		if (col.gameObject.name == "Killzone") {
//			if (manager.allQuestsCompleted) {
////				jumpedOff = true;
////				if (jumpedOff == true) {
////					finished = true;
////					if (finished == true) {
////						Destroy (this);
////					}
////				}
//			}
//		}
//	}

}
