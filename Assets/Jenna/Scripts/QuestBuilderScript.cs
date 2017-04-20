using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// LOCATION: QUEST MANAGER

public class QuestBuilderScript : MonoBehaviour {

	// quest finder
	public QuestFinderScript finder;

	// quest manager
	public QuestManager manager;

	// pickup quest
	public PickupQuest pickup;

	// info for equip quest will go here
	// public EquipQuest equip;
	GameObject spawner;

	// bootstrapper
	LevelManager levelman;

	// quest object script
	public QuestObject objectScript;

	// stuff for picking objects/generation
	public int questThing, ranger, length;
	public GameObject objeto;


	void Start () {

		// get scripts
		finder = this.gameObject.GetComponent<QuestFinderScript> ();
		manager = this.gameObject.GetComponent<QuestManager> ();
		pickup = this.gameObject.GetComponent<PickupQuest> ();
		levelman = GameObject.Find ("Bootstrapper").GetComponent<LevelManager> ();

		// instantiate notespawner
		spawner = Instantiate(Resources.Load("NoteSpawner", typeof (GameObject))) as GameObject;

	}

	void Update () {

		ranger = Random.Range (0, finder.questItems.Count);
		length = finder.questItems.Count;

		// CHANGE THIS TO BE A RANDOM ROLL
		// AND CERTAIN NUMBERS TURN OUT TO BE GENERATING CERTAIN QUEST TYPES

		manager.questsToComplete = levelman.levelNum + 2;

		if (manager.currentCompletedQuests == manager.questsToComplete) {
			manager.allQuestsCompleted = true;
		} else {
			manager.allQuestsCompleted = false;
		}

		if (Input.GetKeyDown(KeyCode.Tab)){
			if (finder.pickups.Count > 0) {
				if (manager.questList.Count <= (Mathf.Abs (levelman.levelNum + 1))) {
					if (manager.questList.Count >= 0) {
						GeneratePickup ();
					}
				} else if (manager.questList.Count >= (Mathf.Abs (levelman.levelNum + 1))) {
					for (int i = 0; i < Random.Range (10, 30); i++) {
						NoteSpawnerScript rain = spawner.GetComponent<NoteSpawnerScript> ();
						rain.MakeItRain ();
					}
				}
			}
		}
	}

	// generates pickup quest
	public void GeneratePickup() {
		// pick an object for it
		questThing = ranger % length;
		objeto = finder.pickups [Random.Range (0, finder.pickups.Count) % finder.pickups.Count];
		objectScript = objeto.GetComponent<QuestObject> ();

		// add the script if it's part of finder.pickups
		PickupQuest newQuest = objeto.AddComponent<PickupQuest> ();
		pickup.makeTheQuest (newQuest);
		manager.questList.Add (newQuest);

		if (newQuest.progress == Quest.QuestProgress.AVAILABLE) {
			manager.QuestRequest(objectScript);
			Debug.Log ("quest added to list");
			pickup.spawnNote ();
		}

		if (newQuest.progress == Quest.QuestProgress.ACCEPTED) {
			manager.currentQuestList.Add (newQuest);
		}
	}
}
