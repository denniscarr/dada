using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
    public float sfxVolume = 0.3f;

    public Text txtInfo;
    public Writer writer;
    public Vector3 textPosition;

    string lastCursorName;

	float CLICKGAPTIME = 0.3f;
	public Shader outlineShader;
	private Transform lastTargetObject;
	List<Renderer> renderList;
	List<string> shaderList;

	// Use this for initialization
	void Start () {

		UpperCamera = playercontroller.UpperCamera;
        //txtInfo = transform.parent.FindChild("txtInfo").GetComponent<Text>();

        writer = GetComponent<Writer>();
        writer.lineLength = 20f;

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
	void Update () {
		if(playercontroller.Mode == ControlMode.ZOOM_OUT_MODE){
			transform.position = Input.mousePosition;
			clickGapCount += Time.fixedDeltaTime;
			if(clickGapCount > CLICKGAPTIME){
				DetectSelection();
			}
		}else{
			GetComponent<RectTransform>().localPosition = Vector3.zero;

			if(playercontroller.Mode == ControlMode.ZOOM_IN_MODE){
				if(selectedObject){
					Debug.Log("Switch mode when picking up object");
					ThrowObject();
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
				}
			}
		}

			
	}

	//detect if mouse click on something, then switch and save the selected object
	void DetectSelection(){
		//get the ray to check whether player points at visor from upper camera
		if(selectedObject){
			//Debug.Log("select "+selectedObject.name);
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

	void DeoutlineTargetObject(){
		//Debug.Log("DeoutlineTargetObject");
		if(renderList != null){
			for(int i = 0; i < renderList.Count;i++){
				if(renderList[i]){
					renderList[i].material.shader = Shader.Find(shaderList[i]);
				}
				//Debug.Log(shaderList[i]);
			}

			renderList.Clear();
			shaderList.Clear();
		}

	}


	void OutlineTargetObject(Transform t_hit){

        if (t_hit.name.Contains("Grail")) return;

		renderList = new List<Renderer>();
		shaderList = new List<string>();
		Renderer renderer = t_hit.GetComponent<Renderer>();
		if(renderer && renderer.GetComponent<ParticleSystem>() == null){
			shaderList.Add(renderer.material.shader.name);
			//Debug.Log(renderer.material.shader.name);
			renderList.Add(renderer);
			renderer.material.shader = Shader.Find("Mistral/Outline");
		}else{
			Renderer[] renderers = t_hit.GetComponentsInChildren<Renderer>();
			for(int i = 0;i<renderers.Length;i++){
				if(renderers[i] && renderers[i].GetComponent<ParticleSystem>() == null){
					renderList.Add(renderers[i]);
					//Debug.Log(renderers[i].material.shader.name);
					shaderList.Add(renderers[i].material.shader.name);
					renderers[i].material.shader = Shader.Find("Mistral/Outline");
				}
			}
		}
	}

	public void TutorialPressTabTip(int num){
		writer.WriteAtPoint("Press Tab " + num.ToString() + " times", textPosition);
	}

	void CheckPointedObject(Transform pointedObject){
		InteractionSettings inSets = pointedObject.GetComponentInChildren<InteractionSettings>();

        // HELPER TEXT STUFF:
        if (inSets != null)
        {

			DeoutlineTargetObject();
			OutlineTargetObject(pointedObject);

            // If the player already owns this item:
            if (inSets.isOwnedByPlayer)
            {
                if (inSets.ableToBeCarried)
                {
                    ChangeCursor("openHand");
                    writer.WriteAtPoint("Hold Left Mouse Button to pick up " + pointedObject.name + ".", textPosition);
                }
            }

            // If the player does not own this item:
            else
            {
                // If the player has enough money to purchase:
                if (GameObject.Find("Bootstrapper").GetComponent<PlayerMoneyManager>().funds >= inSets.price)
                {
                    ChangeCursor("buying");
                    writer.WriteAtPoint("Press Left Mouse Button to purchase " + pointedObject.name + " for $" + pointedObject.GetComponentInChildren<InteractionSettings>().price + ".", textPosition);
                }

                // If the player does not have enough money to purchase:
                else
                {
                    writer.WriteAtPoint("You need $" + pointedObject.GetComponentInChildren<InteractionSettings>().price + " to purchase this " + pointedObject.gameObject.name + ".", textPosition);
                }
            }
        }

        else
        {
			DeoutlineTargetObject();
            writer.DeleteTextBox();
            ChangeCursor("idle");
        }

        if (Input.GetMouseButtonDown(0)){

            if (pointedObject.GetComponentInChildren<GrailFunction>() != null)
            {
                pointedObject.GetComponentInChildren<GrailFunction>().Use();
                return;
            }

            if (CheckAbility(inSets,false)){
                GetComponent<Image> ().color = new Color(1,1,1,1);
				if(playercontroller.Mode == ControlMode.ZOOM_IN_MODE){
					//EquippableFinder.cs
				}else if(playercontroller.Mode == ControlMode.ZOOM_OUT_MODE){
                    //txtInfo.text = "pick up "+pointedObject.name;
                    //Debug.Break();
					PickUpObject(pointedObject);
				}else{
					//InsideVisorMan.cs
				}
			}else if(!pointedObject.name.Equals("GROUND")){
                writer.DeleteTextBox();
                writer.WriteAtPoint(pointedObject.name + " refuses to be picked up.", textPosition);
				//txtInfo.text = pointedObject.name + " refuses to be picked up.";
				GetComponent<Image> ().color = new Color(1,0,0,0.5f);
				//give cannot feedback
			}

		}
		else if(Input.GetMouseButtonDown(1)){
			if(CheckAbility(inSets,true)){
				GetComponent<Image> ().color = new Color(1,1,1,1);
				//use object
				//txtInfo.text = "Use "+pointedObject.name;
				//UseHandler();
			}else{
                //give cannot feedback
                writer.DeleteTextBox();
                writer.WriteAtPoint(pointedObject.name + " refuses to be used.", textPosition);
				//txtInfo.text = pointedObject.name + " refuses to be used.";
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
        // If this item is not yet owned by the player, then handle buying.
        InteractionSettings intSet = pickedUpObject.GetComponentInChildren<InteractionSettings>();
        PlayerMoneyManager moneyManager = GameObject.Find("Bootstrapper").GetComponent<PlayerMoneyManager>();
        if (!intSet.isOwnedByPlayer)
        {
            if (moneyManager.funds >= intSet.price)
            {
                intSet.GetPurchased();
                writer.DeleteTextBox();
            }

            else
            {
                writer.WriteAtPoint("You cannot afford this " + pickedUpObject.name + "!", textPosition);
            }

            return;
        }

		if(pickedUpObject.parent != t_INROOMOBJECTS){
			//change the parent of selected object

			//if(!(pickedUpObject.parent && pickedUpObject.parent.name.Equals("Equip Reference"))){
            if (1 == 1) {
                //Debug.Log("resize");
                //change scale
                float distanceInside = Mathf.Abs(
                    Vector3.Dot((inPointForPlaneFromCube - UpperCamera.transform.position),
                        UpperCamera.transform.forward));
                float distance = Mathf.Abs(
                    Vector3.Dot((pickedUpObject.position - playercontroller.m_Camera.transform.position),
                        playercontroller.m_Camera.transform.forward));
                if (distance < 0)
                {
                    Debug.Log("error");
                }
                float frustumHeightInside = distanceInside * Mathf.Tan(UpperCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
                float frustumHeight = distance * Mathf.Tan(playercontroller.m_Camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
                float scale = frustumHeightInside / frustumHeight;
                pickedUpObject.localScale *= scale;

			}else{
                pickedUpObject.localScale = pickedUpObject.GetComponentInChildren<InteractionSettings>().savedScale;
			}
			pickedUpObject.SetParent(t_INROOMOBJECTS,true);
            pickedUpObject.GetComponentInChildren<InteractionSettings>().carryingObject = Services.Player.transform;

			//stop gravity simulation and free rotation
			//Debug.Log("pick up "+pickedUpObject.name+" outside visor");

		}else{

			//Debug.Log("pick up "+pickedUpObject.name+" inside visor");
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

        ChangeCursor("closedHand");

        bool hitVisor = false;

        Ray ray = UpperCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] rayHits = Physics.RaycastAll(ray, 50f);
        foreach (RaycastHit hit in rayHits)
        {
            if (hit.collider.tag.Equals("Visor"))
            {
                hitVisor = true;
            }
        }

        string helperText = "";

        if (hitVisor)
        {
            helperText = "Release Left Mouse Button to place " + draggedObject.name + " in your room.";

        }
        else
        {
            helperText = "Release Left Mouse Button to throw " + draggedObject.name + ".";
        }
        

        if (writer.lastWrite != helperText)
        {
            writer.DeleteTextBox();
            writer.WriteAtPoint(helperText, textPosition);
        }

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
			//Debug.Log("place click");
			Ray ray = UpperCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit)){
				if(hit.collider.tag.Equals("Visor")){
					//Debug.Log("place in visor");
					//set layer to default 

				}else{
					//Debug.LogError("click "+selectedObject.name+" in "+hit.collider.name);
				}
			}else{
				//set layer to default 
				//selectedObject.gameObject.layer = 0;

				//Debug.Log("Player threw "+selectedObject.name);
				ThrowObject();
			}
			selectedObject.GetComponent<Collider>().isTrigger = false;
			selectedObject.gameObject.layer = 0;
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

        writer.DeleteTextBox();

		Transform carriedObject = selectedObject;
		selectedObject.gameObject.layer = 0;

		Ray ray = playercontroller.m_Camera.ScreenPointToRay(Input.mousePosition);
		carriedObject.position = playercontroller.m_Camera.transform.position + ray.direction*5f;

        carriedObject.SetParent(carriedObject.GetComponentInChildren<InteractionSettings>().originalParent, true);
        carriedObject.localScale = carriedObject.GetComponentInChildren<InteractionSettings>().savedScale;

        carriedObject.GetComponentInChildren<InteractionSettings>().carryingObject = null;

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
		selectedObject.gameObject.layer = 0;
		selectedObject = null;
		//change state back
		//state = InterationState.NONE_SELECTED_STATE;
	}

    public void ChangeCursor(string cursorName)
    {
        if (cursorName == lastCursorName) return;

        // Set all animator bools to false.
        Animator myAnimator = GetComponent<Animator>();
        myAnimator.SetBool("idle", false);
        myAnimator.SetBool("buying", false);
        myAnimator.SetBool("equip", false);
        myAnimator.SetBool("cantBuy", false);
        myAnimator.SetBool("openHand", false);
        myAnimator.SetBool("closedHand", false);

        myAnimator.SetBool(cursorName, true);

        lastCursorName = cursorName;
    }
}
