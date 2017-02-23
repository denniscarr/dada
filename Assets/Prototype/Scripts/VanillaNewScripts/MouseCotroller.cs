using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InterationState{
	NONE_SELECTED_STATE = 0,//no thing select mode
	DRAG_STATE = 1,
	// = 2,//
}

public class MouseCotroller : MonoBehaviour {

	Camera UpperCamera;
	InterationState state;
	Transform selectedObject;
	Plane draggedPlane;
	Transform cubeOndraggedPlane;

	// Use this for initialization
	void Start () {
		state = InterationState.DRAG_STATE;
		selectedObject = cubeOndraggedPlane;
		UpperCamera = GameObject.Find("UpperCamera").GetComponent<Camera>();
		Cursor.visible = false;
		cubeOndraggedPlane = GameObject.Find("CubeOnSelectedPlane").transform;
		draggedPlane.SetNormalAndPosition(UpperCamera.transform.forward,transform.position);
		//cubeOnSelectedPlane.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//update the mouse position
		transform.position = Input.mousePosition;

		switch(state){
		case InterationState.NONE_SELECTED_STATE:DetectSelection();break;
			//selected object follows the mouse
		case InterationState.DRAG_STATE:UpdateDraggedObjectPosition(selectedObject);break;
		}


	}

	//detect if mouse click on something, then switch and save the selected object
	void DetectSelection(){
		//when click except visor
		if(Input.GetMouseButtonDown(0) && isSelecting()){
			
			//detect ray from main camera
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				//check if click on something selectable
				if(isAbleToBeCarried(hit.collider.GetComponentInChildren<InteractionSettings>())){
					Debug.Log("Player picked up "+ hit.collider.name);

					state = InterationState.DRAG_STATE;
					selectedObject = hit.collider.transform;

					//change the parent of selected object
					selectedObject.SetParent(UpperCamera.transform.parent);
					//update the postion
					UpdateDraggedObjectPosition(selectedObject);
				}
				//Debug.Log("is selecting");
			}
		}


	}

	void UpdateDraggedObjectPosition(Transform draggedObject){

		//get the ray
		Ray ray = UpperCamera.ScreenPointToRay(Input.mousePosition);

		//update the mouse position when mouse ray intersects with the plane
		float rayDistance = 10;
		//update plane
		draggedPlane.SetNormalAndPosition(UpperCamera.transform.forward,cubeOndraggedPlane.position);
		if (draggedPlane.Raycast (ray, out rayDistance)) {
			draggedObject.position = ray.GetPoint (rayDistance);
			// Offset cursor position.
			//draggedObject.position = new Vector3(transform.position.x /*+ cursorOffset.x*/, transform.position.y /*+ cursorOffset.y*/, transform.position.z);
		}

	}

	//if this object can be interacted with, return true; else return false
	bool isAbleToBeCarried(InteractionSettings interactionSettings){
		if (interactionSettings != null && interactionSettings.ableToBeCarried) {
			return true;
		}else{
			return false;
		}

	}

	//if mouse is on visor, return false, else return true
	bool isSelecting(){
		//get the ray to check whether player points at visor from upper camera
		Ray ray = UpperCamera.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		if (Physics.Raycast (ray, out hit) && hit.collider.name.Equals("PlayerVisor")) {
			return false;
		}else{
			return true;
		}

	}




}
