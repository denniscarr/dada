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

	// glow
	GameObject fieryGlow;

	// for finishing quest
	public int requiredPickups;
	public int numberofPickups;
	public bool pickedUp;
	public D_starryExpolsion stars;
    public int rewardMoney;
    bool completed;

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
			    !pickedUp)
            {
				numberofPickups++;
				text.text = ("Picked up " + numberofPickups.ToString () + " " + "times");

				if (numberofPickups >= requiredPickups) {
					FinishQuest ();
				}

				pickedUp = true;
			
			} else if (parentObject.GetComponentInChildren<InteractionSettings> ().carryingObject == null) {
				pickedUp = false;
			}
		}

		if (fieryGlow != null) fieryGlow.transform.position = parentObject.transform.position;
    }

	public void makeTheQuest(Quest type){

		parentObject = builder.objeto;
		objectScript = parentObject.GetComponent<QuestObject> ();
		requiredPickups = Random.Range (2, 6);
        rewardMoney = 100 * requiredPickups;
        //Debug.Log("Required pickups: " + requiredPickups + ", Reward money: " + rewardMoney);

        // add the glow
        fieryGlow = Instantiate(Resources.Load ("questobject-fire", typeof (GameObject))) as GameObject;
		fieryGlow.transform.parent = parentObject.transform;

		// store the transform for later text spawning
		positionX = parentObject.transform.position.x;
		positionY = parentObject.transform.position.y + 1;
		positionZ = parentObject.transform.position.z;

		// create title to appear. THIS IS THE QUEST OBJECTIVE.
		title = ("Drag the" + " " + parentObject.name + " " + "with the text on it"); 

		// set the ID based on what point in the queue it is
		// note: there's probably a more efficient way to do this, pls lmk if so
		id = (QuestManager.questManager.questList.Count);

		// add to the list of available quests on the parent object
		if (objectScript != null) objectScript.receivableQuestIDs.Add (id);
		manager.CheckAvailableQuests (objectScript);
		progress = Quest.QuestProgress.AVAILABLE;

		// give it a description eh
		// can make this more interesting later during tweaking/juicing stages
		description = (title + " " + requiredPickups.ToString() + " " + "times and put it down again");

		questTextSpawn ();

		spawnNote ();

        // put it on the parent object
        CopyComponent(this, parentObject);

		for (int i = 0; i < 20; i++) {
			NoteSpawnerScript noteSpawn = GameObject.Find("NoteSpawner(Clone)").GetComponent<NoteSpawnerScript>();
			noteSpawn.MakeItRain ();
			noteSpawn.AssignID (1);
		}
	}

	// method to copy alla this shit on the pickupquest on the quest object generated
	// in questbuilderscript
	Component CopyComponent (Component original, GameObject destination)
    {
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
        visorNode = GameObject.Find("INROOMOBJECTS").GetComponent<Transform>();
        questItNote = Instantiate(Resources.Load("QuestItNote", typeof (GameObject))) as GameObject;
        questItNote.transform.parent = visorNode.transform;
        questItNote.transform.localPosition = new Vector3(
            Random.Range(-2.3f, 5.1f),
			Random.Range(1f, 4.1f),
			2.5f);

        questItNote.transform.localRotation = Quaternion.Euler(new Vector3(
            0f,
            0f,
            Random.Range(-1f, 1f)));

        // make the actual text appear
        Canvas questCanvas = questItNote.GetComponentInChildren<Canvas>();
		Text questText = questCanvas.GetComponentInChildren<Text> ();
		questText.text = description;

        // Stick em to the wall.
        questItNote.GetComponentInChildren<QuestItNoteFunction>().StickToScreen();
		questItNote.GetComponentInChildren<QuestItNoteFunction> ().questID = 1;
    }

    public void questTextSpawn(){
		
		// put the text of the quest right over the object
		textSpawn = Instantiate (Resources.Load("TextSpawn", typeof(GameObject))) as GameObject;
		textSpawn.transform.parent = parentObject.transform;
		textSpawn.transform.position = new Vector3 (positionX, positionY, positionZ);
		text = textSpawn.GetComponent<TextMesh> ();
		text.text = ("pick me up" + " " + requiredPickups.ToString());
		
	}

	public void FinishQuest(){

        if (completed) return;

		// find the quest
		PickupQuest theCurrentQuest = parentObject.GetComponent<PickupQuest>();

		// mark it done
		text.text = ("donezo");
		progress = Quest.QuestProgress.COMPLETE;

		// explode it
		GameObject stars = Instantiate (Resources.Load ("explosion-noforce", typeof(GameObject))) as GameObject; 
		stars.transform.position = parentObject.transform.position;

		// find the notes and destroy them
		NoteSpawnerScript notes = GameObject.Find ("NoteSpawner(Clone)").GetComponent<NoteSpawnerScript> ();
		for (int i = 0; i < notes.id1.Count; i++) {
			Destroy (notes.id1[i]);
		}

		notes.id1.Clear ();

        // Give player money
        Debug.Log("Giving reward: " + rewardMoney);
        GameObject.Find("Bootstrapper").GetComponent<PlayerMoneyManager>().funds += rewardMoney;

		if (manager != null) manager.currentCompletedQuests++;

        //Destroy (parentObject);
        Destroy(parentObject.GetComponent<PickupQuest>());
        Destroy(parentObject.GetComponent<QuestObject>());
        Destroy(fieryGlow);

        completed = true;
    }
}
