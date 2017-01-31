using UnityEngine;
using System.Collections;

public class ViewController : MonoBehaviour {
	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;
	//float _translation = 0f;
	void Update() {
		float translation = Input.GetAxis("Vertical") * speed;
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		translation *= Time.deltaTime;
		rotation *= Time.deltaTime;
		//transform.Translate(-translation*transform.forward);
		transform.Translate(0, 0, -translation);
		transform.Rotate(0, rotation, 0);
		//_translation = translation;
	}

//	void OnTriggerEnter(Collider other) {
//		Debug.Log(other.name);
//		transform.Translate(0, 0, _translation);
//	}
}
