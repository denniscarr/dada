using UnityEngine;
using System.Collections;

public class PointController : MonoBehaviour {
	bool isCarrying;
	private Transform t_carried;
	// Use this for initialization
	void Start () {
		isCarrying = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//Debug.Log(Input.mousePosition);
		RaycastHit hit;
		transform.position = ray.origin + ray.direction * 2f;
		if(Physics.Raycast(ray,out hit)){
			
			if(hit.collider.name.Contains("Cube")){
				
				if(Input.GetMouseButtonDown(0)){
					Transform t = hit.collider.transform;
					//Debug.Log(hit.collider.name);
					if(t.parent == transform.parent){
						Debug.Log("throw "+hit.collider.name);
						//Ray ray2 = new Ray(,)
						//t_carried = t;
						//isCarrying = true;

						hit.rigidbody.useGravity = true;
						//hit.collider.isTrigger = false;
						t.GetComponent<Rigidbody>().AddForce(-transform.forward*100);

					}else{
						Debug.Log("grap");
						t.SetParent(transform.parent);
						//t.GetComponent<BoxCollider>().enabled = false;
						//hit.collider.attachedRigidbody.
						hit.collider.isTrigger = true;
						hit.rigidbody.isKinematic = false;
						t.localScale = 0.01f*Vector3.one;
						//t.localRotation = Quaternion.Euler(45f,0f,45f);
						t.localPosition = transform.parent.Find("Cylinder").localPosition+new Vector3(0,0.015f,0);
					}
				}else if(isCarrying){
					//Transform t = hit.collider.transform;
//					Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//					temp.z = t_carried.position.z;//Set this to be the distance you want the object to be placed in front of the camera.
//					t_carried.position = temp;

				}

			}

		}
//		if (Physics.Raycast(ray,out hit)){
//			//Debug.Log(hit.collider.name);
//			if(hit.collider.name.Contains("Quad")){
//				transform.position = hit.point;
//				Ray ray2 = new Ray(hit.point,ray.direction);
//				RaycastHit hit2;
//				//transform.position = ray.origin+ray.direction*1.559f;
//				if (Physics.Raycast(ray2,out hit2)){
//					Debug.Log(hit2.collider.name);
//					if(hit2.collider.name.Contains("Cube")){
//						//transform.position = hit.point;
//						if(Input.GetMouseButtonDown(0)){
//
//						}
//					}
//				}
//			}
//		}

	}

}