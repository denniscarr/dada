using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpriteSound : MonoBehaviour {

	float overlapRadius = 10f; 

	AudioSource thisAudioSource;

	Collider thisCollider;

	float startTimer = 0f;
	public float thisSpriteCooldown = 0.0f;
	Color origColor;

	void Start() {

		thisAudioSource = GetComponent<AudioSource> ();
		thisAudioSource.clip = Services.AudioManager.confirmationTones [Random.Range (0, Services.AudioManager.confirmationTones.Length)];
		origColor = GetComponent<SpriteRenderer> ().material.color;
	}

	void Update() {
		thisSpriteCooldown += Time.deltaTime;
	}

	public void PlaySound() {

		thisAudioSource.Play ();

		thisSpriteCooldown = 0.0f;

		//Debug.Log ("sprite played");

		Collider[] overlapObjects = Physics.OverlapSphere (transform.position, overlapRadius);




		gameObject.GetComponent<SpriteRenderer> ().material.color = Color.magenta;

		gameObject.GetComponent<SpriteRenderer> ().material.DOColor (origColor, 1.0f);

		int soundCount = 0;

		foreach (Collider collider in overlapObjects) {
			
			if (collider.gameObject.GetComponent<SpriteRenderer> () != null && !collider.gameObject.GetComponent<AudioSource> ().isPlaying && 
				collider.gameObject.GetComponent<SpriteSound> ().thisSpriteCooldown >= 1.0f ) {
				startTimer = 0f;
				StartCoroutine ("PlayExternalSound", collider.gameObject);

				soundCount++;

			}
			if (soundCount >= 3) {
				return;
			}
		}

	}



	IEnumerator PlayExternalSound(GameObject otherObject) {
		while (startTimer <= 0.25f) {
			startTimer += Time.deltaTime;
			yield return null;
		}

		otherObject.GetComponent<SpriteSound> ().PlaySound ();


	}

}
