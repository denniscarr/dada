using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestItNote : MonoBehaviour {

	
	public Text questText;

	[SerializeField] bool beingCarried;
	[SerializeField] InteractionSettings interactionSettings;

	// Use this for initialization
	void Start () {

		interactionSettings = GetComponentInChildren<InteractionSettings> ();
		questText = transform.Find ("Canvas").GetComponentInChildren<Text> ();

	}
	
	// Update is called once per frame
	void Update () {

		if (interactionSettings.carryingObject != null && !beingCarried) {
			//carried by player

			beingCarried = true;

			Quaternion newRotation = Quaternion.Euler (new Vector3 (0.0f, transform.parent.rotation.y, transform.parent.rotation.z));

			transform.localRotation = newRotation;


		} else if (beingCarried) {

			if (interactionSettings.carryingObject == null) {
				//actor dropped note
				//stick to wall
				beingCarried = false;
			}

		} 




	}

	void StickToWall() {

	}
}
