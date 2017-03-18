using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public enum ControlMode{
	ZOOM_IN_MODE = -1,
	ZOOM_OUT_MODE = 1,//can see inventory
	IN_ROOM_MODE = 0,
}

public class PlayerController : MonoBehaviour {
	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;

	ControlMode mode;
	private Transform uppernode;
	Camera uppercamera;
	GameObject canvas;
	FirstPersonController fpController;
	//count the time between pickup and place,prevent from vaild click repeatly in a second
	float pressGapCount;

	CS_PlaySFX playSFXScript;

	Transform inRoomNode;

	//count the time enter the front pic
	//float enterTimeCount;

	// Use this for initialization
	void Start () {
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

		if(mode == ControlMode.ZOOM_IN_MODE){
			canvas.SetActive(false);
			inRoomNode.gameObject.SetActive(false);
		}else if(mode == ControlMode.ZOOM_OUT_MODE){
			fpController.enabled = false;
			inRoomNode.gameObject.SetActive(false);
		}else if(mode == ControlMode.IN_ROOM_MODE){
			fpController.enabled = false;
			uppercamera.enabled = false;
			canvas.SetActive(false);
			gameObject.SetActive(false);

			inRoomNode.gameObject.SetActive(true);
		}
			
		playSFXScript = this.GetComponent<CS_PlaySFX> ();

	}

	void OnEnable(){
		Start();
	}

	void Update() {

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
				fpController.enabled = true;
				//Debug.Log(transform.position);
					
			}else{
				//zoom in mood -> zoom out
				canvas.SetActive(true);
				fpController.enabled = false;
				Cursor.lockState = CursorLockMode.None;

				//reset the main camera to be parallel to the plane
				Camera.main.transform.localEulerAngles = Vector3.zero;//.SetLookRotation(forward,Vector3.up);
				//update upper node rotation with the player
				uppernode.localRotation = transform.localRotation;
			}
		}

		switch(mode){
		case ControlMode.ZOOM_IN_MODE:ZoomInMove();;break;
		case ControlMode.ZOOM_OUT_MODE:ZoomOutMove();break;
		case ControlMode.IN_ROOM_MODE:InRoomMove();break;
		}
		//Debug.Log(transform.position);
	}

	void InRoomMove(){
	}



	public ControlMode getControlMode(){
		return mode;
	}

	public bool isInRoomMode(){
		if(mode == ControlMode.IN_ROOM_MODE){
			return true;
		}
		return false;
	}

	void ChangeToInRoomMode(){
		mode = ControlMode.IN_ROOM_MODE;
		inRoomNode.FindChild("CameraForScreen").position = Camera.main.transform.position;
		inRoomNode.FindChild("CameraForScreen").rotation = Camera.main.transform.rotation;
		fpController.enabled = false;
		uppercamera.enabled = false;
		canvas.SetActive(false);
		//gameObject.SetActive(false);
		inRoomNode.gameObject.SetActive(true);//player in room enable
		gameObject.SetActive(false);

	}

	void InRoomChangeToZoomOut(){
		Debug.Log("in room -> zoom out");
		uppercamera.enabled = true;
		canvas.SetActive(true);

	}

	void ZoomOutMove(){
		//using W & S to go forward and backward, A & D to rotate left and right
		if(uppercamera.fieldOfView < 16f){
			uppercamera.fieldOfView ++;
		}
		float translation = Input.GetAxis("Vertical") * speed;
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		translation *= Time.deltaTime;
		rotation *= Time.deltaTime;
		transform.Translate(0, 0, translation);
		//update uppercamera simultaneously
		uppernode.Translate(0, 0, translation);
		transform.Rotate(0, rotation, 0);
		uppernode.Rotate(0, rotation, 0);


	}

	void ZoomInMove(){
		//follow mouse
		if(uppercamera.fieldOfView > 10f){
			uppercamera.fieldOfView --;
		}
	}

}
