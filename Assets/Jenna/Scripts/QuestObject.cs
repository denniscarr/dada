using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour {

	//!!! THIS GOES ON QUEST GIVER OBJECTS, SO PROBABLY EVERYTHING !!!

	private bool inTrigger = false;

	public List<int> availableQuestIDs = new List<int>();
	public List<int> receivableQuestIDs = new List<int>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.name.Contains ("Blue")) {
				inTrigger = true;
			} else {
				inTrigger = false;
			}
		}

		if (inTrigger == true) {
			if (Input.GetMouseButton(0)) {
				//quest user interface manager to check shit
				Debug.Log ("it's seeing you have a quest");
			}
		}

	}

	// these can all be changed, i guess, like the tag and stuff, once we have a uniform
	// nomenclature scheme. again, this is just a frame I'm learning from some guy on the
	// internet so we can change everything when it comes to that point.
	// the ontrigger functions are exclusively for flagging to see if something is interactable.
//	void OnTriggerEnter(Collider col){
//		if (col.tag == "Player") {
//			inTrigger = true;
//		}
//	}
//
//	void OnTriggerExit(Collider col){
//		if (col.tag == "Player") {
//			inTrigger = false;
//		}
//	}
}
