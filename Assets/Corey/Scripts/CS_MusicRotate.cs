using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_MusicRotate : MonoBehaviour {

	//public List<GameObject> soundableObjects;
	AudioSource audioSource;

	public float thisClipPosition;

	public float thisCubeRotation;


	// Use this for initialization
	void Start () {

		audioSource = gameObject.GetComponent<AudioSource> ();
		thisCubeRotation = Mathf.Abs(transform.rotation.eulerAngles.y / 360f);
		thisClipPosition = audioSource.clip.length * (thisCubeRotation / 16f);

		audioSource.PlayScheduled(AudioSettings.dspTime + thisClipPosition);
	}

	// Update is called once per frame
	void Update () {

		thisCubeRotation = Mathf.Abs(transform.rotation.eulerAngles.y / 360f);
		

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

	public void PlayCubeClip () {
		thisClipPosition = audioSource.clip.length * (thisCubeRotation / 16f);
		audioSource.PlayScheduled(AudioSettings.dspTime + thisClipPosition);

	}

	public void RotateCube() {
		
		gameObject.transform.Rotate (new Vector3 (0f, 90f, 0f));
	}
}
