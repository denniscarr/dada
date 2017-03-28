using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinkleScript : MonoBehaviour {
    public Vector3 sprinklerP;
    public GameObject sprinkle;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        sprinklerP = GameObject.Find("Sprinkler").transform.position;
        Instantiate(sprinkle, sprinklerP, Quaternion.identity);
	}
}
