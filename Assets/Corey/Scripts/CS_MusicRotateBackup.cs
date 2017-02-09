using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_MusicRotateBackup : MonoBehaviour {

	//public List<GameObject> soundableObjects;
	AudioSource audioSource;

	public float thisClipPosition;

	public float thisObjectYRotation;


	void Start () {

		audioSource = gameObject.GetComponent<AudioSource> ();
		thisObjectYRotation = Mathf.Abs(transform.rotation.eulerAngles.y / 360f);
		thisClipPosition = audioSource.clip.length * (thisObjectYRotation / 16f);

		audioSource.PlayScheduled(AudioSettings.dspTime + thisClipPosition);
	}


	void Update () {
		thisObjectYRotation = Mathf.Abs(transform.rotation.eulerAngles.y / 360f);
	}

	/*
	public IEnumerator NextClip(int sourceNumber, int clipIndex) {

		//audio.Play();
		AudioSource thisSource = soundSources[sourceNumber];



		yield return new WaitForSeconds((thisSource.clip.length - thisSource.time)/32f);
		thisSource.clip = audioClipPool[clipIndex];

		thisSource.Play();

	}
	*/

	public void PlayObjectClip () {
		thisClipPosition = audioSource.clip.length * (thisObjectYRotation / 16f);
		audioSource.PlayScheduled(AudioSettings.dspTime + thisClipPosition);
	}

	public void RotateObject() {
		gameObject.transform.Rotate (new Vector3 (0f, 90f, 0f));
	}
}
