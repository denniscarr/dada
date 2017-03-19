using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sofaFunction : MonoBehaviour {
    public GameObject sofa;
    public KeyCode useSofa = KeyCode.Mouse0;
    public AudioClip fart;
    public AudioSource sofaAudio;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (sofa.transform.parent != null && Input.GetKeyDown(useSofa))
        {
            sofaAudio.PlayOneShot(fart);
        }
	}
}
