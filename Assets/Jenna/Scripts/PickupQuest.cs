using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupQuest : Quest {

	// finding object
	public GameObject objectToPickUp;
	public int timesPressed;

	// scripts
	QuestFinderScript qfs;
	QuestBuilderScript builder;
	QuestManager manager;

	// for testing only
	Ray ray;
	RaycastHit hit;

	void Start () {

		// find referenced materials
		builder = gameObject.GetComponent<QuestBuilderScript> ();
		qfs = gameObject.GetComponent<QuestFinderScript> ();
		manager = gameObject.GetComponent<QuestManager> ();
		timesPressed = 0;

		// build the actual quest
		// maybe this has to be a trigger or an event
//		for (int i = 0; i < QuestManager.questManager.questList.Count; i++){
//			title = (builder.currentVerb + " " + builder.currentNoun);
//			manager.questList[i].progress = Quest.QuestProgress.AVAILABLE;
//
//		}
	}
	
	public override void CheckStatus() {
		// check if done on active quests

	}
}
