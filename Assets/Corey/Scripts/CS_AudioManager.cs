//http://www.cnblogs.com/gameprogram/archive/2012/08/15/2640357.html
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

	public float thisClipPosition;


	private static CS_AudioManager instance = null;

	[SerializeField] GameObject myPrefabSFX;

	[SerializeField] AudioSource myAudioSource;

	[SerializeField] AudioMixerGroup SFXGroup;



	//========================================================================
	//enforce singleton pattern

	public static CS_AudioManager Instance {
		get { 
			return instance;
		}
	}

	void Awake () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}

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

	//========================================================================
	//DADA-SPECIFIC FUNCTIONS
	//========================================================================

	void Start() {
		ReassignMusic ();
	}

	public void ReassignMusic () {

		soundSources.Clear ();

		//This will find every object that has canBeUsedAsSoundSource == true and assign it a random clip from the pool
		foreach (InteractionSettings intSettings in GameObject.FindObjectsOfType<InteractionSettings> ())  {
			// first find every game object with the component CS_MusicRotate
			// TODO - include all music sources

			/* STUPID HACKY THING FOR PROTOTYPE!!!! (FEEL FREE TO DELETE IT LATER) */
			float rand = Random.Range (0f, 1f);
			if (rand < 0.75f) {
				
			} else {

				if (intSettings.canBeUsedAsSoundSource) {
					Transform rootTransform = intSettings.transform.parent;
					rootTransform.gameObject.AddComponent<AudioSource> ();
					soundSources.Add (rootTransform.gameObject.GetComponent<AudioSource> ());

					// MORE DENNIS PROTOTYPE STUFF:
					rootTransform.gameObject.GetComponent<AudioSource> ().spatialBlend = 1f;
					rootTransform.gameObject.GetComponent<AudioSource> ().maxDistance = 50f;
					rootTransform.gameObject.GetComponent<AudioSource> ().loop = true;
					rootTransform.gameObject.AddComponent<CS_MusicRotate> ();
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
