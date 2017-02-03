using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicShifter : MonoBehaviour {
	public AudioClip otherClip;
	public AudioSource audioSource;

	public void NextClip () {

	}

//	public IEnumerator NextClip() {
//			
//		//audio.Play();
//		yield return new WaitForSeconds(audioSource.clip.length - audioSource.time);
//		audioSource.clip = otherClip;
//		Debug.Log ("why the fuck isn't this working");
//		audioSource.Play();
//
//	}
}