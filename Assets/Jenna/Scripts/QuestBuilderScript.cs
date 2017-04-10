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

	}

	void Update () {

		ranger = Random.Range (0, finder.questItems.Count);
		length = finder.questItems.Count;

		// set specific parameters depending on level # here soon
		if (Input.GetKeyDown(KeyCode.Tab)) {
			Generate ();
		}
	}

	public void Generate() {
		// pick an object for it
		questThing = ranger % length;
		objeto = finder.pickups [Random.Range (0, finder.pickups.Count) % finder.pickups.Count];
		objectScript = objeto.GetComponent<QuestObject> ();

		// add the script if it's part of finder.pickups
		PickupQuest newQuest = objeto.AddComponent<PickupQuest> ();
		pickup.makeTheQuest (newQuest);
		manager.questList.Add (newQuest);
	}
}
