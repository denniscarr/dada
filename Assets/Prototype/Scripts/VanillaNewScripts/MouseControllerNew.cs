using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum InteractionMode{
	GRAB_MODE = 0,
	USE_MODE = 1,
//	INROOM_MODE = 2,
}

public class MouseControllerNew : MonoBehaviour {

	public float throwForce = 100f;
	private Camera UpperCamera;

	//InterationState state;
	InteractionMode mode;
	//store the selected object, which will be null after deselecting
	Transform selectedObject;

	//inpoint get from the reference cube for dragging plane, which is visible at the scene but hide later
	Plane draggedPlane;
	public Transform cubeOnDraggedPlane;
	public Transform t_INROOMOBJECTS;
	public PlayerControllerNew playercontroller;
	Vector3 inPointForPlaneFromCube;

	//count the time between pickup and place,prevent from vaild click repeatly in a second
	float clickGapCount;

	//For Sound Effects
	//CS_PlaySFX sfxScript;
	public AudioClip pickupClip;
	public AudioClip throwClip;
	public float sfxVolume = 0.1f;

	public Text txtInfo;

	float CLICKGAPTIME = 0.3f;

	// Use this for initialization
	void Start () {
		UpperCamera = playercontroller.UpperCamera;
		//txtInfo = transform.parent.FindChild("txtInfo").GetComponent<Text>();
		clickGapCount = 0;

		//state = InterationState.NONE_SELECTED_STATE;
		//UpperCamera = GameObject.Find("UpperCamera").GetComponent<Camera>();
		Cursor.visible = false;
		//cubeOnDraggedPlane = GameObject.Find("CubeOnSelectedPlane").transform;

		selectedObject = null;

		inPointForPlaneFromCube = cubeOnDraggedPlane.position;
		draggedPlane.SetNormalAndPosition(
			UpperCamera.transform.forward,
			inPointForPlaneFromCube);
		cubeOnDraggedPlane.gameObject.SetActive(false);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(playercontroller.Mode == ControlMode.ZOOM_OUT_MODE){
			transform.position = Input.mousePosition;
			
		}else{
			GetComponent<RectTransform>().localPosition = Vector3.zero;
		}

		clickGapCount += Time.fixedDeltaTime;
		if(clickGapCount > CLICKGAPTIME){
			DetectSelection();
		}
			
	}

	//detect if mouse click on something, then switch and save the selected object
	void DetectSelection(){
		//get the ray to check whether player points at visor from upper camera
		if(selectedObject){
			Debug.Log("select "+selectedObject.name);
			UpdateDraggedObjectPosition(selectedObject);
			DetectPlacing(selectedObject);
		}else{
			Ray ray = UpperCamera.ScreenPointToRay(Input.mousePosition);
			//Debug.DrawRay(ray.origin,ray.direction);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)){
				CheckPointedObject(hit.transform);
				return;
			}
			//detect if click on things outside visor ray from main camera
			ray = playercontroller.m_Camera.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay(ray.origin,ray.direction);
			if (Physics.Raycast (ray, out hit)) {
				CheckPointedObject(hit.transform);
			}
		}
	}

	void CheckPointedObject(Transform pointedObject){
		InteractionSettings inSets = pointedObject.GetComponentInChildren<InteractionSettings>();
		if(Input.GetMouseButtonDown(0)){
			if(CheckAbility(inSets,false)){
				GetComponent<Image> ().color = new Color(1,1,1,1);
				if(playercontroller.Mode == ControlMode.ZOOM_IN_MODE){
					//equip.cs
				}else if(playercontroller.Mode == ControlMode.ZOOM_OUT_MODE){
					txtInfo.text = "pick up "+pointedObject.name;
					PickUpObject(pointedObject);
				}else{
					//in visor man.cs
				}
			}else if(!pointedObject.name.Equals("GROUND")){
				txtInfo.text = pointedObject.name + "refuses to be picked up.";
				GetComponent<Image> ().color = new Color(1,0,0,0.5f);
				//give cannot feedback
			}

		}
		else if(Input.GetMouseButtonDown(1)){
			if(CheckAbility(inSets,true)){
				GetComponent<Image> ().color = new Color(1,1,1,1);
				//use object
				txtInfo.text = "use "+pointedObject.name;
				//UseHandler();
			}else{
				//give cannot feedback
				txtInfo.text = pointedObject.name + " refuses to be used.";
				GetComponent<Image> ().color = new Color(1,0,0,0.5f);
			}

		}else{
			GetComponent<Image> ().color = new Color(1,1,1,1);
			//if(!pointedObject.name.Equals("GROUND")){
				//txtInfo.text = "Your mouse is over "+pointedObject.name;	
			//}

		}
	}

	bool CheckAbility(InteractionSettings ability, bool isCheckUsable){
		if (ability != null) {
			if (isCheckUsable) {
				return ability.usable;

			}else{
				return ability.ableToBeCarried;

			}
		}
		return false;
	}
		

	//do the transfer actions about the object
	void PickUpObject(Transform pickedUpObject){
		
		if(pickedUpObject.parent != t_INROOMOBJECTS){
			//change the parent of selected object
			pickedUpObject.SetParent(t_INROOMOBJECTS);

			//change scale
			float distanceInside = Mathf.Abs(
				Vector3.Dot((inPointForPlaneFromCube - UpperCamera.transform.position),
					UpperCamera.transform.forward));
			float distance = Mathf.Abs(
				Vector3.Dot((pickedUpObject.position - playercontroller.m_Camera.transform.position),
					playercontroller.m_Camera.transform.forward));
			if(distance < 0){
				Debug.Log("error");
			}
			float frustumHeightInside = distanceInside * Mathf.Tan(UpperCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
			float frustumHeight = distance * Mathf.Tan(playercontroller.m_Camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
			float scale = frustumHeightInside/frustumHeight;
			pickedUpObject.localScale *= scale;

			//stop gravity simulation and free rotation
			Debug.Log("pick up "+pickedUpObject.name+" outside visor");

		}else{

			Debug.Log("pick up "+pickedUpObject.name+" inside visor");
		}

		//pickedUpObject.localScale = 
		Rigidbody body = pickedUpObject.GetComponentInChildren<Rigidbody>();
		if(body){
			body.useGravity = false;
			body.freezeRotation = true;
		}
		//set layer to ignoreraycast 
		pickedUpObject.gameObject.layer = 2;

		//update the postion
		UpdateDraggedObjectPosition(pickedUpObject);
		selectedObject = pickedUpObject;
		Services.AudioManager.PlaySFX (pickupClip, sfxVolume);

			
	}

	void UpdateDraggedObjectPosition(Transform draggedObject){
		
		//get the ray
		Ray ray = UpperCamera.ScreenPointToRay(Input.mousePosition);

		//update the mouse position when mouse ray intersects with the plane
		float rayDistance = 10;

		//update plane
		draggedPlane.SetNormalAndPosition(
			UpperCamera.transform.forward,
			inPointForPlaneFromCube);

		//update dragged object position
		if (draggedPlane.Raycast (ray, out rayDistance)) {
			draggedObject.position = ray.GetPoint (rayDistance);
			// Offset cursor position.
			//draggedObject.position = new Vector3(transform.position.x /*+ cursorOffset.x*/, transform.position.y /*+ cursorOffset.y*/, transform.position.z);
		}

	}

	void DetectPlacing(Transform draggedObject){
		//detect click
		if(Input.GetMouseButtonUp(0)){
			Debug.Log("place click");
			Ray ray = UpperCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)){
				if(hit.collider.tag.Equals("Visor")){
					Debug.Log("place in visor");
					//set layer to default 
					selectedObject.gameObject.layer = 0;
				}else{
					Debug.LogError("click "+selectedObject.name+" in "+hit.collider.name);
				}
			}else{
				//set layer to default 
				selectedObject.gameObject.layer = 0;

				Debug.Log("Player threw "+selectedObject.name);
				ThrowObject();
			}
				
			clickGapCount = 0;
            Rigidbody body = selectedObject.GetComponentInChildren<Rigidbody>();
			if(body){
	            body.isKinematic = false;
	            body.useGravity = true;
	            body.freezeRotation = false;
			}
            if (selectedObject.FindChild("Incoherence Controller") != null) selectedObject.FindChild("Incoherence Controller").gameObject.SetActive(true);
            if (selectedObject.FindChild("NPC AI") != null) selectedObject.FindChild("NPC AI").gameObject.SetActive(true);
            selectedObject.GetComponentInChildren<InteractionSettings>().carryingObject = null;

            selectedObject = null;
			//change state back
			//state = InterationState.NONE_SELECTED_STATE;
		}

	}

	void ThrowObject() {
		Services.AudioManager.PlaySFX (throwClip, sfxVolume);

		Transform carriedObject = selectedObject;

		Ray ray = playercontroller.m_Camera.ScreenPointToRay(Input.mousePosition);
		carriedObject.position = playercontroller.m_Camera.transform.position + ray.direction*5f;

        carriedObject.SetParent(carriedObject.GetComponentInChildren<InteractionSettings>().originalParent, true);

		carriedObject.GetComponent<Rigidbody> ().AddExplosionForce (throwForce*5, playercontroller.m_Camera.transform.position, 10f);
	}

	//if this object can be interacted with, return true; else return false
	bool isAbleToBeCarried(InteractionSettings interactionSettings){
		if (interactionSettings != null && interactionSettings.ableToBeCarried) {
			return true;
		}else{
			return false;
		}

	}

	bool isAbleToBeUse(InteractionSettings interactionSettings){
		if (interactionSettings != null) {
			if (interactionSettings.usable) {
				return true;

			}
		}
		return false;
	}

	public void StopHoldingItemInMouse()
	{
		Debug.Log ("hi");

		selectedObject.gameObject.layer = 0;
		selectedObject = null;
		//change state back
		//state = InterationState.NONE_SELECTED_STATE;

	}
}
