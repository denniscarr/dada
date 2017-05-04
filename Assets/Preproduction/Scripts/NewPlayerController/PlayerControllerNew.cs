using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using DG.Tweening;

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
	private ControlMode mode;

	public ControlMode Mode{
		get{
			return mode;
		}
		set{
			mode = value;
			switch(mode){
			case ControlMode.IN_ROOM_MODE:InitInRoomMode();break;
			case ControlMode.ZOOM_IN_MODE:InitZoomInMode();break;
			case ControlMode.ZOOM_OUT_MODE:InitZoomOutMode();break;
			}
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

	//private float clickGapCount;
	private const float CLICKGAPTIME = 0.3f;

	public AudioClip pickupClip;
	public AudioClip throwClip;
	public AudioClip toggleViewSFX;
	public float sfxVolume = 0.1f;

	private InterationState state;
	private Transform selectedObject;

	private Plane draggedPlane;
	public Transform cubeOnDraggedPlane;
	//private Vector3 inPointForPlaneFromCube;


	// Use this for initialization
	void Start () {
		initPos = transform.position;
		initRotation = transform.rotation;
		//Debug.Log("init");
		initCameraRotation = UpperCamera.transform.rotation;
		//inPointForPlaneFromCube = cubeOnDraggedPlane.position;

        writer = GameObject.Find("Mouse").GetComponent<Writer>();

		//pressGapCount = 0f;
		//clickGapCount = 0f;
		//fpController = this.GetComponent<FirstPersonController>();

		mode = ControlMode.ZOOM_IN_MODE;
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
		m_Camera.DOFieldOfView(ZoomOutMainCameraFoV,1.5f);
		UpperCamera.DOFieldOfView(ZoomOutUpperCameraFoV,1.5f);
//		m_Camera.fieldOfView = ZoomOutMainCameraFoV;
//		UpperCamera.fieldOfView = ZoomOutUpperCameraFoV;

		Debug.Log(mode);

		Services.AudioManager.PlaySFX (Services.AudioManager.enterRoomClip, 0.2f);
		//Debug.Log ("playing fucking sfx");

	}

	public void InitZoomInMode(){
		m_Camera.DOFieldOfView(ZoomInMainCameraFoV,1.5f);
		UpperCamera.DOFieldOfView(ZoomInUpperCameraFoV,1.5f);
        //Debug.Log("reset");
        writer.DeleteTextBox();
		transform.DOMove(initPos,1.0f);
		transform.DORotateQuaternion(initRotation,1.0f);

		rigidbodyFirstPersonController.enabled = false;
		insideVisorMan.enabled = false;
		headBob.enabled = false;

		Services.AudioManager.PlaySFX (Services.AudioManager.exitRoomClip, 0.2f);

	}

	public void InitZoomOutMode(){
		Debug.Log("zoom out");
		m_Camera.DOFieldOfView(ZoomOutMainCameraFoV,1.5f);
		UpperCamera.DOFieldOfView(ZoomOutUpperCameraFoV,1.5f);

		transform.DOMove(initPos,1.0f);
		transform.DORotateQuaternion(initRotation,1.0f);
		UpperCamera.transform.DORotateQuaternion(initCameraRotation,1.0f);
//		transform.position = initPos;
//		transform.rotation = initRotation;
//		UpperCamera.transform.rotation = initCameraRotation;

		Cursor.lockState = CursorLockMode.None;
		rigidbodyFirstPersonController.enabled = false;
		insideVisorMan.enabled = false;
		headBob.enabled = false;


		Services.AudioManager.PlaySFX (Services.AudioManager.toggleVisor, 0.7f);
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
					Services.AudioManager.PlaySFX (Services.AudioManager.exitRoomClip, 0.2f);
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

		if(Input.GetMouseButtonDown(0)){
			//pick up or drop
			//if player is picking up an object then drop, else pick up
			//mousenew will do that

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
