using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InvokeRepeating ("lookAtPlayer", Random.Range(0.00f, 2.00f), 0.5f);
	}

	void Update(){
		lookAtPlayer ();
	}

	void lookAtPlayer(){
		Vector3 targetPosition = Services.Player.transform.position;
		targetPosition.y = transform.position.y;
		transform.LookAt(targetPosition);
		transform.Rotate (0, 180, 0);
	}
}
