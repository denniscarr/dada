using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

	public static QuestManager questManager;

	// MASTER QUEST LIST...hopefully
	public List<Quest> questList = new List<Quest>();

	// KEEPS TRACK OF CURRENT QUEST
	public List<Quest> currentQuestList = new List<Quest>();

	// FINDS THE GRAIL QUEST
	public List<Quest> unattainableQuestItems = new List<Quest>();

//	// I AM ONLY MAKING THIS PUBLIC BECAUSE I MAY CHANGE THE NAME WRT RANDOM LATER
	[HideInInspector]
	public int tempID;

	// number of quests to complete
	public int questsToComplete;
	public int currentCompletedQuests;

	// quests completed
	public bool allQuestsCompleted;

	//private variables for QuestObject for later

	void Awake() {
		
		if (questManager == null) {
			questManager = this;
		} else if (questManager != this) {
			Destroy (gameObject);
		}
		// ^^^ the above makes sure we don't have multiple managers in the game at once
		// unless that's what we want!!!
		DontDestroyOnLoad(gameObject);

	}

	void Update() {
		if (currentQuestList.Count > 0) {
			foreach (Quest q in currentQuestList) {
				q.CheckStatus ();
			}
		}
	}
		
	public void QuestRequest(QuestObject NPCQuestObject) {

		// testing available IDs for quest objects and seeing if they match and what the progress is
		// if it does. Not...really sure this will be relevant, but putting it for example.
		// CHECK FOR AVAILABLE QUESTS
		if (NPCQuestObject.availableQuestIDs.Count > 0){
			for (int i = 0; i < questList.Count; i++){ //for every available quest list with i items, loop through list i times
				for (int j = 0; j < NPCQuestObject.availableQuestIDs.Count; j++){ //for every item j in list i, loop j times to check for a match. math is hard.
					if (questList[i].id == NPCQuestObject.availableQuestIDs[j]
						&& questList[i].progress == Quest.QuestProgress.AVAILABLE){
							Debug.Log ("Quest ID: " + NPCQuestObject.availableQuestIDs[j] + " " + questList[i].progress);
							AcceptQuest (NPCQuestObject.availableQuestIDs[j]);
						// NOTE TO SELF: PARSE THIS BETTER FOR SELF UNDERSTANDING LATER
					}
				}
			}
		}

		// CHECK FOR ACTIVE QUESTS
		for (int i = 0; i < currentQuestList.Count; i++) {
			for (int j = 0; j < NPCQuestObject.receivableQuestIDs.Count; j++) {
				if (currentQuestList [i].id == NPCQuestObject.receivableQuestIDs [j]
					&& currentQuestList [i].progress == Quest.QuestProgress.ACCEPTED
					|| currentQuestList [i].progress == Quest.QuestProgress.COMPLETE) {
					//!!!!!!! quest UI manager goes here later !!!!!!!
					Debug.Log("Quest ID: " + NPCQuestObject.receivableQuestIDs[j] + " " + currentQuestList[i].progress);
					CompleteQuest (NPCQuestObject.receivableQuestIDs[j]); // checks to see if quest is already completed and, if complete, will run the function to mark the quest as donezo
				}
			}
		}
	}


	// ACCEPT QUEST
	// though this will be automatic ;)
	public void AcceptQuest(int questID) {
		
		for (int i = 0; i < questList.Count; i++){
			if (questList [i].id == questID && questList [i].progress == Quest.QuestProgress.AVAILABLE) {
				currentQuestList.Add (questList [i]); //add to list
				questList [i].progress = Quest.QuestProgress.ACCEPTED; //set to accepted
			}
		}
	}

	// give up quest option? i don't know if we should let them, but i'm putting it below
//	public void GiveUpQuest(int questID){
//		for (int i = 0; i < currentQuestList.Count; i++) {
//			if (currentQuestList [i].id == questID && currentQuestList [i].progress == Quest.QuestProgress.ACCEPTED) {
//				currentQuestList [i].progress = Quest.QuestProgress.AVAILABLE; //makes it available again
//				currentQuestList [i].questObjective = 0; //deletes progress
//				currentQuestList.Remove(currentQuestList[i]); //removes it all yo
//			}
//		}
//	}

	// COMPLETE QUEST
	public void CompleteQuest(int questID){
		
		for (int i = 0; i < currentQuestList.Count; i++) {
			if (currentQuestList [i].id == questID && currentQuestList [i].progress == Quest.QuestProgress.COMPLETE) {
				currentQuestList [i].progress = Quest.QuestProgress.DONE; //set it to totally done
				currentQuestList.Remove (currentQuestList[i]); //remove it from the list

				//enter the reward here eventually. !!! THE REWARD IS A NEW RANDOM QUEST LOL !!!
			}		
		}

		//could also put the randomness here -- use the chain quest function below
		CheckChainQuest(questID);
	}

	// CHECK CHAIN QUEST
	public void CheckChainQuest (int questID){

		int tempID = 0;
		for (int i = 0; i < questList.Count; i++) {
			if (questList [i].id == questID && questList [i].nextQuest > 0) { // this checks to see if there's a chain quest available at all. if there's a chain quest, set it > 0 IN INSPECTOR
				tempID = questList[i].nextQuest;
			}
		}

		// check for chain quests to unlock!
		// i suspect everything will have to be randomized chain quest ID#s
		if (tempID > 0) {
			for (int i = 0; i < questList.Count; i++) {
				if (questList [i].id == tempID && questList [i].progress == Quest.QuestProgress.NOT_AVAILABLE) { // if it's not been unlocked yet, unlock it
					questList[i].progress = Quest.QuestProgress.AVAILABLE;
				}
			}
		}
	}

	// ADD ITEMS
	// here it's a string because the tutorial I'm following kept objectives as item name
	// strings, but this is manipulable as long as it's changed in the Quest Class script.
	public void AddQuestItem(string questObjective, int itemAmount){
		
		for (int i = 0; i < currentQuestList.Count; i++) {
			if (currentQuestList [i].questObjective == questObjective && currentQuestList [i].progress == Quest.QuestProgress.ACCEPTED) {
				currentQuestList [i].questObjectiveCount += itemAmount;
			}

			// if you've accepted the quest and have the correct number of items (again,
			// we can modify this later if need be, or I can rewrite the code), then set
			// the quest progress to complete.
			if (currentQuestList [i].questObjectiveCount >= currentQuestList [i].questObjectiveRequirement
			    && currentQuestList [i].progress == Quest.QuestProgress.ACCEPTED) {
				currentQuestList [i].progress = Quest.QuestProgress.COMPLETE;
			}
		}
	}

	// remove items
	// i think this is probably necessary, again, not for "items" per se, so much as we can
	// set up a sort of inventory-like system to store the random quests themselves.


	// BOOLS FOR CHECKING QUEST INFO, to keep track...because this is the manager
	public bool RequestAvailableQuest(int questID){
		
		for (int i = 0; i < questList.Count; i++) {
			if (questList [i].id == questID && questList [i].progress == Quest.QuestProgress.AVAILABLE) {
				return true;
			}
		}
		return false;
	}

	public bool RequestAcceptedQuest(int questID){
		
		for (int i = 0; i < questList.Count; i++) {
			if (questList [i].id == questID && questList [i].progress == Quest.QuestProgress.ACCEPTED) {
				return true;
			}
		}
		return false;
	}

	public bool RequestCompletedQuest(int questID){
		
		for (int i = 0; i < questList.Count; i++) {
			if (questList [i].id == questID && questList [i].progress == Quest.QuestProgress.COMPLETE) {
				return true;
			}
		}
		return false;
	}

	// BOOLS FOR PASSING COMPLETE QUEST OBJECTS...which can also be done in the UI manager
	// !!!!!!! NOTE TO SELF: COMMENT FOR UNDERSTANDING AT A LATER DATE !!!!!!!
	public bool CheckAvailableQuests(QuestObject NPCQuestObject){

		for (int i = 0; i < questList.Count; i++) {
			for (int j = 0; j < NPCQuestObject.availableQuestIDs.Count; j++) {
				if (questList[i].id == NPCQuestObject.availableQuestIDs[j]&& questList[i].progress == Quest.QuestProgress.AVAILABLE){
					return true;
				}
			}
		}
		return false;
	}

	public bool CheckAcceptedQuests(QuestObject NPCQuestObject){
		for (int i = 0; i < questList.Count; i++) {
			for (int j = 0; j < NPCQuestObject.availableQuestIDs.Count; j++) {
				if (questList [i].id == NPCQuestObject.availableQuestIDs [j] && questList [i].progress == Quest.QuestProgress.ACCEPTED) {
					return true;
				}
			}
		}
		return false;
	}

	public bool CheckCompletedQuests(QuestObject NPCQuestObject){
		for (int i = 0; i < questList.Count; i++) {
			for (int j = 0; j < NPCQuestObject.availableQuestIDs.Count; j++) {
				if (questList [i].id == NPCQuestObject.availableQuestIDs [j] && questList [i].progress == Quest.QuestProgress.COMPLETE) {
					return true;
				}
			}
		}
		return false;
	}

}
