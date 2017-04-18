using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public enum ControlMode{
	ZOOM_IN_MODE = -1,
	ZOOM_OUT_MODE = 1,//can see inventory
	IN_ROOM_MODE = 0,
}

public enum InterationState{
	NONE_SELECTED_STATE = 0,//no thing select mode
	DRAG_STATE = 1,
	// = 2,//
}

public class PlayerControllerNew : MonoBehaviour {
	ControlMode mode;

	public ControlMode Mode{
		get{
			return mode;
		}
	}

	//FirstPersonController fpController;//fps controller

	//public Transform CameraForScreen;//camera at upper node for screen display at in-visor mode
	public Camera m_Camera;//camera under the player
	public Camera UpperCamera;//camera upper

	//private Transform uppernode;
	public Transform inRoomNode;//upper node is the parent of inroomnode

	public float throwForce = 100f;

	//fov settings
	public float ZoomInUpperCameraFoV = 17f;
	public float ZoomOutUpperCameraFoV = 33f;
	public float ZoomInMainCameraFoV = 60f;
	public float ZoomOutMainCameraFoV = 80f;

	//count the time between pickup and place,prevent from vaild click repeatly in a second
	float pressGapCount;

	CS_PlaySFX playSFXScript;

	//INSTRUCTIONS TEXT do not need any more
	public Text txtInfo;

	public RigidbodyFirstPersonController rigidbodyFirstPersonController;
	public InsideVisorMan insideVisorMan;
	public HeadBob headBob;

	private Vector3 initPos;
	private Quaternion initRotation;
	private Quaternion initCameraRotation;

	private float clickGapCount;
	private const float CLICKGAPTIME = 0.3f;

	public AudioClip pickupClip;
	public AudioClip throwClip;
	public AudioClip toggleViewSFX;
	public float sfxVolume = 0.1f;

	private InterationState state;
	private Transform selectedObject;

	private Plane draggedPlane;
	public Transform cubeOnDraggedPlane;
	private Vector3 inPointForPlaneFromCube;


	// Use this for initialization
	void Start () {
		initPos = transform.position;
		initRotation = transform.rotation;
		Debug.Log("init");
		initCameraRotation = UpperCamera.transform.rotation;
		inPointForPlaneFromCube = cubeOnDraggedPlane.position;

		pressGapCount = 0f;
		clickGapCount = 0f;
		//fpController = this.GetComponent<FirstPersonController>();

		mode = ControlMode.ZOOM_OUT_MODE;
		switch(mode){
		case ControlMode.IN_ROOM_MODE:InitInRoomMode();break;
		case ControlMode.ZOOM_IN_MODE:InitZoomInMode();break;
		case ControlMode.ZOOM_OUT_MODE:InitZoomOutMode();break;
		}


	}

	public void InitInRoomMode(){
		rigidbodyFirstPersonController.enabled = true;
		insideVisorMan.enabled = true;
		headBob.enabled = true;

		//inRoomNode.gameObject.SetActive(true);

		m_Camera.fieldOfView = ZoomOutMainCameraFoV;
		UpperCamera.fieldOfView = ZoomOutUpperCameraFoV;

		Debug.Log(mode);

		Services.AudioManager.PlaySFX (Services.AudioManager.enterRoomClip, 0.5f);

	}

	void InitZoomInMode(){
		Debug.Log("reset");
		transform.position = initPos;
		transform.rotation = initRotation;

		rigidbodyFirstPersonController.enabled = false;
		insideVisorMan.enabled = false;
		headBob.enabled = false;

	}

	public void InitZoomOutMode(){
		transform.position = initPos;
		transform.rotation = initRotation;
		UpperCamera.transform.rotation = initCameraRotation;

		Cursor.lockState = CursorLockMode.None;
		rigidbodyFirstPersonController.enabled = false;
		insideVisorMan.enabled = false;
		headBob.enabled = false;


		//Services.AudioManager.PlaySFX (Services.AudioManager.exitRoomClip, 0.5f);
	}


	
	// Update is called once per frame
	void Update () {
		switch(mode){
		case ControlMode.IN_ROOM_MODE:InRoomUpdate();break;
		case ControlMode.ZOOM_IN_MODE:ZoomInUpdate();break;
		case ControlMode.ZOOM_OUT_MODE:ZoomOutUpdate();break;
		}
	}

	void InRoomUpdate(){
		//change back to zoom out when click
		Transform t_hit = CameraRayCast(UpperCamera);

		if(t_hit){
			//Debug.Log(t_hit.name);
			if(t_hit.parent.name.Equals("Viewing Platform")){
				//Debug.Log(t_hit.parent.name);
				if(Input.GetMouseButtonDown(0)){
					Debug.Log(t_hit.parent.name);
					mode = ControlMode.ZOOM_OUT_MODE;
					Services.AudioManager.PlaySFX (Services.AudioManager.exitRoomClip, 0.5f);
					InitZoomOutMode();

				}
			}

		}

		//fps control
		//rigidbodyfirstperson.cs

		//change to  furniture mode after click
		//InsideVisorMan.cs will do that
	}

	void ZoomInUpdate(){

		if(UpperCamera.fieldOfView > ZoomInUpperCameraFoV){
			UpperCamera.fieldOfView --;
		}

		if(m_Camera.fieldOfView > ZoomInMainCameraFoV){
			m_Camera.fieldOfView --;
			//Debug.Log("my camera fov:"+myCamera.fieldOfView);
		}

		if(Input.GetKeyDown(KeyCode.Tab)){
			//switch to zoom out mode
			InitZoomOutMode();
			mode = ControlMode.ZOOM_OUT_MODE;
		}
//		else if (Input.GetMouseButtonDown(0)){
//			//left click to equip/drop
//			//D_function will do it
//		}
//		else if(Input.GetMouseButtonDown(1)){
//			//right click to use equipped object
//			//(if there is no object equipped, then nothing happens.)
			//D_function will do it
//		}

	}

	void ZoomOutUpdate(){

		if(UpperCamera.fieldOfView < ZoomOutUpperCameraFoV){
			UpperCamera.fieldOfView ++;
			//Debug.Log("upper camera fov:"+uppercamera.fieldOfView);

		}
		if(m_Camera.fieldOfView < ZoomOutMainCameraFoV){
			m_Camera.fieldOfView ++;
			//Debug.Log("my camera fov:"+myCamera.fieldOfView);
		}
		//PickUpHandler();

		if(Input.GetMouseButtonDown(0)){
			//pick up or drop
			//if player is picking up an object then drop, else pick up
			//if(player is picking up an object){
			//	Drop();
			//}else{
			//	Check the object is carriable if(yes), pickup


		}else if(Input.GetMouseButtonDown(1)){
			//use clicked object and also the equipped object at the same time
			//D_function will do using equipped object
			//so here jus for use clicked object

			UseHandler();

		}else if(Input.GetKeyDown(KeyCode.Tab)){
			//switch to zoom in mode
			mode = ControlMode.ZOOM_IN_MODE;
			InitZoomInMode();

		}else if(Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.D)){
			mode = ControlMode.IN_ROOM_MODE;
			InitInRoomMode();
		}


	}

	Transform CameraRayCast(Camera camera){
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		Debug.DrawRay(ray.origin,ray.direction);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit) && !hit.collider.tag.Equals("Visor")){


			if(!hit.collider.name.Equals("ground")){

				return hit.transform;
			}
			//return null;

		}
		return null;
	}

	bool CheckAbility(InteractionSettings ability, bool isCheckUsable){
		if (ability != null) {
			if (isCheckUsable) {
				return ability.usable;

			}else{
				return ability.ableToBeCarried;

			}
		}
		return false;
	}

	void PickUpHandler(){
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

					UpdateDraggedObjectPosition();
					DetectPlacing();
					break;
				}
			}

		}

	}
	void UpdateDraggedObjectPosition( ){

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
			selectedObject.position = ray.GetPoint (rayDistance);
		}

	}

	void DetectPlacing(){
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
		Services.AudioManager.PlaySFX (throwClip, sfxVolume);

		Transform carriedObject = selectedObject;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		carriedObject.position = Camera.main.transform.position + ray.direction*5f;

		carriedObject.SetParent(carriedObject.GetComponentInChildren<InteractionSettings>().originalParent, true);

		carriedObject.GetComponent<Rigidbody> ().AddExplosionForce (throwForce*5, Camera.main.transform.position, 10f);

		selectedObject = null;
	}

	//detect if mouse click on something, then switch and save the selected object
	void DetectSelection(){

		//get the ray to check whether player points at visor from upper camera
		Ray ray = UpperCamera.ScreenPointToRay(Input.mousePosition);
		Debug.DrawRay(ray.origin,ray.direction);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit) && !hit.collider.tag.Equals("Visor")){


			if(!hit.collider.name.Equals("ground")){
				//txtInfo.text = "YOu ";
				selectedObject = hit.collider.transform;
				CheckPickUp();
			}
			return;

		}
		//detect if click on things outside visor ray from main camera
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {

			if(!hit.collider.name.Equals("ground")){
				selectedObject = hit.collider.transform;
				CheckPickUp();
			}
		}

	}

	//check first then pick up
	void CheckPickUp(){
		InteractionSettings intSet = selectedObject.GetComponentInChildren<InteractionSettings>();

		bool check =  CheckAbility(intSet,false);
		//Debug.Log("Check "+pickedUpObject.name+" can be carried:"+check);
		//check if click on something selectable
		if(check){
			GetComponent<Image> ().color = new Color(1,1,1,1);
			txtInfo.text = selectedObject.name+" waits to be picked up.";
			if(Input.GetMouseButtonDown(0)){
				clickGapCount = 0;
				// = pickedUpObject;
				state = InterationState.DRAG_STATE;
				PickUpObject();
				intSet.carryingObject = Services.Player.transform;
				Debug.Log (intSet.carryingObject);
			}
		}else{
			//GetComponent<Image> ().color = new Color(1,0,0,0.5f);
			txtInfo.text = selectedObject.name+" refuses to be picked up.";
		}
	}

	//do the transfer actions about the object
	void PickUpObject(){
		Transform pickedUpObject = selectedObject;
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
		UpdateDraggedObjectPosition();

		Services.AudioManager.PlaySFX (pickupClip, sfxVolume);


	}

	void PlaceHandler(){

	}

	void DropHandler(){//throw object


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
					//txtInfo.text = "Want to enter your room?";
					if(mode != ControlMode.IN_ROOM_MODE && Input.GetMouseButtonDown(0)){
						Debug.Log("click on player visor");

					}
				}else{
					txtInfo.text = "This thing can't be picked up.";
					isCollidingSomethingInVisor = true;

					InteractionSettings interactionSettings = hit.transform.GetComponentInChildren<InteractionSettings> ();
					if (CheckAbility(interactionSettings,true)) {

						GetComponent<Image> ().color = new Color(1,1,1,1);
						txtInfo.text = hit.collider.name + " is useful.";
						if(Input.GetMouseButtonDown(0)){
							clickGapCount = 0;
							Services.AudioManager.PlaySFX (pickupClip, sfxVolume);

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
					if (CheckAbility(interactionSettings,true)) {
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
}
