using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tennisBallExplosion : MonoBehaviour {
	public GameObject[] tennisExplosion;
	//public AudioClip explosionClip;
	//private AudioSource tennisSource;
	// Use this for initialization
	void Start () {
		//tennisSource = gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision collision) {
		Instantiate (tennisExplosion[Random.Range(0, tennisExplosion.Length)], transform.position, Quaternion.identity);
		//tennisSource.PlayOneShot (explosionClip, 1);
		Destroy (gameObject);
        //print(collision.gameObject);
        if (collision.gameObject.GetComponentInChildren<InteractionSettings>() != null)
        {
            collision.gameObject.GetComponentInChildren<InteractionSettings>().heat += 1f;
        }
	}
}
