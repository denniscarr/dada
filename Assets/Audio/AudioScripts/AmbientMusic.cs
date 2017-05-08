using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beat;
using DG.Tweening;

public class AmbientMusic : MonoBehaviour {

	public AudioSource[] hiSource; 
	AudioSource currentHiSource;
	public AudioSource[] loSource;
	AudioSource currentLoSource;
	public AudioSource[] ambienceSource;

	public BufferShuffler[] shufflers;
	BufferShuffler currentShuffler = null;
	public bool shuffleTrigger;

	Clock myClock;


	float lHue, lSat, lVal;

	float hue;

	float shufflerTime;

	public float pitchShiftingScale;
	public float pitchShiftingTimer;

	Color currentLevelColor;

	public float fadeOutTime = 5.0f;

	float prevIncoherence = 0f;

	[SerializeField] float crossfadeThreshold;

	public void NewShuffle() {

		Debug.Log ("new shuffle");
		shuffleTrigger = true;

	}

	void Awake() {
		myClock = GetComponent<Clock> ();
		
		shufflers = GetComponentsInChildren<BufferShuffler> ();
		foreach (BufferShuffler shuffler in shufflers) {
			shuffler.MusicClip = shuffler.gameObject.GetComponent<AudioSource> ().clip;
			shuffler.enabled = false;
		}

		pitchShiftingScale = 0f;
		pitchShiftingTimer = 1f;

		//StartCoroutine ("RemapPitch");
	}

	// Use this for initialization
	void Start () {

		

		currentLevelColor = Services.LevelGen.currentLevel.levelTint;

		Color.RGBToHSV( currentLevelColor,out lHue, out lSat,out lVal);



		for( int i = 0; i < hiSource.Length; i ++ ) {


			hiSource [i].volume = 0.0f;
			currentHiSource = hiSource [i];

		}

		for (int i = 0; i < loSource.Length; i++) {
	
			loSource [i].volume = 0.0f;
			
		}

		ambienceSource [0].volume = 1.0f;
		
	}
	
	// Update is called once per frame
	void Update () {

		if (shuffleTrigger == true) {
			shuffleTrigger = false;
		}

		if (!Services.LevelGen.isTutorialCompleted) {
			//TODO: Stem for tutorial
			ambienceSource [0].volume = 1.0f;
		} else if (Services.LevelGen.currentLevel.levelTint != currentLevelColor) {

			Color.RGBToHSV( Services.LevelGen.currentLevel.levelTint, out lHue, out lSat, out lVal);


			for( int i = 0; i < hiSource.Length; i ++ ) {

				if (lHue >= ((float)i * (1.0f / (float)hiSource.Length)) && lHue <= ((float)(i + 1f) * (1f / (float)hiSource.Length))) {
					hiSource [i].DOFade(1.0f, 3.0f);
					currentHiSource = hiSource [i];
				} else {
					hiSource [i].DOFade(0.0f, 3.0f);
				}

			}

			for (int i = 0; i < loSource.Length; i++) {
				//Set "sat source" -- saturation goes from 0 to 0.5f
				float loSatBound = ((float)i * (1f / (float)loSource.Length));
				float hiSatBound = ((float)(i + 1f) * (1f / (float)loSource.Length));

				if (lSat >= loSatBound*0.5f && lSat < hiSatBound*0.5f) {
					
					loSource [i].DOFade(1.0f, 3.0f);
				} else {
					loSource [i].DOFade (0.0f, 3.0f);
				}
			}

		}



		#region buffer shuffler stuff
		if (Services.IncoherenceManager.globalIncoherence > 0.8f) {

			foreach (BufferShuffler shuffler in shufflers) {

				shuffler.SetBeatsPerShuffle (TickValue.Sixteenth);
				shuffler.SetBeatsPerCrossfade( TickValue.ThirtySecond);

				//shufflerTime = CS_AudioManager.remapRange (Services.IncoherenceManager.globalIncoherence, 0.25f, 1.0f, 0.1f, 2.9f);

				//shuffler.SecondsPerShuffle = 3.0f - (shufflerTime);
			}

		} else if (Services.IncoherenceManager.globalIncoherence > 0.6f) {

			foreach (BufferShuffler shuffler in shufflers) {

				shuffler.SetBeatsPerShuffle (TickValue.Eighth);
				shuffler.SetBeatsPerCrossfade( TickValue.Sixteenth);

				//shufflerTime = CS_AudioManager.remapRange (Services.IncoherenceManager.globalIncoherence, 0.25f, 1.0f, 0.1f, 2.9f);

				//shuffler.SecondsPerShuffle = 3.0f - (shufflerTime);
			}

		} else if (Services.IncoherenceManager.globalIncoherence > 0.4f) {

			foreach (BufferShuffler shuffler in shufflers) {

				shuffler.SetBeatsPerShuffle (TickValue.Quarter);
				shuffler.SetBeatsPerCrossfade( TickValue.Eighth);

				//shufflerTime = CS_AudioManager.remapRange (Services.IncoherenceManager.globalIncoherence, 0.25f, 1.0f, 0.1f, 2.9f);

				//shuffler.SecondsPerShuffle = 3.0f - (shufflerTime);
			}

		} else if (Services.IncoherenceManager.globalIncoherence > 0.2f) {

			foreach (BufferShuffler shuffler in shufflers) {

				shuffler.SetBeatsPerShuffle (TickValue.Half);
				shuffler.SetBeatsPerCrossfade( TickValue.Eighth);

				//shufflerTime = CS_AudioManager.remapRange (Services.IncoherenceManager.globalIncoherence, 0.25f, 1.0f, 0.1f, 2.9f);

				//shuffler.SecondsPerShuffle = 3.0f - (shufflerTime);
			}

		} else if (Services.IncoherenceManager.globalIncoherence > 0.1f) {

			foreach (BufferShuffler shuffler in shufflers) {
				if (!shuffler.enabled && shuffler.gameObject.GetComponent<AudioSource> ().volume >= 0.5f) {
					shuffler.enabled = true;
					shuffler.ClipToShuffle = shuffler.gameObject.GetComponent<AudioSource> ().clip;
				
				}

				shuffler.SetBeatsPerShuffle( TickValue.Measure);
				shuffler.SetBeatsPerCrossfade(TickValue.Eighth);

				//shufflerTime = CS_AudioManager.remapRange (Services.IncoherenceManager.globalIncoherence, 0.25f, 1.0f, 0.1f, 2.9f);

				//shuffler.SecondsPerShuffle = 3.0f - (shufflerTime);
			}
		}
		#endregion


			/*
			pitchShiftingScale = 3.0f * Services.IncoherenceManager.globalIncoherence;
			pitchShiftingTimer = 2.0f - Services.IncoherenceManager.globalIncoherence;
			*/
		prevIncoherence = Services.IncoherenceManager.globalIncoherence;

	}





	IEnumerator RemapPitch() {
		while (true) {

			yield return new WaitForSeconds (pitchShiftingTimer);
			currentHiSource.pitch =  1f + (Random.value * pitchShiftingScale) - (pitchShiftingScale/2f);

		}

	}

}
