using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// making the Quest class for the Quest system
// commenting thoroughly for learning purposes
// seems similar to CSS
// will be useful later, hopefully -J

[System.Serializable]
public class Quest: MonoBehaviour {

	// an enum is a number of integers that we can give a name, apparently
	// the curly brackets are possible states that a quest "progress" can be in.
	// this can be changed/added onto later; right now this is for testing purposes.
	// in fact, it looks like making this its own class in the game is giving it a lot of
	// flexibility for us!
	public enum QuestProgress {NOT_AVAILABLE, AVAILABLE, ACCEPTED, COMPLETE, DONE}

	// finding object used for quest
	public GameObject targetObject;

	// scripts
	[HideInInspector]
	public QuestFinderScript qfs;
	[HideInInspector]
	public QuestBuilderScript builder;
	[HideInInspector]
	public QuestManager manager;

	// self-explanatory
	public string title;

	// id of the quest, which gives us apparently all the info for the quest
	public int id;

	// tracks the progress state and compare it with any other state, as well as
	// set states. This gives us access to the enum above!
	public QuestProgress progress;

	// quest description -- I gather that this will store text that will be accessible
	// to the player, what appears on screen and such the like
	public string description;

	// it'll be fun and probably not too hard to figure out how to randomize this
	// i love a challenge!
	// particularly useful for chained quests apparently
	public int nextQuest;

	// apparently this can also be an int, depending on how you want to classify your
	// quests. We might want to change this to an int later. Can also be used to remove
	// items if you have an inventory system? Which we might be doing?
	public string questObjective; 

	// in case it's a countable quest...instead of a countably infinite one lolol
	public int questObjectiveCount;

	// required amount of quest objectives/objects from the above few
	public int questObjectiveRequirement;

	// lol this definitely won't be relevant at all, but we could maybe replace the idea
	// of an expReward with, instead, a tween/lerp/perlin about BLEED WHOOOOOOOOOOOOOOA
	// i'm a genius
	// maybe we should call this "bleedReward"
	public int expReward;

	// this one we can maybe keep since we do have an inventory system eventually i think
	public int itemReward;

	public D_starryExpolsion stars;
	public int rewardMoney;
	protected bool completed;
	protected List<GameObject> myNotes; //The list is here in case if we want to bring back more than 1 note per quest

	// for the questit note
	public GameObject questItNote;
	protected Transform visorNode;

	// glow
	protected GameObject fieryGlow;

	//sound
	protected GameObject radarSound;
	public QuestObject objectScript;

	public virtual void CheckStatus()
	{
	}

	public virtual void makeTheQuest()
	{
		targetObject = builder.objeto;

		objectScript = targetObject.GetComponent<QuestObject> ();

		// set the ID based on what point in the queue it is
		// note: there's probably a more efficient way to do this, pls lmk if so
		id = (QuestManager.questManager.questList.Count);


		// add the glow
		fieryGlow = Instantiate(Resources.Load ("questobject-fire", typeof (GameObject))) as GameObject;
		fieryGlow.transform.parent = targetObject.transform;
		fieryGlow.transform.position = targetObject.transform.position;

		// add the sound
		radarSound = Instantiate(Resources.Load ("QuestItemChildren/QuestItemSound", typeof (GameObject))) as GameObject;
		radarSound.transform.parent = targetObject.transform;
		radarSound.transform.position = targetObject.transform.position;

		// set the ID based on what point in the queue it is
		// note: there's probably a more efficient way to do this, pls lmk if so
		id = (QuestManager.questManager.questList.Count);


	}


	public virtual void Start(){
		// find referenced materials
		builder = Services.Quests.GetComponent<QuestBuilderScript> ();
		qfs = Services.Quests.GetComponent<QuestFinderScript> ();
		manager = Services.Quests.GetComponent<QuestManager> ();
		myNotes = new List<GameObject>();
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

		targetObject.GetComponentInChildren<InteractionSettings>().associatedNotes.Add(questItNote);
		//        Debug.Log("Notes: " + myNotes.Count);
	}

	public virtual void FinishQuest(){
		if (completed) return;
		// explode it
		GameObject stars = Instantiate (Resources.Load ("explosion-noforce", typeof(GameObject))) as GameObject; 
		stars.transform.position = targetObject.transform.position;

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

		// mark it done
		//text.text = ("donezo");
		progress = Quest.QuestProgress.COMPLETE;


		//Destroy everything and make sure they stay dead goddamnit.;
		Destroy(fieryGlow);
		Destroy(radarSound);
		for (int i = 0; i < targetObject.transform.childCount; i++)
		{
			if (targetObject.transform.GetChild(i).name.Contains("questobject-fire"))
			{
				Destroy(targetObject.transform.GetChild(i).gameObject);
			}

			else if (targetObject.transform.GetChild(i).name.Contains("QuestItemSound"))
			{
				Destroy(targetObject.transform.GetChild(i).gameObject);
			}
		}
		foreach (PickupQuest picko in targetObject.GetComponents<PickupQuest>())
		{
			Destroy(picko);
		}
		foreach(QuestObject questo in targetObject.GetComponents<QuestObject>())
		{
			Destroy(questo);
		}

		targetObject.GetComponentInChildren<InteractionSettings>().DestroyAssociatedNotes();

		if (targetObject.GetComponentInChildren<IncoherenceController>() != null) targetObject.GetComponentInChildren<IncoherenceController>().incoherenceMagnitude += Services.IncoherenceManager.questIncrease;

		completed = true;


	}
}
