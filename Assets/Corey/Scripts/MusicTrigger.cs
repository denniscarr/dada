using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger: MonoBehaviour {

	public float overlapRadius;

	List<AudioSource> objectSources;

	float playProbability;

	// Use this for initialization
	void Start () {

		playProbability = 0.1f;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		Collider[] collidedObjects = Physics.OverlapSphere (transform.position, overlapRadius);
		objectSources = new List<AudioSource> ();
		foreach (Collider collider in collidedObjects) {
			if (collider.gameObject.tag == "ToneTrigger") {
				objectSources.Add (collider.gameObject.GetComponent<AudioSource>());
			}
		}

		foreach (AudioSource source in objectSources) {
			if (source.isPlaying) {
				playProbability += 0.1f;
			}
		}
		foreach (AudioSource source in objectSources) {
			if (Random.value < playProbability) {
				source.Play ();
			}
		}
	}
}
