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

public class PlayerController : MonoBehaviour {
	private float speed = 1.0F;
	private float rotationSpeed = 100.0F;

	public float ZoomInUpperCameraFoV = 10f;
	public float ZoomOutUpperCameraFoV = 16f;
	public float ZoomInMainCameraFoV = 33f;
	public float ZoomOutMainCameraFoV = 45f;

	private List<GameObject> equipTips;
	ControlMode mode;
	private Transform uppernode;
	Camera uppercamera;
	GameObject canvas;
	FirstPersonController fpController;
	//count the time between pickup and place,prevent from vaild click repeatly in a second
	float pressGapCount;

	CS_PlaySFX playSFXScript;

	Transform inRoomNode;
	Transform t_gun;

	Camera myCamera;
	//count the time enter the front pic
	//float enterTimeCount;


	//INSTRUCTIONS TEXT
	Text instructionText;

	// Use this for initialization
	void Start () {
		//equipTips = 
		//t_gun = transform.GetChild(0).GetChild(0);
		//t_gun.gameObject.SetActive(false);
		//equipTips = new List<GameObject>();
		//enterTimeCount = 0;
		Cursor.lockState = CursorLockMode.None;
		mode = ControlMode.ZOOM_OUT_MODE;
		fpController = this.GetComponent<FirstPersonController>();
		Transform root = transform.parent;
		canvas = root.FindChild("Canvas").gameObject;
		uppernode = root.FindChild("UpperNode");
		inRoomNode = uppernode.FindChild("InRoomNode");
		pressGapCount = 0f;
		uppercamera = uppernode.FindChild("UpperCamera").GetComponent<Camera>();
		myCamera = transform.GetChild(0).GetComponent<Camera>();


		if(mode == ControlMode.ZOOM_IN_MODE){
			canvas.SetActive(false);

			//Debug.Log("my camera fov:"+myCamera.fieldOfView);
			//Debug.Log("upper camera fov:"+uppercamera.fieldOfView);
			myCamera.fieldOfView = ZoomInMainCameraFoV;
			uppercamera.fieldOfView = ZoomInUpperCameraFoV;
			inRoomNode.gameObject.SetActive(false);
		}else if(mode == ControlMode.ZOOM_OUT_MODE){
			fpController.isFPSMode = false;
			inRoomNode.gameObject.SetActive(false);
			//Debug.Log("my camera fov:"+myCamera.fieldOfView);
			//Debug.Log("upper camera fov:"+uppercamera.fieldOfView);
			myCamera.fieldOfView = ZoomOutMainCameraFoV;
			uppercamera.fieldOfView = ZoomOutUpperCameraFoV;
		}else if(mode == ControlMode.IN_ROOM_MODE){
			
			ChangeToInRoomMode();
		}
			
		playSFXScript = this.GetComponent<CS_PlaySFX> ();

		instructionText = canvas.transform.FindChild ("Instructions").GetComponent<Text> ();;

	}
		
	void OnEnable(){
		Debug.Log("player enable");
		//Start();
	}

	void Update() {
		//disable collider to fall down
		if(Input.GetKeyDown(KeyCode.RightShift)){
			GetComponent<CharacterController>().enabled = ! GetComponent<CharacterController>().enabled;
		}

		//if press tab, mode change to another
		pressGapCount += Time.deltaTime;

		if(pressGapCount > 0.1f && Input.GetKeyDown(KeyCode.Tab)){
			
			mode = mode == ControlMode.ZOOM_IN_MODE? ControlMode.ZOOM_OUT_MODE:ControlMode.ZOOM_IN_MODE;
			Debug.Log("mode change to:"+ mode);
			//Debug.Log(transform.position);
			//play sound effect for mode switch
			playSFXScript.PlaySFX (1);


			if(canvas.activeInHierarchy == true){
				//zoom out -> zoom in
				//conceal canvas and change to fps control
				canvas.SetActive(false);
				fpController.isFPSMode = true;
					
			}else{
				//zoom in mood -> zoom out
				canvas.SetActive(true);
				fpController.isFPSMode = false;
				Cursor.lockState = CursorLockMode.None;

				//reset the main camera to be parallel to the plane

				//Camera.main.transform.localEulerAngles = Vector3.zero;//.SetLookRotation(forward,Vector3.up);
				//update upper node rotation with the player
				//uppernode.localRotation = transform.localRotation;
			}
		}

		switch(mode){
		case ControlMode.ZOOM_IN_MODE:ZoomInMove();;break;
		case ControlMode.ZOOM_OUT_MODE:ZoomOutMove();break;
		case ControlMode.IN_ROOM_MODE:InRoomMove();break;
		}

		if (Input.GetKeyDown (KeyCode.C)) {
			ToggleInstructions ();
		}

		//Debug.Log(transform.position);
	}

	void InRoomMove(){
	}

	public ControlMode controlMode{
		get
		{
			//Some other code
			return mode;
		}
		set
		{
			//Some other code
			mode = value;
		}
	}


	void ChangeToInRoomMode(){
		myCamera.fieldOfView = ZoomOutMainCameraFoV;
		uppercamera.fieldOfView = ZoomOutUpperCameraFoV;
		Debug.Log("my camera fov:"+myCamera.fieldOfView);
		Debug.Log("upper camera fov:"+uppercamera.fieldOfView);
		mode = ControlMode.IN_ROOM_MODE;
		Debug.Log(mode);
		inRoomNode.FindChild("CameraForScreen").position = Camera.main.transform.position;
		inRoomNode.FindChild("CameraForScreen").rotation = Camera.main.transform.rotation;

		//?????????
		uppercamera.enabled = false;
		canvas.SetActive(false);
		fpController.enabled = false;

		myCamera.gameObject.SetActive(false);
		inRoomNode.gameObject.SetActive(true);//player in room enable

	}

	void InRoomChangeToZoomOut(){
		Debug.Log("in room -> zoom out");
		mode = ControlMode.ZOOM_OUT_MODE;
		uppercamera.enabled = true;
		canvas.SetActive(true);
		fpController.enabled = true;
		myCamera.gameObject.SetActive(true);
		inRoomNode.gameObject.SetActive(false);
	}

	void ZoomOutMove(){
		//Debug.Log("Zoom out move");

		//using W & S to go forward and backward, A & D to rotate left and right
		if(uppercamera.fieldOfView < ZoomOutUpperCameraFoV){
			uppercamera.fieldOfView ++;
			//Debug.Log("upper camera fov:"+uppercamera.fieldOfView);

		}
		if(Camera.main.fieldOfView < ZoomOutMainCameraFoV){
			Camera.main.fieldOfView ++;
			//Debug.Log("my camera fov:"+myCamera.fieldOfView);
		}


			
		updateUpper();


	}

	void updateUpper(){
		
		uppernode.rotation = transform.rotation;
	}

	void ZoomInMove(){
		//Debug.Log("Zoom in move");

		//follow mouse
		if(uppercamera.fieldOfView > ZoomInUpperCameraFoV){
			//Debug.Log("upper camera fov:"+uppercamera.fieldOfView);
			//Debug.Log(uppercamera.fieldOfView);
			uppercamera.fieldOfView --;
		}

		if(Camera.main.fieldOfView > ZoomInMainCameraFoV){
			Camera.main.fieldOfView --;
			//Debug.Log("my camera fov:"+myCamera.fieldOfView);
		}


		//updateUpper();
	}

	void ToggleInstructions() {

		instructionText.enabled = !instructionText.enabled;

	}

}
