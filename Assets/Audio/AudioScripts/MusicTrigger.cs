using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to player object to trigger tone 
/// objects in the scene
/// </summary>
public class MusicTrigger: MonoBehaviour {

	public float overlapRadius;

	public List<AudioSource> objectSources;

	public float playProbability;

	public int numberPlaying;
	public float percentPlaying;

	public float baseProbability = 0.001f;

	public float totalObjects = 0f;
	public float nonPickups = 0f;
	public float inkSprites = 0f;
	public float imageSprites = 0f;
	public float npcs = 0f;

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

		totalObjects = 0f;
		nonPickups = 0f;
		inkSprites = 0f;
		imageSprites = 0f;
		npcs = 0f;

		foreach (Collider collider in collidedObjects) {

			if (collider.gameObject.tag == "ImageSprite") {
				imageSprites++;
			} else if (collider.gameObject.tag == "InkSprite") {
				inkSprites++;
			} else if (collider.gameObject.GetComponentInChildren<InteractionSettings> () != null &&
				collider.gameObject.GetComponentInChildren<InteractionSettings> ().ableToBeCarried == false && 
				collider.gameObject.GetComponentInChildren<NPC>() == null) {
				//Debug.Log ("nonpickup object");
				nonPickups++;
			} else if (collider.gameObject.GetComponentInChildren<NPC> () != null) {
				npcs ++;
			}


			if (collider.gameObject.tag == "ToneTrigger") {
				objectSources.Add (collider.gameObject.GetComponent<AudioSource>());
			}
		}

		totalObjects = imageSprites + inkSprites + nonPickups + npcs;

		//Debug.Log (nonPickups);

		imageSprites = Mathf.Clamp (imageSprites, 0f, 20f);
		inkSprites = Mathf.Clamp (inkSprites, 0f, 20f);
		nonPickups = Mathf.Clamp (nonPickups, 0f, 20f);
		npcs = Mathf.Clamp (npcs, 0f, 15f);
		totalObjects = Mathf.Clamp (totalObjects, 0f, 20f);

		//Debug.Log (inkSprites);


		Services.AudioManager.EqualizeStems (inkSprites, imageSprites, npcs, nonPickups, totalObjects);



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
