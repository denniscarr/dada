using UnityEngine;
using System.Collections;

public class PointController : MonoBehaviour {
	
	bool isCarrying;
	bool isMouseOnVisor;
	private Transform carriedObject;	// The transform of the carried object

	public float interactionRange = 5f;	// How close an object needs to be for the player to interact with it.
	public float throwForce = 100f;
	public Plane wallPlane;
	//public GameObject go;

	Vector3 carriedObjectSavedScale;


	void Start () {
		isCarrying = false;
		isMouseOnVisor = false;
		carriedObjectSavedScale = Vector3.zero;
		//create a plane that the mouse is on
		wallPlane.SetNormalAndPosition(transform.forward,transform.position);
	}


	void Update () {
		
		//get the ray
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		//update the mouse position when mouse ray intersects with the plane
		float rayDistance = 10;
		//update plane
		wallPlane.SetNormalAndPosition(transform.forward,transform.position);
		if (wallPlane.Raycast(ray,out rayDistance)){
			transform.position = ray.GetPoint(rayDistance);;
		}

		// If the player is carrying an object, move the object to the cursor's position.
		if (isCarrying) {
			carriedObject.localPosition = transform.localPosition;

		}

		RaycastHit hit;
		if(Physics.Raycast(ray,out hit)){
			/* OBJECT INTERACTION */
			if (Input.GetMouseButtonDown (0))
			{
				if (isCarrying) {
					if (hit.collider.name.Contains("Visor")) {
						PlaceObjectInVisor ();
					}else{
						//if collide with objects except Visor, when carry sth and click, throw object
						ThrowObject ();
					}
				} else {
					//print ("Clicked on " + hit.transform.name);
					//you can not pick up yourself
					if(hit.transform.name.Contains("Player")){
						return;
					}
					// Make sure this object can be interacted with.
					InteractionSettings interactionSettings = hit.transform.GetComponentInChildren<InteractionSettings> ();
					if (interactionSettings != null) {
						if (interactionSettings.ableToBeCarried) {
							PickUpObject (hit.transform);
						}

						if (interactionSettings.usable) {
							hit.collider.BroadcastMessage ("UsedByPlayer");
						}
					}
				}
			}

		}else{
			//if do not collider with any object(Visor), when carry sth and click, throw object
			if (isCarrying && Input.GetMouseButtonDown (0)){
				ThrowObject ();
			}
		}
			

	}


	void PlaceObjectInVisor() {
		Debug.Log("place in visor");
		isCarrying = false;

		// Reenable any colliders on the picked up object
		Collider[] colliders = carriedObject.GetComponentsInChildren<Collider> ();
		foreach (Collider c in colliders) {
			c.enabled = true;
		}

		carriedObject.GetComponent<Rigidbody> ().useGravity = true;
		carriedObject.GetComponent<Rigidbody> ().isKinematic = false;
	}


	void PickUpObject(Transform t)
	{
		Debug.Log("Player picked up "+ t.name);

		isCarrying = true;

		carriedObject = t;

		if(t.parent == transform.parent){

		}else{
			carriedObjectSavedScale = carriedObject.localScale;
			Vector3 newScale = carriedObjectSavedScale * 0.2f;
			t.localScale = newScale;
			t.SetParent(transform.parent);
			t.localPosition = transform.localPosition;
		}


		// Disable any colliders on the picked up object
		Collider[] colliders = t.GetComponentsInChildren<Collider> ();
		foreach (Collider c in colliders) {
			c.enabled = false;
//			c.isTrigger = true;
		}

		t.GetComponentInChildren<Rigidbody> ().isKinematic = true;

//		Physics.IgnoreCollision(transform.parent.GetComponent<Collider>(), hit2.collider);
		//t.localRotation = Quaternion.Euler(45f,0f,45f);
		//transform.parent.Find("Cylinder").localPosition+new Vector3(0,0.015f,0);
	}


	void ThrowObject() {
		
		Debug.Log("Player threw "+carriedObject);

		//Ray ray2 = new Ray(,)
		//t_carried = t;
		isCarrying = false;

		carriedObject.position -= (carriedObject.position - Camera.main.transform.position).normalized*-3f;
		carriedObject.transform.parent = null;

		// Reenable any colliders on the picked up object
		Collider[] colliders = carriedObject.GetComponentsInChildren<Collider> ();
		foreach (Collider c in colliders) {
			c.enabled = true;
		}

		carriedObject.GetComponent<Rigidbody> ().useGravity = true;
		carriedObject.GetComponent<Rigidbody> ().isKinematic = false;
		carriedObject.GetComponent<Rigidbody> ().AddExplosionForce (throwForce, Camera.main.transform.position, 10f);

		carriedObject.transform.localScale = carriedObjectSavedScale;
	}
}
