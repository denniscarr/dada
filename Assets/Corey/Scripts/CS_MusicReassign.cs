using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reassigns music clips if it is used by player
/// </summary>

public class CS_MusicReassign : MonoBehaviour {


	//Attach this to a game object if you want it to reassign music clips,
	//via the audio manager

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Test purposes only
		/*
		if (Input.GetKeyDown (KeyCode.G)) {
			UsedByPlayer ();
		}
	*/
	}

	public void MusicUsedByPlayer() {
		Debug.Log ("BeingUsed");
		CS_AudioManager.Instance.ReassignMusic();
	}

}
