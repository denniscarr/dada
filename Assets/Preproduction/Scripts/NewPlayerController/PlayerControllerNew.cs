using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public enum ControlMode{
	ZOOM_IN_MODE = -1,
	ZOOM_OUT_MODE = 1,//can see inventory
	IN_ROOM_MODE = 0,
}


public class PlayerControllerNew : MonoBehaviour {
	ControlMode mode;

	FirstPersonController fpController;//fps controller

	public Transform CameraForScreen;//camera at upper node for screen display at in-visor mode
	public Camera m_Camera;//camera under the player
	public Camera UpperCamera;//camera upper

	//private Transform uppernode;
	public Transform inRoomNode;//upper node is the parent of inroomnode

	//fov settings
	public float ZoomInUpperCameraFoV = 17f;
	public float ZoomOutUpperCameraFoV = 33f;
	public float ZoomInMainCameraFoV = 60f;
	public float ZoomOutMainCameraFoV = 80f;

	//count the time between pickup and place,prevent from vaild click repeatly in a second
	float pressGapCount;

	CS_PlaySFX playSFXScript;

	// Use this for initialization
	void Start () {
//		CameraForScreen.position = transform.position;
//		CameraForScreen.rotation = transform.rotation;


		mode = ControlMode.ZOOM_IN_MODE;
		Cursor.lockState = CursorLockMode.None;
		fpController = this.GetComponent<FirstPersonController>();

		switch(mode){
		case ControlMode.IN_ROOM_MODE:InitInRoomMode();break;
		case ControlMode.ZOOM_IN_MODE:InitZoomInMode();break;
		case ControlMode.ZOOM_OUT_MODE:InitZoomOutMode();break;
		}
	}

	void InitInRoomMode(){
		

	}

	void InitZoomInMode(){
		//change fov

		//change control way
		fpController.isFPSMode = true;
	}

	void InitZoomOutMode(){
		//change fov

		//change control way
		fpController.isFPSMode = false;
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
		//fps control


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
			//switch to zoom out mode
			InitZoomInMode();
			mode = ControlMode.ZOOM_IN_MODE;
		}


	}

	bool CheckAbility(InteractionSettings ability){
		//1.ray cast to get the object
		//2.check ability on the object and return true or false
		return false;
	}

	void PickUpHandler(){


	}

	void PlaceHandler(){

	}

	void DropHandler(){//throw object


	}

	void UseHandler(){
		//1.check ray cast hit something is an object not environment 
		//2.check the usability of the object
		//3.use the object

	}
}
