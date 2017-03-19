using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greeter : MonoBehaviour {
    public GameObject boxshelf;
    public KeyCode useBoxshelf = KeyCode.Mouse0;
    public AudioClip[] greeters;
    public AudioSource boxshelfAudio; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (boxshelf.transform.parent != null && Input.GetKeyDown(useBoxshelf))
        {
            boxshelfAudio.PlayOneShot(greeters[Random.Range(0, 2)]);
        }
	}
}
