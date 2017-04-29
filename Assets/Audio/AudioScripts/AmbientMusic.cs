﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientMusic : MonoBehaviour {

	public AudioSource[] hiSource; 
	public AudioSource[] loSource;
	public AudioSource[] ambienceSource;

	public BufferShuffler[] shufflers;

	float lHue, lSat, lVal;

	float hue;

	Color currentLevelColor;

	public float fadeOutTime = 5.0f;

	[SerializeField] float crossfadeThreshold;

	void Awake() {
		shufflers = GetComponentsInChildren<BufferShuffler> ();
		foreach (BufferShuffler shuffler in shufflers) {
			shuffler.enabled = false;
		}
	}

	// Use this for initialization
	void Start () {

		

		currentLevelColor = Services.LevelGen.currentLevel.levelTint;

		Color.RGBToHSV( currentLevelColor,out lHue, out lSat,out lVal);

		//Debug.Log (hue);

		for( int i = 0; i < hiSource.Length; i ++ ) {

			//Set "hue source"
			float loHueBound = ((float)i * (1f / (float)hiSource.Length));
			float hiHueBound = ((float)(i + 1f) * (1f / (float)hiSource.Length));


			if (lHue >= loHueBound && lHue < hiHueBound) {
				//Debug.Log("range is greater than: " + loBound + "and less than " + hiBound);
				hiSource [i].volume = 1.0f;
			} else {
				hiSource [i].volume = 0.0f;
			}

		}

		for (int i = 0; i < loSource.Length; i++) {
			//Set "sat source" -- saturation goes from 0 to 0.5f
			float loSatBound = ((float)i * (1f / (float)loSource.Length));
			float hiSatBound = ((float)(i + 1f) * (1f / (float)loSource.Length));

			if (lSat >= loSatBound*0.5f && lSat < hiSatBound*0.5f) {
				
				loSource [i].volume = 1.0f;
			} else {
				loSource [i].volume = 0.0f;
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Services.LevelGen.currentLevel.levelTint != currentLevelColor) {

			Color.RGBToHSV( Services.LevelGen.currentLevel.levelTint, out lHue, out lSat, out lVal);


			for( int i = 0; i < hiSource.Length; i ++ ) {

				if (lHue >= ((float)i * (1.0f / (float)hiSource.Length)) && lHue <= ((float)(i + 1f) * (1f / (float)hiSource.Length))) {
					hiSource [i].volume = 1.0f;
				} else {
					hiSource [i].volume = 0.0f;
				}

			}

			for (int i = 0; i < loSource.Length; i++) {
				//Set "sat source" -- saturation goes from 0 to 0.5f
				float loSatBound = ((float)i * (1f / (float)loSource.Length));
				float hiSatBound = ((float)(i + 1f) * (1f / (float)loSource.Length));

				if (lSat >= loSatBound*0.5f && lSat < hiSatBound*0.5f) {
					
					loSource [i].volume = 1.0f;
				} else {
					loSource [i].volume = 0.0f;
				}
			}

		}

		if (Services.IncoherenceManager.globalIncoherence > 0.25f) {

			foreach (BufferShuffler shuffler in shufflers) {
				shuffler.enabled = true;
				shuffler.ClipToShuffle = shuffler.gameObject.GetComponent<AudioSource> ().clip;
				shuffler.SecondsPerCrossfade = 0.1f;
				float shufflerTime = CS_AudioManager.remapRange (Services.IncoherenceManager.globalIncoherence, 0.25f, 1.0f, 0.1f, 1.9f);
				shuffler.SecondsPerShuffle = 2.0f - shufflerTime;


			}

		}

	}



}
