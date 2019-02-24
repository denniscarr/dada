using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpeak : MonoBehaviour {

	private AudioSource _audioSource;
	private AudioClip _voiceClip;

	void Awake() {
		_audioSource = GetComponent<AudioSource>();
	}
	
	/// <summary>
	/// Picks and plays a random clip from the AudioManager VoiceClipPool
	/// Does nothing if already playing a voice clip.
	/// </summary>
	public void PlayNPCSpeak() {
		if (Services.AudioManager != null && !_audioSource.isPlaying)
		{
			_audioSource.clip = Services.AudioManager.voiceClipPool [Random.Range (0, Services.AudioManager.voiceClipPool.Length)];
			_audioSource.Play ();
		}
		
	}
}
