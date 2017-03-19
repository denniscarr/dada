using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InvokeRepeating ("lookAtPlayer", Random.Range(0.00f, 2.00f), 0.5f);
	}

	void lookAtPlayer(){
		transform.LookAt (Camera.main.transform.position);
	}
}
