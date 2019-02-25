
﻿//http://www.cnblogs.com/gameprogram/archive/2012/08/15/2640357.html
//http://www.blog.silentkraken.com/2010/04/06/audiomanager/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using DG.Tweening;
using UnityEngine.Serialization;

/// <summary>
/// This is mostly by Hang Ruan, using it also to assign music clips randomly
/// Currently written as a singleton.
/// </summary>

public class CS_AudioManager : MonoBehaviour {

	//public List<GameObject> soundableObjects;


	//POPULATE THIS - Audio Clips
	[Header("NPC Audio")]
	public AudioClip[] voiceClipPool;

	public AudioClip[] NPCHitPool;
	[Range(0f, 1f)] public float NPCHitVolume = 0.5f;
	public AudioClip[] NPCOnFire;
	public AudioClip[] NPCDie;
	//private List<int> voiceClipPlaylist = new List<int> ();
	//int lastVoiceSamplePlayed = int.MaxValue;
	
	[Header("Visor Audio")]
	public AudioClip enterRoomClip, exitRoomClip;
	public AudioClip toggleVisor;

	public AudioClip[] tonesClipPool;
	[HideInInspector] public AudioClip[] confirmationTones;
	[HideInInspector] public AudioClip[] tutorialTones;
	public List<AudioClip> instClipPool;

	public AudioClip equipSound;
	public AudioClip dropSound;

	public AudioClip pickupSound;
	public AudioClip buySound;
	public AudioClip getMoney;

	public AudioClip grailRejectionClip;

	public float musicFaderTime = 8.0f;


	//private static CS_AudioManager instance = null;


	private bool tutorialInProgress = false;

	//TEMP - testing sfx priority, would like more audio overloading glitches to occur
	[SerializeField] int SFXPriority;


	#region Mixer Variables

	[Header("Audio Mixer Variables")]
	[SerializeField] AudioMixer dadaMixer;

	[SerializeField] AudioMixerGroup SFXGroup;

	//[SerializeField] AudioMixerSnapshot loLandsSnapshot, hiLandsSnapshot;
	//AudioMixerSnapshot[] altitudeBlend;

	public float npcStemVol, inkStemVol, imageStemVol, nonPickupVol, pickupVol;

	#endregion
	
	[FormerlySerializedAs("myPrefabSFX")]
	[Header("SFX Prefab Objects")]
	[SerializeField] GameObject genSFXPrefab;

	[SerializeField] GameObject useSFXPrefab;



	//========================================================================
	//enforce singleton pattern



	void Awake () {

		//altitudeBlend = new AudioMixerSnapshot[] {hiLandsSnapshot, loLandsSnapshot};
		
		tonesClipPool = Resources.LoadAll<AudioClip> ("Tones");

		if (voiceClipPool.Length == 0) {
			voiceClipPool = Resources.LoadAll<AudioClip>("Voice");
		}

		confirmationTones = Resources.LoadAll<AudioClip> ("ConfirmationTones");
		tutorialTones = Resources.LoadAll<AudioClip> ("TutorialTones");

		dadaMixer.SetFloat("ImageSpriteVol", -50f);
		dadaMixer.SetFloat("InkSpriteVol", -50f);
		dadaMixer.SetFloat("NPCStemVol", -50f);
		dadaMixer.SetFloat("NonPickupVol", -50f);

		//PopulateRandomList (voiceClipPool, ClipsPlayList);
		//DontDestroyOnLoad(this.gameObject);
	}
	//========================================================================
	//SFX INSTANTIATION
	//========================================================================

	public void PlaySFX (AudioClip g_SFX, float g_volume = 1.0f, float g_Pitch = 1.0f) {
        if (g_SFX == null) return;
		
		//Seeing if high priority makes a difference
		
		GameObject t_SFX = Instantiate (genSFXPrefab) as GameObject;
		t_SFX.name = "SFX_" + g_SFX.name;
		AudioSource sfxSource = t_SFX.GetComponent<AudioSource>();
		
		//Seeing if high priority makes a difference
		int priority = Mathf.RoundToInt(
			remapRange(
				Services.IncoherenceManager._globalIncoherence,
				0f, 1f,
				1f, 0f
			) * 255
		);

		sfxSource.priority = SFXPriority;
		sfxSource.clip = g_SFX;
		sfxSource.pitch = g_Pitch;
		sfxSource.spatialBlend = 0.0f;
		sfxSource.outputAudioMixerGroup = SFXGroup;
		sfxSource.volume = g_volume;
		sfxSource.Play ();
		DestroyObject(t_SFX, g_SFX.length);
	}

	/// <summary>
	/// Play AudioClip in 3D space with input position
	/// </summary>
	/// <param name="g_SFX">AudioClip to Play</param>
	/// <param name="g_position">Global position to play (fixed)</param>
	/// <param name="g_volume">clip volume</param>
	/// <param name="g_pitch">clip pitch</param>
	public void Play3DSFX(AudioClip g_SFX, Vector3 g_position, float g_volume = 1.0f, float g_pitch = 1.0f) {
        if (g_SFX == null) return;
		GameObject t_SFX = Instantiate (genSFXPrefab) as GameObject;
		AudioSource sfxSource = t_SFX.GetComponent<AudioSource>();
		
		//Seeing if high priority makes a difference
		int priority = Mathf.RoundToInt(
			remapRange(
				Services.IncoherenceManager._globalIncoherence,
				0f, 1f,
				1f, 0f
			) * 255
		);
		
		t_SFX.name = "SFX_" + g_SFX.name;
		t_SFX.transform.position = g_position;
		sfxSource.priority = SFXPriority;
		sfxSource.clip = g_SFX;
		sfxSource.volume = g_volume;
		sfxSource.pitch = g_pitch;
		sfxSource.outputAudioMixerGroup = SFXGroup;
		sfxSource.Play ();
		DestroyObject(t_SFX, g_SFX.length);
	}
	
	
	/// <summary>
	/// Play AudioClip in 3D space with input transform parent
	/// </summary>
	/// <param name="g_SFX">AudioClip to play</param>
	/// <param name="g_transform">Transform parent (position will be the parent's)</param>
	/// <param name="g_volume">Clip volume</param>
	/// <param name="g_pitch">Clip pitch</param>
	public void Play3DSFX(AudioClip g_SFX, Transform g_transform, float g_volume = 1.0f, float g_pitch = 1.0f) {
		if (g_SFX == null) return;
		GameObject t_SFX = Instantiate (genSFXPrefab) as GameObject;
		AudioSource sfxSource = t_SFX.GetComponent<AudioSource>();
		
		//Seeing if high priority makes a difference
		int priority = Mathf.RoundToInt(
			remapRange(
				Services.IncoherenceManager._globalIncoherence,
				0f, 1f,
				1f, 0f
			) * 255
		);
		
		t_SFX.name = "SFX_" + g_SFX.name;
		t_SFX.transform.position = g_transform.position;
		t_SFX.transform.parent = g_transform;
		sfxSource.priority = SFXPriority;
		sfxSource.clip = g_SFX;
		sfxSource.volume = g_volume;
		sfxSource.pitch = g_pitch;
		sfxSource.outputAudioMixerGroup = SFXGroup;
		sfxSource.Play ();
		DestroyObject(t_SFX, g_SFX.length);
	}

	/// <summary>
	/// Plays a 3D sound effect with the UseSFXPrefab on the Services Game Object, using an input position
	/// </summary>
	/// <param name="g_SFX">AudioClip to Play</param>
	/// <param name="g_position">Position to play the clip</param>
	/// <param name="g_volume">volume of the clip</param>
	/// <param name="g_pitch">pitch of the clip</param>
	public void PlayUseSFX(AudioClip g_SFX, Vector3 g_position, float g_volume = 1.0f, float g_pitch = 1.0f) {
		if (g_SFX == null) return;
		GameObject t_SFX = Instantiate (useSFXPrefab) as GameObject;
		AudioSource sfxSource = t_SFX.GetComponent<AudioSource>();
		
		//Seeing if high priority makes a difference
		int priority = Mathf.RoundToInt(
			remapRange(
				Services.IncoherenceManager._globalIncoherence,
				0f, 1f,
				1f, 0f
			) * 255
		);
		
		t_SFX.name = "SFX_" + g_SFX.name;
		t_SFX.transform.position = g_position;
		sfxSource.priority = SFXPriority;
		sfxSource.clip = g_SFX;
		sfxSource.volume = g_volume;
		sfxSource.pitch = g_pitch;
		sfxSource.outputAudioMixerGroup = SFXGroup;
		sfxSource.Play ();
		DestroyObject(t_SFX, g_SFX.length);
	}
	
	/// <summary>
	/// Plays a 3D sound effect with the UseSFXPrefab on the Services Game Object, using an input transform
	/// </summary>
	/// <param name="g_SFX">AudioClip to Play</param>
	/// <param name="g_transform">Parent transform of the audio</param>
	/// <param name="g_volume">volume of the clip</param>
	/// <param name="g_pitch">pitch of the clip</param>
	public void PlayUseSFX(AudioClip g_SFX, Transform g_transform, float g_volume = 1.0f, float g_pitch = 1.0f) {
		if (g_SFX == null) return;
		GameObject t_SFX = Instantiate (useSFXPrefab) as GameObject;
		AudioSource sfxSource = t_SFX.GetComponent<AudioSource>();
		
		//Seeing if high priority makes a difference
		int priority = Mathf.RoundToInt(
			remapRange(
				Services.IncoherenceManager._globalIncoherence,
				0f, 1f,
				1f, 0f
			) * 255
		);
		
		t_SFX.name = "SFX_" + g_SFX.name;
		t_SFX.transform.position = g_transform.position;
		t_SFX.transform.parent = g_transform;
		sfxSource.priority = SFXPriority;
		sfxSource.clip = g_SFX;
		sfxSource.volume = g_volume;
		sfxSource.pitch = g_pitch;
		sfxSource.outputAudioMixerGroup = SFXGroup;
		sfxSource.Play ();
		DestroyObject(t_SFX, g_SFX.length);
	}

	//========================================================================
	//DADA-SPECIFIC FUNCTIONS
	//========================================================================

	public void PlayNPCImpactSound(Vector3 npcPosition) {
		Play3DSFX (Services.AudioManager.NPCHitPool [Random.Range (0, NPCHitPool.Length - 1)], 
			npcPosition,
			NPCHitVolume, 1.0f + Random.Range(-0.1f, 0.1f));
	}

	public static float remapRange(float oldValue, float oldMin, float oldMax, float newMin, float newMax )
	{
		float newValue = 0;
		float oldRange = (oldMax - oldMin);
		float newRange = (newMax - newMin);
		newValue = (((oldValue - oldMin) * newRange) / oldRange) + newMin;
		return newValue;
	}

	public void EqualizeStems (float n_ink, float n_image, float n_npc, float n_nonPickup, float n_pickup, float totalObjects) {
		if (GameObject.FindObjectOfType<Tutorial>() != null) {
			tutorialInProgress = true;
		}
		else tutorialInProgress = false;

		float newImageSpriteVol = -60f;
		float newInkSpriteVol = -60f;
		float newNPCStemVol = -60f;
		float newNonPickupVol = -60f;
		float newPickupVol = -60f;

		if (totalObjects > 0f) {
			if (n_image > 0f) {
				newImageSpriteVol = remapRange (n_image, 0.0f, 2f, -25f, -12f);
			}
			if (n_ink > 0f) {
				newInkSpriteVol = remapRange (n_ink, 0.0f, 20f, -25f, -15f);
			}
			if (n_npc > 0f) {
				newNPCStemVol = remapRange (n_npc, 0.0f, 15f, -10f, 5f);
			}
			if (n_nonPickup > 0f) {
				newNonPickupVol = remapRange (n_nonPickup, 0.0f, totalObjects, -20f, -10f);
			}
			if (n_pickup > 0f ) {
				newPickupVol = remapRange (n_pickup, 0.0f, totalObjects, -30f, -10f);
			}
		}
		

		inkStemVol = GetGroupLevel ("InkSpriteVol");
		imageStemVol = GetGroupLevel ("ImageSpriteVol");
		npcStemVol = GetGroupLevel ("NPCStemVol");
		nonPickupVol = GetGroupLevel ("NonPickupVol");
		pickupVol = GetGroupLevel ("PickupVol");



		if (newInkSpriteVol != inkStemVol) {
			dadaMixer.DOSetFloat("InkSpriteVol", newInkSpriteVol, musicFaderTime);
		}
		if (newImageSpriteVol != imageStemVol) {
			dadaMixer.DOSetFloat("ImageSpriteVol", newImageSpriteVol, musicFaderTime);
		}
		if (newNPCStemVol != npcStemVol) {
			dadaMixer.DOSetFloat("NPCStemVol", newNPCStemVol, musicFaderTime);
		}
		if (newNonPickupVol != nonPickupVol) {
			dadaMixer.DOSetFloat("NonPickupVol", newNonPickupVol, musicFaderTime);
		}
		if (newPickupVol != pickupVol) {
			dadaMixer.DOSetFloat("PickupVol", newPickupVol, musicFaderTime);
		}
			

	}

	public float GetGroupLevel(string mixerGroup){
		float value;
		bool result =  dadaMixer.GetFloat(mixerGroup, out value);
		if(result){
			return value;
		}else{
			return 0f;
		}
	}

	#region Graveyard
	//	public AudioClip[] Clips;
	//	private List<int> ClipsPlayList = new List<int>();
	//	private int lastSamplePlayed = System.Int16.MaxValue;
	//	private bool freshList = false;
	//
	//	private AudioSource mySource;
	//
	//	void Start()
	//	{
	//		; //populate the list of ints
	//		mySource = GetComponent<AudioSource>(); //get our audio source
	//	}
	//
	//	private void PlayClip() //plays a random clip
	//	{
	//		mySource.clip = PickClip(Clips, ClipsPlayList);
	//		mySource.Play();
	//	}
	//
	//
	//	void PopulateRandomList(AudioClip[] clips, List<int> playList) //shuffles the list
	//	{
	//		int i = 0;
	//		foreach (AudioClip clip in clips)
	//		{
	//			playList.Insert(Random.Range(0, i + 1), i);
	//			i++;
	//		}
	//		freshList = true;
	//	}
	//
	//	int noRepeatClipIndex(List<int> playList) //grab a new clip index & ensure we don't play the same clip twice 
	//	{
	//		int index = Random.Range(0, playList.Count - 1);
	//		int clipIndex = playList[index];
	//		if (freshList) //we only risk repetition when we have a new list
	//		{
	//			if (clipIndex == lastSamplePlayed) //if we got to the same list
	//			{
	//				return noRepeatClipIndex(playList); //just keep searching
	//			}
	//		}
	//		freshList = false; //no longer a fresh list
	//		playList.RemoveAt(index); //remove the int from the list
	//		lastSamplePlayed = clipIndex; //store the last sample played
	//		return clipIndex;
	//	}
	//
	//	AudioClip PickClip(AudioClip[] clips, List<int> playList) //pick a clip
	//	{
	//		if (playList.Count <= 0) //if we're out of ints in our list
	//		{
	//			PopulateRandomList(clips, playList); //make a new list
	//		}
	//		int clipIndex = noRepeatClipIndex(playList); //grab a new index
	//		return clips[clipIndex]; //return the clip at that index
	//	}

	/*

	public void RetuneRadio (Transform radioTransform) {
		//this will crossfade to the radio static sound, reassign a clip, then transition to that clip
		radioTransform.gameObject.AddComponent<SoundCrossfade>().CrossFade(radioStaticClip, 0.6f, 1f);

		StartCoroutine ("WaitForStaticEnd");

		AudioClip newClip = audioClipPool [Random.Range (0, audioClipPool.Count)];

		radioTransform.gameObject.GetComponent<SoundCrossfade> ().CrossFade (newClip, 0.6f, 2f);


	}
	*/

	//	public void AltitudeMusic() {
	//
	//		if (Services.LevelGen.currentLevel != null) {	
	//			float maxLevelHeight = ((float)Services.LevelGen.height * (float)Services.LevelGen.tileScale) + (float)Services.LevelGen.currentLevel.transform.position.y;
	//
	//			float minLevelHeight = ((float)Services.LevelGen.currentLevel.transform.position.y * (float)Services.LevelGen.tileScale) + ((float)Services.LevelGen.height * 0.9f);
	//
	//			float normalizedHeights = (float)(Services.Player.gameObject.transform.position.y - minLevelHeight) / (maxLevelHeight - minLevelHeight);
	//
	//			float clampedNormHeights = Mathf.Clamp (normalizedHeights, 0f, 1f);
	//
	//			float[] weights = new float[] { clampedNormHeights, 1.0f - clampedNormHeights };
	//			//Debug.Log (clampedNormHeights);
	//
	//			dadaMixer.TransitionToSnapshots (altitudeBlend, weights, 0.01f);
	//		}
	//
	//	}


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

	/*
	public void ReassignMusic () {

		soundSources.Clear ();

		//This will find every object that has canBeUsedAsSoundSource == true and assign it a random clip from the pool
		foreach (InteractionSettings intSettings in GameObject.FindObjectsOfType<InteractionSettings> ())  {
			// first find every game object with the component CS_MusicRotate
			// TODO - include all music sources

			// STUPID HACKY THING FOR PROTOTYPE!!!! (FEEL FREE TO DELETE IT LATER) 
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



	//	public IEnumerator NextClip(int sourceNumber, int clipIndex) {
	//
	//		//audio.Play();
	//		AudioSource thisSource = soundSources[sourceNumber];
	//
	//
	//		//TODO - CROSSFADE last clip and next one
	//		if (thisSource.clip != null) {
	//			yield return new WaitForSeconds ((thisSource.clip.length - thisSource.time) / 16f);
	//			thisSource.clip = audioClipPool [clipIndex];
	//		}
	//		thisSource.Play();
	//
	//	}
	//
	//	public IEnumerator WaitForStaticEnd() {
	//		yield return new WaitForSeconds(radioStaticClip.length/2);
	//
	//	}
	*/
	#endregion

}
