using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCotroller : MonoBehaviour {

	Camera UpperCamera;
	const int GRAB_MODE = 0;
	const int USE_MODE = 1;
	int interactionMode = GRAB_MODE;
	// Use this for initialization
	void Start () {
		UpperCamera = GameObject.Find("UpperCamera").GetComponent<Camera>();
		Cursor.visible = false;


	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//update the mouse position
		transform.position = Input.mousePosition;

		//get the ray to check whether player points at visor
		Ray ray = UpperCamera.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			
			if(hit.collider.name.Equals("PlayerVisor")){
				//hit visor

			}
		}
	}
}
