using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetPlayerTitleScreen : MonoBehaviour {

	void Start () {
		Services.Player = gameObject;
	}

	void Update(){
		if (Input.anyKeyDown) {
			SceneManager.LoadScene ("ProofOfConcept");
		}
	}
}
