using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public enum ControlMode{
	ZOOM_IN_MODE = -1,
	ZOOM_OUT_MODE = 1,//can see inventory
}

public class PlayerController : MonoBehaviour {
	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;

	ControlMode mode{ get;set;}
	private Transform uppernode;
	Camera uppercamera;
	GameObject canvas;
	FirstPersonController fpController;
	//count the time between pickup and place,prevent from vaild click repeatly in a second
	float pressGapCount;

	CS_PlaySFX playSFXScript;

	// Use this for initialization
	void Start () {
		fpController = this.GetComponent<FirstPersonController>();
		canvas = GameObject.Find("Canvas");
		mode = ControlMode.ZOOM_IN_MODE;
		uppernode = GameObject.Find("UpperNode").transform;
		pressGapCount = 0f;
		uppercamera = uppernode.FindChild("UpperCamera").GetComponent<Camera>();
		if(mode == ControlMode.ZOOM_IN_MODE){
			canvas.SetActive(false);

		}else{
			fpController.enabled = false;
		}

<<<<<<< HEAD

=======
		playSFXScript = this.GetComponent<CS_PlaySFX> ();
>>>>>>> origin/master

	}


	void Update() {

		//Debug.Log(Globe.prefab);
		//if press tab, mode change to another
		pressGapCount += Time.deltaTime;

		if(pressGapCount > 0.1f && Input.GetKeyDown(KeyCode.Tab)){
			
			mode = mode == ControlMode.ZOOM_IN_MODE? ControlMode.ZOOM_OUT_MODE:ControlMode.ZOOM_IN_MODE;
			Debug.Log("mode change to:"+ mode);

			//play sound effect for mode switch
			playSFXScript.PlaySFX (1);

			//when it is zoom out mood,change to zoom in
			if(canvas.activeInHierarchy == true){
				//conceal canvas and change to fps control
				canvas.SetActive(false);
				fpController.enabled = true;
					
				Debug.Log(canvas.activeInHierarchy);
			}else{
				//when it is zoom in mood,change to zoom out
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

		}

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

	public float horizontalSpeed = 2.0F;
	public float verticalSpeed = 2.0F;

	void ZoomInMove(){
		//follow mouse
		if(uppercamera.fieldOfView > 10f){
			uppercamera.fieldOfView --;
		}
//
//		float h = horizontalSpeed * Input.GetAxis("Mouse X");
//		float v = verticalSpeed * Input.GetAxis("Mouse Y");
//
//		Transform cameraT = Camera.main.transform;
//		cameraT.Rotate(v, h, 0);
//
//		if (Input.GetKey(KeyCode.W))
//		{
//			transform.Translate(cameraT.forward * speed * Time.deltaTime);
//		}
//		else if (Input.GetKey(KeyCode.S))
//		{
//			transform.Translate(-cameraT.forward * speed * Time.deltaTime);
//		}
//		else if (Input.GetKey(KeyCode.A))
//		{
//			transform.Translate(-cameraT.right * speed * Time.deltaTime);
//		}
//		else if(Input.GetKey(KeyCode.D))
//		{
//			transform.Translate(cameraT.right * speed * Time.deltaTime);
//		}

	}

}
