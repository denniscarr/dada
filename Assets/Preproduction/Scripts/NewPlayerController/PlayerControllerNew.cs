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
	public ControlMode mode;

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
	//float pressGapCount;

	CS_PlaySFX playSFXScript;

	//INSTRUCTIONS TEXT do not need any more
	public Text txtInfo;
    Writer writer;
    Vector3 textPosition = new Vector3(18.049f, 189.5f, 32.3271f);

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
		//Debug.Log("init");
		initCameraRotation = UpperCamera.transform.rotation;
		inPointForPlaneFromCube = cubeOnDraggedPlane.position;

        writer = GameObject.Find("Mouse").GetComponent<Writer>();

		//pressGapCount = 0f;
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

	public void InitZoomInMode(){
        //Debug.Log("reset");
        writer.DeleteTextBox();
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

		if(t_hit && t_hit.parent){
			//Debug.Log(t_hit.name);
			if(t_hit.parent.name.Equals("Viewing Platform")){
                //txtInfo.text = "Platform is calling you...";
                //Debug.Log(t_hit.parent.name);
                if (Input.GetMouseButtonDown(0)){
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
            //txtInfo.text = "Switch to zoom out mode";
            //switch to zoom out mode
            writer.DeleteTextBox();
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

	public void ZoomOutUpdate(){

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

		}else if(Input.GetKeyDown(KeyCode.Tab)){
			//txtInfo.text = "Zoom in";

			//switch to zoom in mode
			mode = ControlMode.ZOOM_IN_MODE;
			InitZoomInMode();

		}else if(Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.D)){
            writer.DeleteTextBox();
            writer.WriteAtPoint("Welcome home.", textPosition);
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
		
}
