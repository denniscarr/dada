﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CURRENTLY RENDERING THIS OBSOLETE; FUNCTIONALITY ---> CS_AUDIOMANAGER -- Corey

public class CS_MusicShifter : MonoBehaviour {

	//public List<GameObject> soundableObjects;
	public List<AudioSource> soundSources;
	public List<AudioClip> audioClipPool;

	public float thisClipPosition;


	// Use this for initialization
	void Start () {
		//This will find every object with an AudioSource and assign it a clip from the pool
		for (int i = 0; i < GameObject.FindObjectsOfType<AudioSource> ().Length; i++) {
			soundSources.Add (GameObject.FindObjectsOfType<AudioSource> () [i]);
			soundSources [i].clip = audioClipPool [Random.Range (0, audioClipPool.Count)];
			soundSources [i].Play ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.L)) {
			StartCoroutine(NextClip(Random.Range(0, soundSources.Count-1), Random.Range(0, audioClipPool.Count-1)));

			Debug.Log ("NewClip");	
		}
		
	}

	public IEnumerator NextClip(int sourceNumber, int clipIndex) {

		//audio.Play();
		AudioSource thisSource = soundSources[sourceNumber];



		yield return new WaitForSeconds((thisSource.clip.length - thisSource.time)/16f);
		thisSource.clip = audioClipPool[clipIndex];
	
		thisSource.Play();

	}

	public void PlayCubeClip () {

	}
}
