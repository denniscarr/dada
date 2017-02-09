using UnityEngine;
using System.Collections;

public class PointController : MonoBehaviour {
	
	bool isCarrying;
	private Transform carriedObject;	// The transform of the carried object

	public float interactionRange = 5f;	// How close an object needs to be for the player to interact with it.
	public float throwForce = 100f;

	Vector3 carriedObjectSavedScale;


	void Start () {
		isCarrying = false;
	}


	void Update () {

		// Get all the objects intercepting a ray from the camera to the cursor's position.
		RaycastHit[] raycastHits = Physics.RaycastAll (Camera.main.ScreenPointToRay(Input.mousePosition), interactionRange);

		// Iterate through those objects.
		foreach( RaycastHit hit in raycastHits)
		{
			// Update cursor object's position.
			if (hit.collider.name.Equals("Player Wall Rear")) {
				transform.position = hit.point;
			}

			// If the player is carrying an object, move the object to the cursor's position.
			if (isCarrying) {
				carriedObject.localPosition = transform.localPosition;
			}

			/* OBJECT INTERACTION */
			if (Input.GetMouseButtonDown (0))
			{
				if (isCarrying) {
					if (hit.collider.name == "Visor") {
						PlaceObjectInVisor ();
					}
					ThrowObject ();
				} else {
					print ("Clicked on " + hit.transform.name);

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
		}
	}


	void PlaceObjectInVisor() {
	}


	void PickUpObject(Transform t)
	{
		Debug.Log("Player picked up "+ t.name);

		isCarrying = true;

		carriedObject = t;

		carriedObjectSavedScale = carriedObject.localScale;
		Vector3 newScale = carriedObjectSavedScale * 0.2f;
		t.localScale = newScale;

		t.SetParent(transform.parent);

		// Disable any colliders on the picked up object
		Collider[] colliders = t.GetComponentsInChildren<Collider> ();
		foreach (Collider c in colliders) {
			c.enabled = false;
//			c.isTrigger = true;
		}

		t.GetComponentInChildren<Rigidbody> ().isKinematic = true;

//		Physics.IgnoreCollision(transform.parent.GetComponent<Collider>(), hit2.collider);
		//t.localRotation = Quaternion.Euler(45f,0f,45f);
		t.localPosition = transform.localPosition;//transform.parent.Find("Cylinder").localPosition+new Vector3(0,0.015f,0);
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
