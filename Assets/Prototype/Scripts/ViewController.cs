using UnityEngine;
using System.Collections;

public class ViewController : MonoBehaviour {
	
	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;
	bool isStopped = false;
	float _translation = 0f;

	void Update() {
		float translation = Input.GetAxis("Vertical") * speed;
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		translation *= Time.deltaTime;
		rotation *= Time.deltaTime;
		transform.root.Translate(0, 0, -translation);
		transform.root.Rotate(0, rotation, 0);
		_translation = translation;
	}

	void OnCollisionEnter(Collision other){
		Debug.Log(other.collider.name+" enter");
		if(other.collider.name.Contains("Cube")){
			//isStop = true;
		}
	}

	void OnCollisionExit(Collision other){
		Debug.Log(other.collider.name+" exit");
		if(other.collider.name.Contains("Cube")){
			//isStop = false;
		}

	}
//	void OnTriggerEnter(Collider other) {
//		Debug.Log(other.name);
//		transform.Translate(0, 0, _translation);
//	}
}
