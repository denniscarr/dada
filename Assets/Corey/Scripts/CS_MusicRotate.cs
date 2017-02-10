using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Adjusts audioclip start time based on it's y rotation
/// </summary>

public class CS_MusicRotate : MonoBehaviour {

	AudioSource audioSource;

	float thisClipPosition;
	float thisObjectYRotation;


	void Start () {

		audioSource = gameObject.GetComponent<AudioSource> ();
		thisObjectYRotation = Mathf.Abs(transform.rotation.eulerAngles.y / 360f);
		if (audioSource.clip != null) {
			thisClipPosition = audioSource.clip.length * (thisObjectYRotation / 16f);
		}
		audioSource.PlayScheduled(AudioSettings.dspTime + thisClipPosition);
	}



	public void UsedByPlayer() {
		PlayObjectClip ();
	}


	void Update () {

		// TEST INPUT ONLY -- COMMENT OUT IF NOT TESTING
		/*
		if (Input.GetKeyDown (KeyCode.G)) {
			UsedByPlayer ();
		}
		*/

		thisObjectYRotation = Mathf.Abs(transform.rotation.eulerAngles.y / 360f);
	}


	void PlayObjectClip () {
		thisClipPosition = audioSource.clip.length * (thisObjectYRotation / 16f);
		audioSource.PlayScheduled(AudioSettings.dspTime + thisClipPosition);
	}
		
}
