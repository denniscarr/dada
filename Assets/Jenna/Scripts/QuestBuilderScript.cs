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

	//Store all game objects with NPC.cs attached in this list
	private NPC[] enemies;
	private InteractionSettings[] usableItems;


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

		//if (Input.GetKeyDown(KeyCode.Tab)){
			//if (finder.pickups.Count > 0) {
            if (manager.questsGeneratedInCurrentLevel < Services.Quests.questsToComplete)
            {
                GeneratePickup();
				GenerateElimination ();
				GenerateUse ();
//                Debug.Log("Quests genereated in current level: " + manager.questsGeneratedInCurrentLevel);
//                Debug.Log("Quests to complete: " + manager.questsToComplete);
			}
                //else if (manager.questList.Count >= (Mathf.Abs (levelman.levelNum + 1))) {
					//for (int i = 0; i < Random.Range (10, 30); i++) {
					//	NoteSpawnerScript rain = spawner.GetComponent<NoteSpawnerScript> ();
					//	rain.MakeItRain ();
					//}
				//}
			//}
		//}
	}

	// generates pickup quest
	public void GeneratePickup() {

        if (Services.LevelGen.levelNum == 0) return;
        //if (finder.pickups.Count <= 0) return;

		// pick an object for it
		//questThing = ranger % length;
		objeto = finder.pickups [Random.Range (0, finder.pickups.Count)];
		//print (objeto);
        if (objeto == null) return;
		objectScript = objeto.GetComponent<QuestObject> ();

        if (objeto.GetComponent<PickupQuest>() != null) return;
        else if (objeto.GetComponentInChildren<InteractionSettings>().IsInVisor) return;

		// add the script if it's part of finder.pickups
		PickupQuest newQuest = objeto.AddComponent<PickupQuest> ();
        if (objeto.GetComponents<PickupQuest>().Length > 1)
        {
            Destroy(objeto.GetComponents<PickupQuest>()[1]);
            return;
        }
		pickup.makeTheQuest (objeto);
		manager.questList.Add (newQuest);

		if (newQuest.progress == Quest.QuestProgress.AVAILABLE) {
			manager.QuestRequest(objectScript);
			//Debug.Log ("quest added to list");
			pickup.spawnNote ();
		}

		if (newQuest.progress == Quest.QuestProgress.ACCEPTED) {
			manager.currentQuestList.Add (newQuest);
		}

        manager.questsGeneratedInCurrentLevel++;
    }

	public void GenerateElimination(){
		if (Services.LevelGen.levelNum == 0) return;
		enemies = GameObject.FindObjectsOfType<NPC> ();
		GameObject enemy = enemies[Random.Range (0, enemies.Length)].transform.parent.gameObject;
		//print (enemy);
		if (enemy.GetComponent<NPCEliminationQuest> () != null)
			return;
		NPCEliminationQuest newQuest = enemy.AddComponent<NPCEliminationQuest> ();
		if (enemy.GetComponents<NPCEliminationQuest> ().Length > 1) {
			Destroy (enemy.GetComponents<NPCEliminationQuest> () [1]);
			return;
		}
		this.gameObject.GetComponent<NPCEliminationQuest> ().makeTheQuest (enemy);
		manager.questList.Add (newQuest);

		if (newQuest.progress == Quest.QuestProgress.AVAILABLE) {
			manager.QuestRequest(objectScript);
			//Debug.Log ("quest added to list");
			this.gameObject.GetComponent<NPCEliminationQuest> ().spawnNote ();
		}

		if (newQuest.progress == Quest.QuestProgress.ACCEPTED) {
			manager.currentQuestList.Add (newQuest);
		}
		manager.questsGeneratedInCurrentLevel++;
	}

	public void GenerateUse() {
		if (Services.LevelGen.levelNum == 0) return;
		//usableItems = GameObject.FindObjectsOfType<InteractionSettings> ();
		GameObject usableItem = finder.pickups [Random.Range (0, finder.pickups.Count)];
		//print (usableItem);

		if (usableItem.GetComponent<useItemQuest> () != null)
			return;
		useItemQuest newQuest = usableItem.AddComponent<useItemQuest> ();
		if (usableItem.GetComponents<useItemQuest> ().Length > 1) {
			Destroy (usableItem.GetComponents<useItemQuest> () [1]);
			return;
		}
		this.gameObject.GetComponent<useItemQuest> ().makeTheQuest (usableItem);
		manager.questList.Add (newQuest);

		if (newQuest.progress == Quest.QuestProgress.AVAILABLE) {
			manager.QuestRequest(objectScript);
			//Debug.Log ("quest added to list");
			this.gameObject.GetComponent<useItemQuest> ().spawnNote ();
		}

		if (newQuest.progress == Quest.QuestProgress.ACCEPTED) {
			manager.currentQuestList.Add (newQuest);
		}
		manager.questsGeneratedInCurrentLevel++;
	}
}
