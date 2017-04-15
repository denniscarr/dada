using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSound : MonoBehaviour {

	float overlapRadius = 10f; 

	AudioSource thisAudioSource;

	Collider thisCollider;

	void Start() {

		thisAudioSource = GetComponent<AudioSource> ();
		thisAudioSource.clip = Services.AudioManager.confirmationTones [Random.Range (0, Services.AudioManager.confirmationTones.Length)];
	}

	public void PlaySound() {

		thisAudioSource.Play ();
		Debug.Log ("sprite played");

		Collider[] overlapObjects = Physics.OverlapSphere (transform.position, overlapRadius);

		foreach (Collider collider in overlapObjects) {
			if (collider.gameObject.GetComponent<SpriteRenderer> () != null && !collider.gameObject.GetComponent<AudioSource> ().isPlaying) {
				Invoke ("PlayExternalSound", 0.3f);
			}
		}


	}

	public void PlayExternalSound(GameObject otherObject) {
		otherObject.GetComponent<SpriteSound> ().PlaySound();
	}

}
