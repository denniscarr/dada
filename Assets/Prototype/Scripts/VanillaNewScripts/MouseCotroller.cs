using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InterationState{
	NONE_SELECTED_STATE = 0,//no thing select mode
	DRAG_STATE = 1,
	// = 2,//
}

public enum InteractionMode{
	GRAB_MODE = 0,
	USE_MODE = 1,
}

public class MouseCotroller : MonoBehaviour {
	public Sprite grabHand;
	public Sprite useHand;

	public float throwForce = 100f;
	Camera UpperCamera;
	InterationState state;
	InteractionMode mode;
	//store the selected object, which will be null after deselecting
	Transform selectedObject;

	//inpoint get from the reference cube for dragging plane, which is visible at the scene but hide later
	Plane draggedPlane;
	Transform cubeOnDraggedPlane;
	Vector3 inPointForPlaneFromCube;
	//count the time between pickup and place,prevent from vaild click repeatly in a second
	float clickGapCount;
	// Use this for initialization
	void Start () {
		clickGapCount = 0;
		state = InterationState.NONE_SELECTED_STATE;

		UpperCamera = GameObject.Find("UpperCamera").GetComponent<Camera>();
		Cursor.visible = false;
		cubeOnDraggedPlane = GameObject.Find("CubeOnSelectedPlane").transform;
		selectedObject = null;
		inPointForPlaneFromCube = cubeOnDraggedPlane.position;
		draggedPlane.SetNormalAndPosition(
			UpperCamera.transform.forward,
			UpperCamera.transform.parent.TransformPoint(inPointForPlaneFromCube));
		cubeOnDraggedPlane.gameObject.SetActive(false);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//update the mouse position
		transform.position = Input.mousePosition;

		switch(mode){
		case InteractionMode.GRAB_MODE:GrabHandler();;break;
		case InteractionMode.USE_MODE:UseHandler();;break;
		}
			
	}

	void ChangeToGrabMode() {
		mode = InteractionMode.GRAB_MODE;
		UpdateCursorImage (grabHand);
	}

	void ChangeToUseMode() {
		mode = InteractionMode.USE_MODE;
		UpdateCursorImage (useHand);
	}

	void UpdateCursorImage(Sprite newCursor) {
		GetComponent<Image> ().sprite = newCursor;
	}
		
	void UseHandler(){
		if(Input.GetMouseButtonDown(0)){

			//get the ray to check whether player points at visor from upper camera
			Ray ray = UpperCamera.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;
			if (Physics.Raycast (ray, out hit) && !hit.collider.name.Equals("PlayerVisor")){
				InteractionSettings interactionSettings = hit.transform.GetComponentInChildren<InteractionSettings> ();
				if (interactionSettings != null) {
					if (interactionSettings.usable) {
						Debug.Log("use "+hit.collider.name+" inside visor");
						hit.collider.BroadcastMessage ("UsedByPlayer");
					}
				}

			}
		}
	}

	void GrabHandler(){
		clickGapCount += Time.deltaTime;
		if(clickGapCount > 0.1f){

			switch(state){
			case InterationState.NONE_SELECTED_STATE:DetectSelection();break;
				//selected object follows the mouse
			case InterationState.DRAG_STATE:{
					//prevent null error
					if(selectedObject == null){
						Debug.LogError("dragged object is null");
						state = InterationState.NONE_SELECTED_STATE;
						return;
					}

					UpdateDraggedObjectPosition(selectedObject);
					DetectPlacing(selectedObject);
					break;
				}
			}

		}
	}

	//detect if mouse click on something, then switch and save the selected object
	void DetectSelection(){
		if(Input.GetMouseButtonDown(0)){

			//get the ray to check whether player points at visor from upper camera
			Ray ray = UpperCamera.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;
			if (Physics.Raycast (ray, out hit) && !hit.collider.name.Equals("PlayerVisor")){
				
				Debug.Log("click "+hit.collider.name+" inside visor");
				PickUpObject(hit.collider.transform);
				return;

			}
			//detect if click on things outside visor ray from main camera
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				
				PickUpObject(hit.collider.transform);
			}
		}


	}

	void PickUpObject(Transform pickedUpObject){

		//check if click on something selectable
		if(isAbleToBeCarried(pickedUpObject.GetComponentInChildren<InteractionSettings>())){
			
			selectedObject = pickedUpObject;
			state = InterationState.DRAG_STATE;
			//prevent from vaild click repeatly in a second
			clickGapCount = 0;

			if(pickedUpObject.parent != UpperCamera.transform.parent){
				//change the parent of selected object
				pickedUpObject.SetParent(UpperCamera.transform.parent);
				//stop gravity simulation and free rotation
				Debug.Log("click "+pickedUpObject.name+" outside visor");

			}else{
				Debug.Log("click "+pickedUpObject.name+" inside visor");
			}

			Rigidbody body = pickedUpObject.GetComponentInChildren<Rigidbody>();
			body.useGravity = false;
			body.freezeRotation = true;
			//set layer to ignoreraycast 
			pickedUpObject.gameObject.layer = 2;

			//update the postion
			UpdateDraggedObjectPosition(pickedUpObject);


		}
			
	}

	void UpdateDraggedObjectPosition(Transform draggedObject){
		
		//get the ray
		Ray ray = UpperCamera.ScreenPointToRay(Input.mousePosition);

		//update the mouse position when mouse ray intersects with the plane
		float rayDistance = 10;

		//update plane
		draggedPlane.SetNormalAndPosition(
			UpperCamera.transform.forward,
			UpperCamera.transform.parent.TransformPoint(inPointForPlaneFromCube));

		//update dragged object position
		if (draggedPlane.Raycast (ray, out rayDistance)) {
			draggedObject.position = ray.GetPoint (rayDistance);
			// Offset cursor position.
			//draggedObject.position = new Vector3(transform.position.x /*+ cursorOffset.x*/, transform.position.y /*+ cursorOffset.y*/, transform.position.z);
		}

	}

	void DetectPlacing(Transform draggedObject){
		//detect click
		if(Input.GetMouseButtonDown(0)){
			
			Ray ray = UpperCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)){
				if(hit.collider.name.Equals("PlayerVisor")){
					Debug.Log("place in visor");
					//set layer to default 
					selectedObject.gameObject.layer = 0;
				}else{
					Debug.LogError("click "+selectedObject.name);
				}
			}else{
				//set layer to default 
				selectedObject.gameObject.layer = 0;

				Debug.Log("Player threw "+selectedObject.name);
				ThrowObject();
			}
				
			clickGapCount = 0;
			Rigidbody body = selectedObject.GetComponentInChildren<Rigidbody>();
			body.useGravity = true;
			body.freezeRotation = false;

			selectedObject = null;
			//change state back
			state = InterationState.NONE_SELECTED_STATE;
		}

	}

	void ThrowObject() {

		Transform carriedObject = selectedObject;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		carriedObject.position = Camera.main.transform.position + ray.direction*5f;

		carriedObject.SetParent (carriedObject.GetComponentInChildren<InteractionSettings> ().originalParent,true);

		carriedObject.GetComponent<Rigidbody> ().AddExplosionForce (throwForce, Camera.main.transform.position, 10f);
	}

	//if this object can be interacted with, return true; else return false
	bool isAbleToBeCarried(InteractionSettings interactionSettings){
		if (interactionSettings != null && interactionSettings.ableToBeCarried) {
			return true;
		}else{
			return false;
		}

	}



}
