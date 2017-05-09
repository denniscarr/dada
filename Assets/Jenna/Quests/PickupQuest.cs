using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

	//sound
	GameObject radarSound;

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

    List<GameObject> myNotes;

	void Start () {

        myNotes = new List<GameObject>();

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
			//if (parentObject.GetComponentInChildren<InteractionSettings> ().carryingObject != null &&
			//    parentObject.GetComponentInChildren<InteractionSettings> ().carryingObject.name == "Player" &&
			//    !pickedUp)
   //         {
			//	numberofPickups++;
			//	text.text = ("Picked up " + numberofPickups.ToString () + " " + "times");

			//	if (numberofPickups >= requiredPickups) {
			//		FinishQuest ();
			//	}

			//	pickedUp = true;
			
			//} else if (parentObject.GetComponentInChildren<InteractionSettings> ().carryingObject == null) {
			//	pickedUp = false;
			//}

            if (parentObject.GetComponentInChildren<InteractionSettings>().IsInVisor)
            {
                if (parentObject.GetComponent<CollisionReporter>() == null) parentObject.AddComponent<CollisionReporter>();

                if (parentObject.GetComponent<CollisionReporter>() != null && parentObject.GetComponentInChildren<InteractionSettings>().carryingObject != Services.Player && parentObject.GetComponent<CollisionReporter>().collidedWithSomethingAtLeastOnce)
                {
                    Destroy(parentObject.GetComponent<CollisionReporter>());
                    FinishQuest();
                }
            }
		}
    }

	public void makeTheQuest(Quest type){

		parentObject = builder.objeto;
		objectScript = parentObject.GetComponent<QuestObject> ();
		requiredPickups = Random.Range (2, 6);
        rewardMoney = Mathf.RoundToInt(parentObject.GetComponentInChildren<InteractionSettings>().price * Random.Range(1.1f, 2f));
        //Debug.Log("Required pickups: " + requiredPickups + ", Reward money: " + rewardMoney);

        // add the glow
        fieryGlow = Instantiate(Resources.Load ("questobject-fire", typeof (GameObject))) as GameObject;
		fieryGlow.transform.parent = parentObject.transform;
        fieryGlow.transform.position = parentObject.transform.position;

        // add the sound
        radarSound = Instantiate(Resources.Load ("QuestItemChildren/QuestItemSound", typeof (GameObject))) as GameObject;
		radarSound.transform.parent = parentObject.transform;
        radarSound.transform.position = parentObject.transform.position;

        // store the transform for later text spawning
        positionX = parentObject.transform.position.x;
		positionY = parentObject.transform.position.y + 1;
		positionZ = parentObject.transform.position.z;

		// create title to appear. THIS IS THE QUEST OBJECTIVE.
		title = ("Pick up the glowing" + " " + parentObject.name + " "); 

		// set the ID based on what point in the queue it is
		// note: there's probably a more efficient way to do this, pls lmk if so
		id = (QuestManager.questManager.questList.Count);

		// add to the list of available quests on the parent object
		if (objectScript != null) objectScript.receivableQuestIDs.Add (id);
		//manager.CheckAvailableQuests (objectScript);
		progress = Quest.QuestProgress.AVAILABLE;

		// give it a description eh
		// can make this more interesting later during tweaking/juicing stages
		description = (title + " " + "and place it in your room. Reward: $" + rewardMoney);

		questTextSpawn ();

        // put it on the parent object
        CopyComponent(this, parentObject);

        spawnNote();

        //for (int i = 0; i < 20; i++) {
        //	NoteSpawnerScript noteSpawn = GameObject.Find("NoteSpawner(Clone)").GetComponent<NoteSpawnerScript>();
        //	noteSpawn.MakeItRain (id);
        //}
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

	public void spawnNote()
    {	
        // make the questit note
        questItNote = Instantiate(Resources.Load("QuestItNote", typeof (GameObject))) as GameObject;

        // make the actual text appear
        Canvas questCanvas = questItNote.GetComponentInChildren<Canvas>();
		Text questText = questCanvas.GetComponentInChildren<Text> ();
		questText.text = description;

        questItNote.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
        questItNote.transform.DOScale(Vector3.one, 0.4f);
        questText.DOText(description, 1f);

        // Stick em to the wall.
        questItNote.GetComponentInChildren<QuestItNoteFunction>().StickToScreen();
		questItNote.GetComponentInChildren<QuestItNoteFunction> ().questID = 1;

        parentObject.GetComponentInChildren<InteractionSettings>().associatedNotes.Add(questItNote);
        Debug.Log("Notes: " + myNotes.Count);
    }

    public void questTextSpawn(){
		
		// put the text of the quest right over the object
		//textSpawn = Instantiate (Resources.Load("TextSpawn", typeof(GameObject))) as GameObject;
		//textSpawn.transform.parent = parentObject.transform;
		//textSpawn.transform.position = new Vector3 (positionX, positionY, positionZ);
		//text = textSpawn.GetComponent<TextMesh> ();
		//text.text = ("pick me up" + " " + requiredPickups.ToString());
		
	}

	public void FinishQuest(){

        if (completed) return;

		// find the quest
		PickupQuest theCurrentQuest = parentObject.GetComponent<PickupQuest>();

		// mark it done
		//text.text = ("donezo");
		progress = Quest.QuestProgress.COMPLETE;

		// explode it
		GameObject stars = Instantiate (Resources.Load ("explosion-noforce", typeof(GameObject))) as GameObject; 
		stars.transform.position = parentObject.transform.position;

        // find the notes and destroy them (?)
        //NoteSpawnerScript notes = GameObject.Find ("NoteSpawner(Clone)").GetComponent<NoteSpawnerScript> ();
        //for (int i = 0; i < notes.id1.Count; i++) {
        //	Destroy (notes.id1[i]);
        //}
        //notes.id1.Clear ();

        // Give player money
        GameObject.Find("Bootstrapper").GetComponent<PlayerMoneyManager>().funds += rewardMoney;

        if (Services.Quests != null)
        {
            Services.Quests.currentCompletedQuests++;
            Debug.Log("Quests completed " + Services.Quests.currentCompletedQuests + ". Quests to complete: " + Services.Quests.questsToComplete);
            if (Services.Quests.currentCompletedQuests >= Services.Quests.questsToComplete)
            {
                //Debug.Log("all quests complete!");
                //Debug.Break();
                Services.Quests.allQuestsCompleted = true;
            }
        }

        //Destroy everything and make sure they stay dead goddamnit.;
        Destroy(fieryGlow);
        Destroy(radarSound);
        for (int i = 0; i < parentObject.transform.childCount; i++)
        {
            if (parentObject.transform.GetChild(i).name.Contains("questobject-fire"))
            {
                Destroy(parentObject.transform.GetChild(i).gameObject);
            }

            else if (parentObject.transform.GetChild(i).name.Contains("QuestItemSound"))
            {
                Destroy(parentObject.transform.GetChild(i).gameObject);
            }
        }
        foreach (PickupQuest picko in parentObject.GetComponents<PickupQuest>())
        {
            Destroy(picko);
        }
        foreach(QuestObject questo in parentObject.GetComponents<QuestObject>())
        {
            Destroy(questo);
        }

        parentObject.GetComponentInChildren<InteractionSettings>().DestroyAssociatedNotes();

        if (parentObject.GetComponentInChildren<InteractionSettings>() != null) parentObject.GetComponentInChildren<IncoherenceController>().incoherenceMagnitude += Services.IncoherenceManager.questIncrease;

        completed = true;
    }


    //public void DestroyNotes()
    //{
    //    Debug.Log("notes belonging to me: " + myNotes.Count);
    //    // Destroy all notes related to this quest.
    //    for (int i = 0; i < myNotes.Count; i++)
    //    {
    //        Destroy(myNotes[i]);
    //    }
    //}
}
