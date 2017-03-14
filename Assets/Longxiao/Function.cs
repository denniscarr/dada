using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Function : MonoBehaviour {
	public KeyCode shootKey = KeyCode.Mouse0;
	public GameObject projectile;
	public float muzzleVelocity = 10000f; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find("Modern Russian AR").GetComponent<equipA2>().equipped == true && Input.GetKey(shootKey)) {
			GameObject shoot = Instantiate (projectile, transform.position, Quaternion.identity)
				as GameObject;
			shoot.GetComponent<Rigidbody> ().AddForce (GameObject.Find("Modern Russian AR").transform.right * muzzleVelocity);
		}
	}
}
