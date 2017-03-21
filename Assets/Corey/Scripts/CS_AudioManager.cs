
﻿//http://www.cnblogs.com/gameprogram/archive/2012/08/15/2640357.html
//http://www.blog.silentkraken.com/2010/04/06/audiomanager/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

/// <summary>
/// This is mostly by Hang Ruan, using it also to assign music clips randomly
/// Currently written as a singleton.
/// </summary>

public class CS_AudioManager : MonoBehaviour {

	//public List<GameObject> soundableObjects;
	public List<AudioSource> soundSources;

	//POPULATE THIS
	public List<AudioClip> audioClipPool;
	public AudioClip[] voiceClipPool;
	public AudioClip[] tonesClipPool;
	public List<AudioClip> instClipPool;

	public AudioClip radioStaticClip;

	public float thisClipPosition;

	public float objectDensity;


	//private static CS_AudioManager instance = null;

	[SerializeField] GameObject myPrefabSFX;

	[SerializeField] AudioSource myAudioSource;

	[SerializeField] AudioMixer dadaMixer;

	[SerializeField] AudioMixerGroup SFXGroup;

	[SerializeField] AudioMixerSnapshot loLandsSnapshot, hiLandsSnapshot;
	AudioMixerSnapshot[] altitudeBlend;



	//========================================================================
	//enforce singleton pattern



	void Awake () {

		altitudeBlend = new AudioMixerSnapshot[2] {hiLandsSnapshot, loLandsSnapshot};
		
		tonesClipPool = Resources.LoadAll<AudioClip> ("Tones");
		voiceClipPool = Resources.LoadAll<AudioClip> ("Voice");
		//DontDestroyOnLoad(this.gameObject);
	}
	//========================================================================
	//SFX INSTANTIATION
	//========================================================================

	//right now have 2 overloads one for just playing an SFX clip, another for pitch ctrl
	//could make more overloads for PLaySFX in the future

	public void PlaySFX (AudioClip g_SFX) {
		GameObject t_SFX = Instantiate (myPrefabSFX) as GameObject;
		t_SFX.name = "SFX_" + g_SFX.name;
		t_SFX.GetComponent<AudioSource> ().clip = g_SFX;
		t_SFX.GetComponent<AudioSource> ().outputAudioMixerGroup = SFXGroup;
		t_SFX.GetComponent<AudioSource> ().Play ();
		DestroyObject(t_SFX, g_SFX.length);
	}

	public void PlaySFX (AudioClip g_SFX, float g_Pitch) {
		GameObject t_SFX = Instantiate (myPrefabSFX) as GameObject;
		t_SFX.name = "SFX_" + g_SFX.name;
		t_SFX.GetComponent<AudioSource> ().clip = g_SFX;
		t_SFX.GetComponent<AudioSource> ().pitch = g_Pitch;
		t_SFX.GetComponent<AudioSource> ().outputAudioMixerGroup = SFXGroup;
		t_SFX.GetComponent<AudioSource> ().Play ();
		DestroyObject(t_SFX, g_SFX.length);
	}

	public void Play3DSFX(AudioClip g_SFX, Vector3 g_position) {
		GameObject t_SFX = Instantiate (myPrefabSFX) as GameObject;
		t_SFX.name = "SFX_" + g_SFX.name;
		t_SFX.transform.position = g_position;
		t_SFX.GetComponent<AudioSource> ().clip = g_SFX;
		t_SFX.GetComponent<AudioSource> ().outputAudioMixerGroup = SFXGroup;
		t_SFX.GetComponent<AudioSource> ().Play ();
		DestroyObject(t_SFX, g_SFX.length);
	}

	//========================================================================
	//DADA-SPECIFIC FUNCTIONS
	//========================================================================



	void Start() {
		ReassignMusic ();
	}

	void Update() {
		AltitudeMusic ();
	}

	public void ReassignMusic () {

		soundSources.Clear ();

		//This will find every object that has canBeUsedAsSoundSource == true and assign it a random clip from the pool
		foreach (InteractionSettings intSettings in GameObject.FindObjectsOfType<InteractionSettings> ())  {
			// first find every game object with the component CS_MusicRotate
			// TODO - include all music sources

			/* STUPID HACKY THING FOR PROTOTYPE!!!! (FEEL FREE TO DELETE IT LATER) */
			float rand = Random.Range (0f, 1f);
			if (rand < 0.1f) {
				
			} else {

				if (intSettings.canBeUsedAsSoundSource) {
					

					Transform rootTransform = intSettings.transform.parent;
					AudioSource clipSource = rootTransform.gameObject.AddComponent<AudioSource> ();
					AudioSource staticSource = rootTransform.gameObject.AddComponent<AudioSource> ();
					staticSource.clip = radioStaticClip;

					soundSources.Add (clipSource);
					rootTransform.gameObject.AddComponent<MusicToColor> ();
					intSettings.usable = true;

					// MORE DENNIS PROTOTYPE STUFF:
					clipSource.spatialBlend = 1f;
					clipSource.maxDistance = 50f;
					clipSource.loop = true;
			
					rootTransform.gameObject.AddComponent<SoundCrossfade> ();
				}
				for (int i = 0; i < soundSources.Count; i++) {
					//StartCoroutine (NextClip (i, Random.Range (0, audioClipPool.Count)));
					soundSources [i].clip = audioClipPool [Random.Range (0, audioClipPool.Count)];
					soundSources [i].Play ();
				}
			}
		}
	}


	public IEnumerator NextClip(int sourceNumber, int clipIndex) {

		//audio.Play();
		AudioSource thisSource = soundSources[sourceNumber];


		//TODO - CROSSFADE last clip and next one
		if (thisSource.clip != null) {
			yield return new WaitForSeconds ((thisSource.clip.length - thisSource.time) / 16f);
			thisSource.clip = audioClipPool [clipIndex];
		}
		thisSource.Play();

	}

	public IEnumerator WaitForStaticEnd() {
		yield return new WaitForSeconds(radioStaticClip.length/2);

	}

	public void RetuneRadio (Transform radioTransform) {
		//this will crossfade to the radio static sound, reassign a clip, then transition to that clip
		radioTransform.gameObject.AddComponent<SoundCrossfade>().CrossFade(radioStaticClip, 0.6f, 1f);

		StartCoroutine ("WaitForStaticEnd");

		AudioClip newClip = audioClipPool [Random.Range (0, audioClipPool.Count)];

		radioTransform.gameObject.GetComponent<SoundCrossfade> ().CrossFade (newClip, 0.6f, 2f);


	}

	public void AltitudeMusic() {
		float maxLevelHeight = ((float) Services.LevelGen.height * (float)Services.LevelGen.tileScale) + (float) Services.LevelGen.currentLevel.transform.position.y;

		float minLevelHeight = (float) Services.LevelGen.currentLevel.transform.position.y * (float)Services.LevelGen.tileScale;

		float normalizedHeights = (float) (Services.Player.gameObject.transform.position.y - minLevelHeight) / (maxLevelHeight - minLevelHeight);

		float clampedNormHeights = Mathf.Clamp (Mathf.Log (normalizedHeights) + 1f, 0f, 1f);

		float[] weights = new float[2] {clampedNormHeights,1.0f - clampedNormHeights};

		dadaMixer.TransitionToSnapshots (altitudeBlend, weights, 0.0f);

	}


	//================================================================================
	// Background Music Functions
	//================================================================================
	//This stuff will probably be useless to the Dada game


	/*
	public void PlayBGM (AudioClip g_BGM) {
		if (myAudioSource.isPlaying == false) {
			myAudioSource.clip = g_BGM;
			myAudioSource.Play ();
			return;
		}

		if (g_BGM == myAudioSource.clip)
			return;

		myAudioSource.Stop ();
		myAudioSource.clip = g_BGM;
		myAudioSource.Play ();
	}

	public void PlayBGM (AudioClip g_BGM, float g_Volume) {
		if (myAudioSource.isPlaying == false) {
			myAudioSource.clip = g_BGM;
			myAudioSource.volume = g_Volume;
			myAudioSource.Play ();
			return;
		} else if (g_BGM == myAudioSource.clip) {
			myAudioSource.volume = g_Volume;
			return;
		}

		myAudioSource.Stop ();
		myAudioSource.clip = g_BGM;
		myAudioSource.volume = g_Volume;
		myAudioSource.Play ();
	}

	public void StopBGM () {
		myAudioSource.Stop ();
	}
	*/


}
