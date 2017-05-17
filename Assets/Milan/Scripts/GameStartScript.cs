using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartScript : MonoBehaviour {

	public GameObject fakeGrail;
	MyFirstPersonController FirstPersonController;
	bool ButtonPressed = false;

	void Awake(){
		FirstPersonController = GetComponentInChildren<MyFirstPersonController> ();
		FirstPersonController.enabled = false;
	}

	void Update () {
		if (Input.anyKey && !ButtonPressed) {
			StartCoroutine (EnableController ());
			ButtonPressed = true;
		}
	}

	IEnumerator EnableController(){
		float t = 0;
		FirstPersonController.enabled = true;

		while (t < 4) {
			t += Time.deltaTime;
			fakeGrail.GetComponentInChildren<AudioSource>().volume = (4f - t) / 4f;
			yield return null;
		}

		Destroy (fakeGrail);
		Destroy (this);
	}
}
