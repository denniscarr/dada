using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedQuestTest : MonoBehaviour {

	private bool redClicked = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.name.Contains ("Red") && Input.GetMouseButton(0)) {
				redClicked = true;
			}
		}

		if (redClicked == true) {
			Debug.Log ("red clicked");
			QuestManager.questManager.AddQuestItem ("click blue", 1);
		}
		
	}
}
