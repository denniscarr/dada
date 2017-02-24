using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlMode{
	ZOOM_IN_MODE = 0,
	ZOOM_OUT_MODE = 1,//can see inventory
}

public class PlayerController : MonoBehaviour {
	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;
	float _translation = 0f;
	ControlMode mode{ get;set;}
	private Transform uppernode;
	// Use this for initialization
	void Start () {
		mode = ControlMode.ZOOM_OUT_MODE;
		uppernode = GameObject.Find("UpperNode").transform;
	}


	void Update() {
		switch(mode){
		case ControlMode.ZOOM_IN_MODE:ZoomInMove();;break;
		case ControlMode.ZOOM_OUT_MODE:ZoomOutMove();break;

		}

	}

	void ZoomOutMove(){
		//using W & S to go forward and backward, A & D to rotate left and right
		float translation = Input.GetAxis("Vertical") * speed;
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		translation *= Time.deltaTime;
		rotation *= Time.deltaTime;
		transform.Translate(0, 0, -translation);
		//update uppercamera simultaneously
		uppernode.Translate(0, 0, -translation);
		transform.Rotate(0, rotation, 0);
		uppernode.Rotate(0, rotation, 0);
		_translation = translation;

	}

	void ZoomInMove(){

	}

}
