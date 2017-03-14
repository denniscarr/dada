using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger: MonoBehaviour {

	public float overlapRadius;

	public List<AudioSource> objectSources;

	public float playProbability;

	public int numberPlaying;
	public float percentPlaying;

	public float baseProbability = 0.001f;

	// Use this for initialization
	void Start () {

		playProbability = baseProbability;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		numberPlaying = 0;
		Collider[] collidedObjects = Physics.OverlapSphere (transform.position, overlapRadius);
		objectSources = new List<AudioSource> ();
		foreach (Collider collider in collidedObjects) {
			if (collider.gameObject.tag == "ToneTrigger") {
				objectSources.Add (collider.gameObject.GetComponent<AudioSource>());
			}
		}


		//find all toneSources, find out if they're playing
		foreach (AudioSource source in objectSources) {
			if (source.isPlaying) {
				numberPlaying++;
			}
		}

		//adjust the playProbability based on number of sound sources playing
		if (objectSources.Count > 0) {
			percentPlaying = numberPlaying / objectSources.Count;
		}

		playProbability = Mathf.Clamp(percentPlaying, 1f, 10f);

		//adjust playProbability to fixed delta step
		playProbability *= baseProbability;


		foreach (AudioSource source in objectSources) {
			if (Random.value < playProbability && !source.isPlaying) {
				source.Play ();
			}
		}
	}
}
