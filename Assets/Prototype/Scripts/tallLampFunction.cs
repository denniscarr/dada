using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tallLampFunction : MonoBehaviour {
    public GameObject tallLamp;
    public KeyCode useTallLamp = KeyCode.Mouse0;
    public AudioClip[] insults;
    public AudioSource tallLampAudio;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (tallLamp.transform.parent != null && Input.GetKeyDown(useTallLamp))
        {
            tallLampAudio.PlayOneShot(insults[Random.Range(0, 2)]);
        }
	}
}
