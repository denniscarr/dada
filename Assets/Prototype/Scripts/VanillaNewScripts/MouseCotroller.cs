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
//	INROOM_MODE = 2,
}

public class MouseCotroller : MonoBehaviour {
	public Sprite grabHand;
	public Sprite useHand;

	public float throwForce = 100f;
	public Camera UpperCamera;

	InterationState state;
	InteractionMode mode;
	//store the selected object, which will be null after deselecting
	Transform selectedObject;

	//inpoint get from the reference cube for dragging plane, which is visible at the scene but hide later
	Plane draggedPlane;
	public Transform cubeOnDraggedPlane;
	public PlayerController playercontroller;
	Vector3 inPointForPlaneFromCube;

	//count the time between pickup and place,prevent from vaild click repeatly in a second
	float clickGapCount;

	//For Sound Effects
	CS_PlaySFX sfxScript;
	public Text txtInfo;

	float CLICKGAPTIME = 0.3f;

	// Use this for initialization
	void Start () {
		//txtInfo = transform.parent.FindChild("txtInfo").GetComponent<Text>();
		clickGapCount = 0;
		state = InterationState.NONE_SELECTED_STATE;
		//UpperCamera = GameObject.Find("UpperCamera").GetComponent<Camera>();
		Cursor.visible = false;
		//cubeOnDraggedPlane = GameObject.Find("CubeOnSelectedPlane").transform;
		selectedObject = null;
		inPointForPlaneFromCube = cubeOnDraggedPlane.position;
		draggedPlane.SetNormalAndPosition(
			UpperCamera.transform.forward,
			inPointForPlaneFromCube);
		cubeOnDraggedPlane.gameObject.SetActive(false);

		sfxScript = gameObject.GetComponent<CS_PlaySFX> ();

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//update the mouse position
		transform.position = Input.mousePosition;
//		Debug.Log(Camera.main.transform.forward);
		switch(mode){
		case InteractionMode.GRAB_MODE:GrabHandler();;break;
		case InteractionMode.USE_MODE:UseHandler();;break;
		}

		//Debug.Log(playercontroller.controlMode)
		if(Input.GetKeyDown(KeyCode.E) && playercontroller.controlMode == ControlMode.ZOOM_OUT_MODE){
			txtInfo.text = "To equip, please press TAB.";
			Debug.Log("press");
		}
			
	}


	void ChangeToGrabMode() {
		Debug.Log("Change To Grab Mode");
		clickGapCount = 0;
		mode = InteractionMode.GRAB_MODE;
		UpdateCursorImage (grabHand);
		GetComponent<RectTransform> ().pivot = new Vector2(0.5f,0.5f);
	}

	void ChangeToUseMode() {
		Debug.Log("Change To Use Mode");
		clickGapCount = 0;
		mode = InteractionMode.USE_MODE;
		UpdateCursorImage (useHand);
		GetComponent<RectTransform> ().pivot = new Vector2(0.6922021f,0.8386418f);

	}

	void UpdateCursorImage(Sprite newCursor) {
		GetComponent<Image> ().sprite = newCursor;
	}
		
	void UseHandler(){
		//if(Input.GetMouseButtonDown(0)){
		clickGapCount += Time.deltaTime;
		if(clickGapCount > CLICKGAPTIME){
			//a switch used to save if use something in inventory
			bool isCollidingSomethingInVisor = false;
			//get the ray to check whether player points at visor from upper camera
			Ray ray = UpperCamera.ScreenPointToRay(Input.mousePosition);
			//Debug.DrawRay(ray.origin,ray.direction);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)){
				if(hit.collider.name.Contains("Visor")){
					GetComponent<Image> ().color = new Color(1,0,1,1);
					txtInfo.text = "Want to enter your room?";
					if(playercontroller.controlMode != ControlMode.IN_ROOM_MODE && Input.GetMouseButtonDown(0)){
						Debug.Log("click on player visor");
						playercontroller.SendMessage("ChangeToInRoomMode");
					}
				}else{
					txtInfo.text = "This thing can't be picked up.";
					isCollidingSomethingInVisor = true;

					InteractionSettings interactionSettings = hit.transform.GetComponentInChildren<InteractionSettings> ();
					if (isAbleToBeUse(interactionSettings)) {
						
						GetComponent<Image> ().color = new Color(1,1,1,1);
						txtInfo.text = hit.collider.name + " is useful.";
						if(Input.GetMouseButtonDown(0)){
							clickGapCount = 0;
							sfxScript.PlaySFX (0);

							Debug.Log("use "+hit.collider.name+" inside visor");
							hit.collider.BroadcastMessage ("Use", SendMessageOptions.DontRequireReceiver);
						}
					}
					else{
						//txtInfo.text = "This thing can't be picked up.";
						GetComponent<Image> ().color = new Color(1,0,0,0.5f);
						txtInfo.text = hit.collider.name+" is useless.";
					}

				}
			}

			//if ray is colliding something in visor, then do not detect collision outside visor////may be want to change
			if(isCollidingSomethingInVisor == false){
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast (ray, out hit)){
					InteractionSettings interactionSettings = hit.transform.GetComponentInChildren<InteractionSettings> ();
					if (isAbleToBeUse(interactionSettings)) {
						GetComponent<Image> ().color = new Color(1,1,1,1);
						txtInfo.text = hit.collider.name + " is useful.";
						if(Input.GetMouseButtonDown(0)){
							Debug.Log("use "+hit.collider.name+" outside visor");
							hit.collider.BroadcastMessage ("Use", SendMessageOptions.DontRequireReceiver);
						}
					}else{
						GetComponent<Image> ().color = new Color(1,0,0,0.5f);
						txtInfo.text = hit.collider.name+" is useless.";
					}
				}

			}


		}



		//}
	}

	void GrabHandler(){
		
		clickGapCount += Time.deltaTime;
		if(clickGapCount > CLICKGAPTIME){

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
	//if(Input.GetMouseButtonDown(0)){

		//get the ray to check whether player points at visor from upper camera
		Ray ray = UpperCamera.ScreenPointToRay(Input.mousePosition);
		Debug.DrawRay(ray.origin,ray.direction);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit) && !hit.collider.tag.Equals("Visor")){
			

			if(!hit.collider.name.Equals("ground")){
				
				//txtInfo.text = "YOu ";
				CheckPickUp(hit.collider.transform);
				//PickUpObject(hit.collider.transform);
			}
			return;

		}
		//detect if click on things outside visor ray from main camera
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {

			if(!hit.collider.name.Equals("ground")){
				//if(){
					//txtInfo.text = hit.collider.name;
				//}
				CheckPickUp(hit.collider.transform);
				//PickUpObject(hit.collider.transform);
			}
		}
	//}


	}

	//check first then pick up
	void CheckPickUp(Transform pickedUpObject){
		InteractionSettings intSet = null;
		for (int i = 0; i < pickedUpObject.childCount; i++)
		{
			if (pickedUpObject.GetChild(i).parent == pickedUpObject && pickedUpObject.GetChild(i).GetComponent<InteractionSettings>() != null)
			{
				//print("donezo!");
				intSet = pickedUpObject.GetChild(i).GetComponent<InteractionSettings>();
			}
		}


		bool check =  isAbleToBeCarried(intSet);
		//Debug.Log("Check "+pickedUpObject.name+" can be carried:"+check);
		//check if click on something selectable
		if(check){
			GetComponent<Image> ().color = new Color(1,1,1,1);
			txtInfo.text = pickedUpObject.name+" waits to be picked up.";
			if(Input.GetMouseButtonDown(0)){
				clickGapCount = 0;
				selectedObject = pickedUpObject;
				state = InterationState.DRAG_STATE;
				PickUpObject(pickedUpObject);
				intSet.carryingObject = Services.Player.transform;
				Debug.Log (intSet.carryingObject);
			}
		}else{
			GetComponent<Image> ().color = new Color(1,0,0,0.5f);
			txtInfo.text = pickedUpObject.name+" refuses to be picked up.";
		}
	}

	//do the transfer actions about the object
	void PickUpObject(Transform pickedUpObject){

		if(pickedUpObject.parent != UpperCamera.transform.parent){
			//change the parent of selected object
			pickedUpObject.SetParent(UpperCamera.transform.parent);

			//change scale
			float distanceInside = Mathf.Abs(Vector3.Dot((inPointForPlaneFromCube - UpperCamera.transform.position),UpperCamera.transform.forward));//Mathf.Abs(inPointForPlaneFromCube.z - UpperCamera.transform.position.z);
			float distance = Mathf.Abs(Vector3.Dot((pickedUpObject.position - Camera.main.transform.position),Camera.main.transform.forward));
			if(distance < 0){
				Debug.Log("error");
			}
			float frustumHeightInside = distanceInside * Mathf.Tan(UpperCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
			float frustumHeight = distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
			float scale = frustumHeightInside/frustumHeight;

			//				Debug.Log(pickedUpObject.position);
			//				Debug.Log(Camera.main.transform.position);
			//				Debug.Log((pickedUpObject.position -  Camera.main.transform.position).magnitude);
			//				Debug.Log("distanceInside:"+distanceInside+" "+"distance:"+distance);
			//				Debug.Log("frustumHeightInside:"+frustumHeightInside+" "+"frustumHeight:"+frustumHeight);
			//				Debug.Log("scale: "+scale);
			pickedUpObject.localScale *= scale;

			//stop gravity simulation and free rotation
			Debug.Log("pick up "+pickedUpObject.name+" outside visor");

		}else{

			Debug.Log("pick up "+pickedUpObject.name+" inside visor");
		}

		//pickedUpObject.localScale = 
		Rigidbody body = pickedUpObject.GetComponentInChildren<Rigidbody>();
		if(body){
			body.useGravity = false;
			body.freezeRotation = true;
		}
		//set layer to ignoreraycast 
		pickedUpObject.gameObject.layer = 2;

		//update the postion
		UpdateDraggedObjectPosition(pickedUpObject);

		sfxScript.PlaySFX (0);

//        // Make sure we're reading from the correct interaction settings (rather than that of the object being carried by an NPC for instance)
//        InteractionSettings intSet = null;
//        for (int i = 0; i < pickedUpObject.childCount; i++)
//        {
//            if (pickedUpObject.GetChild(i).parent == pickedUpObject && pickedUpObject.GetChild(i).GetComponent<InteractionSettings>() != null)
//            {
//                //print("donezo!");
//                intSet = pickedUpObject.GetChild(i).GetComponent<InteractionSettings>();
//            }
//        }
//
//
//		bool check =  isAbleToBeCarried(intSet);
//		Debug.Log("Check "+pickedUpObject.name+" can be carried:"+check);
//		//check if click on something selectable
//		if(check){
//			//GetComponent<Image> ().color = new Color(1,1,1,1);
//			if(Input.GetMouseButtonDown(0)){
//				clickGapCount = 0;
//			}
//			//selectedObject = pickedUpObject;
//			//state = InterationState.DRAG_STATE;
//			//prevent from vaild click repeatly in a second
//
//
//			//calculate new scale
//
//
//
//
//		}else{
//			GetComponent<Image> ().color = new Color(1,1,1,0);
//		}
			
	}

	void UpdateDraggedObjectPosition(Transform draggedObject){
		
		//get the ray
		Ray ray = UpperCamera.ScreenPointToRay(Input.mousePosition);

		//update the mouse position when mouse ray intersects with the plane
		float rayDistance = 10;

		//update plane
		draggedPlane.SetNormalAndPosition(
			UpperCamera.transform.forward,
			inPointForPlaneFromCube);

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
				if(hit.collider.tag.Equals("Visor")){
					Debug.Log("place in visor");
					//set layer to default 
					selectedObject.gameObject.layer = 0;
				}else{
					Debug.LogError("click "+selectedObject.name+" in "+hit.collider.name);
				}
			}else{
				//set layer to default 
				selectedObject.gameObject.layer = 0;

				Debug.Log("Player threw "+selectedObject.name);
				ThrowObject();
			}
				
			clickGapCount = 0;
            Rigidbody body = selectedObject.GetComponentInChildren<Rigidbody>();
			if(body){
	            body.isKinematic = false;
	            body.useGravity = true;
	            body.freezeRotation = false;
			}
            if (selectedObject.FindChild("Incoherence Controller") != null) selectedObject.FindChild("Incoherence Controller").gameObject.SetActive(true);
            if (selectedObject.FindChild("NPC AI") != null) selectedObject.FindChild("NPC AI").gameObject.SetActive(true);
            selectedObject.GetComponentInChildren<InteractionSettings>().carryingObject = null;

            selectedObject = null;
			//change state back
			state = InterationState.NONE_SELECTED_STATE;
		}

	}

	void ThrowObject() {
		sfxScript.PlaySFX (1);

		Transform carriedObject = selectedObject;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		carriedObject.position = Camera.main.transform.position + ray.direction*5f;

		carriedObject.SetParent (carriedObject.GetComponentInChildren<InteractionSettings> ().originalParent,true);

		carriedObject.GetComponent<Rigidbody> ().AddExplosionForce (throwForce*5, Camera.main.transform.position, 10f);
	}

	//if this object can be interacted with, return true; else return false
	bool isAbleToBeCarried(InteractionSettings interactionSettings){
		if (interactionSettings != null && interactionSettings.ableToBeCarried) {
			return true;
		}else{
			return false;
		}

	}

	bool isAbleToBeUse(InteractionSettings interactionSettings){
		if (interactionSettings != null) {
			if (interactionSettings.usable) {
				return true;

			}
		}
		return false;
	}

	public void StopHoldingItemInMouse()
	{
		Debug.Log ("hi");

		selectedObject.gameObject.layer = 0;
		selectedObject = null;
		//change state back
		state = InterationState.NONE_SELECTED_STATE;

	}
}
