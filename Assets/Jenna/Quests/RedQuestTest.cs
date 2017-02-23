using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS QUEST DOES NOT WORK. Trying to understand why.
// -J 2/13

public class RedQuestTest : MonoBehaviour {

	public bool redClicked = false;
	public bool blueClicked = false;
	public bool greenClicked = false;

	public GameObject blue;
	public GameObject green;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void Update () {

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.name.Contains ("Red") && Input.GetMouseButton(0)) {
				redClicked = true;
				QuestManager.questManager.AddQuestItem("click blue", 1);
				Debug.Log ("red clicked");
			}
		}

		if (Physics.Raycast (ray, out hit) && redClicked == true) {
			if (hit.collider.name.Contains ("Blue") && Input.GetMouseButton (0)) {	
				blueClicked = true;
				QuestManager.questManager.AddQuestItem ("click green", 1);
			}
		}

		if (Physics.Raycast (ray, out hit) && blueClicked == true) {
			if (hit.collider.name.Contains ("Green") && Input.GetMouseButtonDown (0)) {
				greenClicked = true;
				QuestManager.questManager.RequestCompletedQuest (1);
				if (QuestManager.questManager.RequestCompletedQuest(1) == true) {
					QuestManager.questManager.CompleteQuest (1);
				}
			}
		}
	}
}
