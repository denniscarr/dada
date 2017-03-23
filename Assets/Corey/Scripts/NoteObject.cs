using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour {

	AudioClip [] tones;

	AudioSource thisSource;

	// Use this for initialization
	void Start () {
		tones = Resources.LoadAll<AudioClip> ("Tones");

		thisSource = GetComponent<AudioSource> ();
		thisSource.clip = tones[Random.Range(0, tones.Length)];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
