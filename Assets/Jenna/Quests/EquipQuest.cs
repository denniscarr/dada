//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//
//// LOCATION: QUEST MANAGER
//// LOCATION: QUEST GAMEOBJECT (description serves as "giving of quest")
//
//public class EquipQuest : Quest
//{
//
//    // find the object
//    public GameObject parentObject;
//    Transform equippedObject;
//
//    // scripts
//    [HideInInspector]
//    QuestFinderScript finder;
//    [HideInInspector]
//    QuestBuilderScript builder;
//    [HideInInspector]
//    EquippableFinder equipFind;
//    [HideInInspector]
//    QuestManager manager;
//    [HideInInspector]
//    QuestObject questobjectscript;
//
//    // text
//    PickupQuest pickup; //using this to spawn quest text
//    GameObject textSpawn;
//    TextMesh text;
//
//    // for the questit note
//    GameObject questItNote;
//    Transform visorNode;
//
//    // for text spawning
//    float positionX;
//    float positionY;
//    float positionZ;
//
//    // finishing the quest
//    public bool equipped = false;
//    Transform equippedItem;
//
//    // or perhaps chain quest -- number of items to steal
//    // this, then this, then this, etc
//    // or maybe you have to equip it and use it
//
//	void OnPostRender(){
//
//        finder = GameObject.Find("QuestManager").GetComponent<QuestFinderScript>();
//        builder = GameObject.Find("QuestManager").GetComponent<QuestBuilderScript>();
//        equipFind = Camera.main.GetComponent<EquippableFinder>();
//        manager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
//
//    }
//
//    void FixedUpdate() {
//        equippedItem = equipFind.equippedObject;
//
//		if (parentObject == equippedItem) {
//			
//		}
//
//    }
//
//	parentObject = builder.objeto;
//	objectScript = parentObject.GetComponent<QuestObject> ();
//	requiredPickups = Random.Range (1, 5);
//
//	// store the transform for later text spawning
//	positionX = parentObject.transform.position.x;
//	positionY = parentObject.transform.position.y + 1;
//	positionZ = parentObject.transform.position.z;
//
//	// create title to appear. THIS IS THE QUEST OBJECTIVE.
//	title = ("Pick up" + " " + parentObject.name); 
//
//	// set the ID based on what point in the queue it is
//	// note: there's probably a more efficient way to do this, pls lmk if so
//	id = (QuestManager.questManager.questList.Count);
//
//	// add to the list of available quests on the parent object
//	objectScript.receivableQuestIDs.Add (id);
//	manager.CheckAvailableQuests (objectScript);
//	progress = Quest.QuestProgress.AVAILABLE;
//
//	// give it a description eh
//	// can make this more interesting later during tweaking/juicing stages
//	description = (title + " " + requiredPickups.ToString() + "times and put it down again");
//
//	questTextSpawn ();
//
//	spawnNote ();
//
//	// put it on the parent object
//	CopyComponent (this, parentObject);
//
//	for (int i = 0; i < 20; i++) {
//		//			GameObject noteSpawn = Instantiate (Resources.Load ("NoteSpawner", typeof(GameObject))) as GameObject;
//		//			NoteSpawnerScript spawn = noteSpawn.GetComponent<NoteSpawnerScript> ();
//		NoteSpawnerScript noteSpawn = GameObject.Find("NoteSpawner(Clone)").GetComponent<NoteSpawnerScript>();
//		noteSpawn.MakeItRain ();
//		noteSpawn.AssignID (1);
//	}
//}
//
//// method to copy alla this shit on the pickupquest on the quest object generated
//// in questbuilderscript
//Component CopyComponent (Component original, GameObject destination){
//	System.Type type = original.GetType ();
//	Component copy = destination.AddComponent(type);
//
//	System.Reflection.FieldInfo[] fields = type.GetFields ();
//	foreach (System.Reflection.FieldInfo field in fields) {
//		field.SetValue (copy, field.GetValue(original));
//	}
//	return copy;
//}
//
//public void spawnNote(){
//	// make the questit note
//	visorNode = GameObject.Find("UpperNode").GetComponent<Transform>();
//	//visorNode = GameObject.Find("PlayerVisor").GetComponent<Transform>();
//	questItNote = Instantiate(Resources.Load("QuestItNote", typeof (GameObject))) as GameObject;
//	questItNote.transform.parent = visorNode.transform;
//	//questItNote.transform.position = visorNode.transform.position;
//	questItNote.transform.localPosition = new Vector3(
//		Random.Range(-2.3f, 5.1f),
//		Random.Range(1f, 4.1f),
//		2.5f);
//
//	questItNote.transform.localRotation = Quaternion.Euler(new Vector3(
//		0f,
//		0f,
//		Random.Range(-1f, 1f)));
//
//	// make the actual text appear
//	Canvas questCanvas = questItNote.GetComponentInChildren<Canvas>();
//	//questCanvas.transform.parent = questItNote.gameObject.transform;
//	Text questText = questCanvas.GetComponentInChildren<Text> ();
//	questText.text = description;
//
//	// Stick em to the wall.
//	questItNote.GetComponentInChildren<QuestItNoteFunction>().StickToScreen();
//	questItNote.GetComponentInChildren<QuestItNoteFunction> ().questID = 1;
//}
//
//public void questTextSpawn(){
//	// put the text of the quest right over the object
//	textSpawn = Instantiate (Resources.Load("TextSpawn", typeof(GameObject))) as GameObject;
//	textSpawn.transform.parent = parentObject.transform;
//	textSpawn.transform.position = new Vector3 (positionX, positionY, positionZ);
//	text = textSpawn.GetComponent<TextMesh> ();
//	text.text = (description);
//
//}
//
//public void FinishQuest(){
//
//	// find the quest
//	PickupQuest theCurrentQuest = parentObject.GetComponent<PickupQuest>();
//
//	// mark it done
//	text.text = ("donezo");
//	progress = Quest.QuestProgress.COMPLETE;
//
//	// explode it
//	GameObject stars = Instantiate (Resources.Load ("explosion", typeof(GameObject))) as GameObject; 
//	stars.transform.position = parentObject.transform.position; 
//
//	Destroy (parentObject);
//
//	// find the notes and destroy them
//	NoteSpawnerScript notes = GameObject.Find ("NoteSpawner(Clone)").GetComponent<NoteSpawnerScript> ();
//	for (int i = 0; i < notes.id1.Count; i++) {
//		stars = Instantiate (Resources.Load("explosion", typeof (GameObject))) as GameObject;
//		stars.transform.position = new Vector3 (Random.Range(0, 500),
//			Random.Range(0, 100),
//			Random.Range(0, 500));
//		Destroy (notes.id1[i]);
//	}
//
//	//		for (int i = 10; i < notes.id1.Count; i++) {
//	//			Destroy (notes.id1[i]);
//	//		}
//
//	notes.id1.Clear ();
//	//foreach(GameObject note in notes.id1){
//	//			stars = Instantiate (Resources.Load ("explosion", typeof(GameObject))) as GameObject;
//	//			stars.transform.position = note.transform.position;
//	//			Destroy (note);
//	//		}
//}