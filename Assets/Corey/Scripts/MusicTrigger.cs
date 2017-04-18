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

	public int totalObjects = 0;
	public int nonPickups = 0;
	public int inkSprites = 0;
	public int imageSprites = 0;
	public int npcs = 0;

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

		totalObjects = 0;
		nonPickups = 0;
		inkSprites = 0;
		imageSprites = 0;
		npcs = 0;

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

		Mathf.Clamp (imageSprites, 0f, 20f);
		Mathf.Clamp (inkSprites, 0f, 20f);
		Mathf.Clamp (nonPickups, 0f, 20f);
		Mathf.Clamp (npcs, 0f, 15f);
		Mathf.Clamp (totalObjects, 0f, 20f);

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
