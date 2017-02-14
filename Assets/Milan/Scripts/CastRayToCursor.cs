using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastRayToCursor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		CallFunctionOnCursorDown ();
	}

	void CallFunctionOnCursorDown(){
		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
				//Execute arbitrary code
				hit.collider.BroadcastMessage("UsedByPlayer", hit.point);
			}
		}
	}
}
