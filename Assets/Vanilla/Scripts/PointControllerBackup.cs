using UnityEngine;
using System.Collections;

public class PointControllerBackup : MonoBehaviour {
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
		//transform.position = ray.origin + new Vector3();

//		if(Physics.Raycast(ray,out hit)){
//			
//			if(hit.collider.name.Contains("Cube")){
//				
//				if(Input.GetMouseButtonDown(0)){
//					Transform t = hit.collider.transform;
//					//Debug.Log(hit.collider.name);
//					if(t.parent == transform.parent){
//						Debug.Log("throw "+hit.collider.name);
//						//Ray ray2 = new Ray(,)
//						//t_carried = t;
//						//isCarrying = true;
//
//						hit.rigidbody.useGravity = true;
//						//hit.collider.isTrigger = false;
//						t.GetComponent<Rigidbody>().AddForce(-transform.forward*500);
//
//						hit.transform.SetParent(transform.root.parent);
//						hit.transform.localScale = Vector3.one;
//
//					}else{
//						Debug.Log("grap");
//						t.SetParent(transform.parent);
//						//t.GetComponent<BoxCollider>().enabled = false;
//						//hit.collider.attachedRigidbody.
//						//hit.collider.isTrigger = true;
//						hit.rigidbody.isKinematic = false;
//						t.localScale = 0.01f*Vector3.one;
//						//t.localRotation = Quaternion.Euler(45f,0f,45f);
//						t.localPosition = transform.parent.Find("Cylinder").localPosition+new Vector3(0,0.015f,0);
//					}
//				}
//
//			}
//
//		}
		if (Physics.Raycast(ray,out hit,Mathf.Infinity,4)){
			//Debug.Log(hit.collider.name);
			if(hit.collider.name.Equals("Wall")){
				transform.position = hit.point;
			}
		}

		if(isCarrying){

			t_carried.localPosition = transform.localPosition;
		}

		Debug.DrawRay(ray.origin,ray.direction);
		RaycastHit hit2;
		//default mask ignoreCast
		if (Physics.Raycast(ray,out hit2,Mathf.Infinity)){
			//Debug.Log("4： "+hit2.collider.name);
			if(hit2.collider.name.Contains("Cube")){
				//transform.position = hit.point;
				if(Input.GetMouseButtonDown(0)){
					Transform t = hit2.collider.transform;
					if(t.parent == transform.parent){
						Debug.Log("drag "+hit2.collider.name);

						//Ray ray2 = new Ray(,)
						//t_carried = t;
						isCarrying = false;

						t.localPosition -= (t.position - Camera.main.transform.position).normalized*0.1f;
						hit2.rigidbody.useGravity = true;
						//hit.collider.isTrigger = false;
						t.GetComponent<Rigidbody>().AddForce(-transform.forward*500);

						hit2.transform.SetParent(transform.root.parent);
						hit2.transform.localScale = Vector3.one;

					}else{
						Debug.Log("grap");
						isCarrying = true;
						t.SetParent(transform.parent);
						t_carried = t;
						//t.GetComponent<BoxCollider>().enabled = false;
						//hit.collider.attachedRigidbody.
						//hit.collider.isTrigger = true;
						hit2.rigidbody.isKinematic = false;
						t.localScale = 0.01f*Vector3.one;
						Physics.IgnoreCollision(transform.parent.GetComponent<Collider>(), hit2.collider);
						//t.localRotation = Quaternion.Euler(45f,0f,45f);
						t.localPosition = transform.localPosition;//transform.parent.Find("Cylinder").localPosition+new Vector3(0,0.015f,0);
					}
				}
			}
		}

	}

}