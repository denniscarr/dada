﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_MusicRotate : MonoBehaviour {

	AudioSource audioSource;

	public float thisClipPosition;

	public float thisObjectYRotation;


	void Start () {

		audioSource = gameObject.GetComponent<AudioSource> ();
		thisObjectYRotation = Mathf.Abs(transform.rotation.eulerAngles.y / 360f);
		thisClipPosition = audioSource.clip.length * (thisObjectYRotation / 16f);

		audioSource.PlayScheduled(AudioSettings.dspTime + thisClipPosition);
	}


	void UsedByPlayer() {
		RotateObject ();
	}


	void Update () {
		thisObjectYRotation = Mathf.Abs(transform.rotation.eulerAngles.y / 360f);
	}


	void PlayObjectClip () {
		thisClipPosition = audioSource.clip.length * (thisObjectYRotation / 16f);
		audioSource.PlayScheduled(AudioSettings.dspTime + thisClipPosition);
	}


	void RotateObject() {
		gameObject.transform.Rotate (new Vector3 (0f, 90f, 0f));
	}
}
