using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionSound : MonoBehaviour {
    public AudioClip[] explosionClip;
    private AudioSource source;
	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        source.PlayOneShot(explosionClip[Random.Range(0, explosionClip.Length)]);
        Destroy(gameObject, 5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
