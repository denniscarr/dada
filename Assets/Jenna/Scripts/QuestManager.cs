using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

	public static QuestManager questManager;

	// MASTER QUEST LIST...hopefully
	public List<Quest> questList = new List<Quest>();

	// KEEPS TRACK OF CURRENT QUEST
	public List<Quest> currentQuestList = new List<Quest>();

	//private variables for QuestObject for later

	void Awake() {
		
		if (questManager == null) {
			questManager = this;
		} else if (questManager != this){
			Destroy (gameObject);
		}
		// ^^^ the above makes sure we don't have multiple managers in the game at once
		// unless that's what we want!!!
		DontDestroyOnLoad(gameObject);

	}


	// ACCEPT QUEST
	// though this will be automatic ;)
	public void AcceptQuest(int questID){
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

}
