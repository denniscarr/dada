using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EquippableFinder : MonoBehaviour {

    GameObject[] equippables;

    public float equipRange = 5f;   // How close the player needs to be to equip an object.
    public float equipSize = 1f;    // The radius of the capsule used to find objects the player is looking at.

    Transform equipTarget;  // The object that can currently be equipped.
    public Transform equippedObject;
    public List<Transform> equippedObjects;
    public List<Transform> tweeningObjects;

    Transform buyTarget;    // The object that can currently be bought.

    Transform lastObjectInspected;  // The last object that was looked at using the mouse.

    Writer writer;
    public Vector3 textPosition;   // Where equip text is written.

    KeyCode equipKey = KeyCode.Mouse0;
    KeyCode abandonKey = KeyCode.G;
    //private bool readyToEquip = false;
    Transform equipReference;

    InteractionSettings intSet;

    // Related to the throw force.
    public float speed = 1;
    public float ASpeed = 10;

    // Whether I do anything:
    public bool active = true;

    Vector3 originalScale;

	List<Renderer> renderList;
	List<string> shaderList;

    MouseControllerNew mouse;


    void Start()
    {
        if (gameObject.name == "UpperCamera")
        {
            active = false;
        }

        else
        {
            active = true;
        }

        mouse = GameObject.Find("Mouse").GetComponent<MouseControllerNew>();

		renderList = new List<Renderer>();
		shaderList = new List<string>();
        // Get references to my buddies.
        writer = GetComponent<Writer>();

        if (gameObject.name == "UpperCamera")
        {
            equipReference = GameObject.Find("Upper Equip Reference").transform;
        }

        else
        {
            equipReference = GameObject.Find("Equip Reference").transform;
        }

        equippedObjects = new List<Transform>();
        tweeningObjects = new List<Transform>();

        writer.textSize = 0.1f;
        textPosition = transform.position + transform.forward * 20f;
    }


    void Update()
    {
        if (!active)
        {
            return;
        }

        //Debug.DrawRay(transform.position + transform.forward, transform.forward * equipRange, Color.cyan);

        // Don't do anything if the player is in zoom out mode.
        if (GameObject.Find("PlayerInRoom").GetComponent<PlayerControllerNew>().Mode == ControlMode.ZOOM_OUT_MODE)
        {
            writer.DeleteTextBox();
            return;
        }

        // Update text position based on current position.
        textPosition = transform.position + transform.forward * 20f;

        // CHECK OUT EACH OBJECT IN RANGE.
        equipTarget = null;
        buyTarget = null;

        // A variable to save the closest object.
        Transform nearestObject = null;
        float nearestObjectDistance = 0f;
        bool sawPlatform = false;

        foreach (RaycastHit hit in Physics.CapsuleCastAll(
            transform.position + transform.forward, transform.position + transform.forward*1.0f, equipSize, transform.forward, equipRange
            ))
        {
			
            if (hit.transform.name != "Player" && hit.transform.GetComponentInChildren<InteractionSettings>() != null &&
                !hit.transform.GetComponentInChildren<InteractionSettings>().IsEquipped && hit.transform.name != "GROUND")
            {
				//Debug.Log(hit.transform.name);
                // Get the distance of this object and, if it's the closest to the player then save it.
                float distance = Vector3.Distance(hit.point, transform.position);

                // Special case for looking at grail.
                if (hit.transform.GetComponentInChildren<GrailFunction>() != null)
                {
                    hit.transform.GetComponentInChildren<GrailFunction>().ReadyToRunAway();
                }

                // If the nearest object has not yet been set, then save this object as the nearest object.
                if (nearestObject == null)
                {
                    if (gameObject.name == "UpperCamera" && hit.transform.name.Contains("QuestItNote")) return;
                    nearestObject = hit.transform;
                    nearestObjectDistance = Vector3.Distance(hit.point, transform.position);
                }

                // If the nearest object has been saved then see if this object is closer.
                else
                {
					Image mouse = GameObject.FindObjectOfType<MouseControllerNew>().GetComponent<Image>();
					//Debug.Log(mouse.name);
					Color c = mouse.color;
                    if (distance < nearestObjectDistance)
                    {
						c.a = 1;
                        nearestObject = hit.transform;
                        nearestObjectDistance = Vector3.Distance(hit.point, transform.position);

					}else if(distance - nearestObjectDistance < 5f){
						
						c.a = 1 - (distance - nearestObjectDistance)*0.2f;

					}else{
						c.a = 0;
					}
					mouse.color = c;
                }
            }

            else if (hit.collider.transform.parent != null && hit.collider.transform.parent.name == "Viewing Platform")
            {
                sawPlatform = true;
            }
        }

        if (!sawPlatform && nearestObject == null || (lastObjectInspected != null && nearestObject != lastObjectInspected))
        {
            writer.DeleteTextBox();
        }

        // Show the equip prompt for the nearest object.
        if (nearestObject != null)
        {
			//Debug.Log("Found object: "+nearestObject);
			//DeoutlineTargetObject();
			GameObject.FindObjectOfType<MouseControllerNew>().OutlineTargetObject(nearestObject);
			//OutlineTargetObject(nearestObject);

            if (nearestObject.GetComponentInChildren<InteractionSettings>().isOwnedByPlayer)
            {
                // If we're looking at money, display a special message.
                if (nearestObject.name.Contains("$"))
                {
                    writer.WriteAtPoint("Press Left Mouse Button to obtain " + nearestObject.name + ".", textPosition);
                }

                else
                {
                    writer.WriteAtPoint("Press Left Mouse Button to equip " + nearestObject.name + ".", textPosition);
                }

                //Debug.Log(mouse);
                mouse.ChangeCursor("equip");
                equipTarget = nearestObject;
                //Debug.Log(equipTarget.name);
            }
            // If the player does not own this item:
            else
            {
                // If the player has enough money to purchase this object.
                if (nearestObject.GetComponentInChildren<InteractionSettings>().price <= GameObject.Find("Bootstrapper").GetComponent<PlayerMoneyManager>().funds)
                {
                    mouse.ChangeCursor("buying");
                    writer.WriteAtPoint("Press Left Mouse Button to purchase " + nearestObject.name + " for $" + nearestObject.GetComponentInChildren<InteractionSettings>().price + ".", textPosition);
                    buyTarget = nearestObject;
                }

                // If the player does not have enough money to purchase this object.
                else
                {
                    mouse.ChangeCursor("cantBuy");
                    writer.WriteAtPoint("You need $" + nearestObject.GetComponentInChildren<InteractionSettings>().price + " to purchase this " + nearestObject.gameObject.name + ".", textPosition);
                }
            }
        }
        else 
        {
			GameObject.FindObjectOfType<MouseControllerNew>().DeoutlineTargetObject();
			mouse.ChangeCursor("idle");

        }

        // PLAYER INPUT

        // Buying targetted object.
        if (buyTarget != null && Input.GetKeyDown(equipKey))
        {
            writer.DeleteTextBox();
            if (buyTarget.GetComponentInChildren<InteractionSettings>().price <= GameObject.Find("Bootstrapper").GetComponent<PlayerMoneyManager>().funds)
            {
                buyTarget.GetComponentInChildren<InteractionSettings>().GetPurchased();
                writer.DeleteTextBox();
                writer.WriteAtPoint("Purchased " + buyTarget.name + " for $" + buyTarget.GetComponentInChildren<InteractionSettings>().price + ".", textPosition);
            }

            else
            {
                writer.WriteAtPoint("You cannot afford this " + buyTarget.name + "!", textPosition);
            }
        }

        // Equipping targetted object.
        else if (equipTarget != null && Input.GetKeyDown(equipKey))
        {
            MoveToCamera(equipTarget, equipReference);
        }
			
        // Abandoning equipped items.
        if (equippedObjects.Count >= 0 && Input.GetKeyDown(abandonKey))
        {
            Debug.Log(gameObject.name + " equipped object count " + equippedObjects.Count);
            AbandonItem();
        }
    }

	void VisorMoveComplete(){
		GameObject.FindObjectOfType<Tutorial>().SendMessage("TurnUpZoomOutMode");
	}

    void MoveToCamera (Transform _equipTarget, Transform _equipReference)
    {
		//Debug.Log(equipTarget.name + "move to camera");

        // SPECIAL CASE FOR EQUIPPING VISOR DURING TUTORIAL
		if(_equipTarget.name.Equals("visor")){
			GameObject playerCamera = GameObject.Find("Player Camera");
            _equipTarget.SetParent(playerCamera.transform);
            _equipTarget.DOScale(100*Vector3.one,2.0f);
            _equipTarget.DOLocalMoveY(2,0.5f);
            _equipTarget.DOLocalMove(new Vector3(-2.7f,2,22.32f),0.5f).SetDelay(0.5f);
            //_equipTarget.DOLocalMove(new Vector3(-2.7f,-4.49f,12.32f),1.0f).SetDelay(0.5f);
            _equipTarget.DOLocalRotate(new Vector3(0,180,0),1.0f);
            _equipTarget.DOLocalMoveY(-4.49f,1.0f).SetDelay(1.0f);
            _equipTarget.DOLocalMoveZ(4.3f,1.0f).SetDelay(1.0f).OnComplete(VisorMoveComplete);
            //_equipTarget.DORotate(new Vector3(0,180,0),1.5f);

            return;
		}

        // SPECIAL CASE FOR TRYING TO EQUIP GRAIL
        if (_equipTarget.GetComponentInChildren<GrailFunction>() != null)
        {
            _equipTarget.GetComponentInChildren<GrailFunction>().Use();
            return;
        }

        // Disable collision & gravity.
        _equipTarget.GetComponent<Collider>().isTrigger = true;
        if (_equipTarget.GetComponent<Collider>() != null) Physics.IgnoreCollision(_equipTarget.GetComponent<Collider>(), transform.parent.GetComponent<Collider>());
        if (_equipTarget.GetComponent<Rigidbody>() != null) _equipTarget.GetComponent<Rigidbody>().isKinematic = true;

        // Save object's scale.
        originalScale = _equipTarget.transform.localScale;

        // Set equip target as child of equip reference.
        _equipTarget.SetParent(_equipReference, true);
       
		//play equip sound effect
		Services.AudioManager.PlaySFX (Services.AudioManager.equipSound);

        // Get position and size for object.
        Vector3 equipPosition = _equipTarget.GetComponentInChildren<InteractionSettings>().equipPosition;
        Vector3 equipRotation = _equipTarget.GetComponentInChildren<InteractionSettings>().equipRotation;
        Vector3 equipScale = _equipTarget.localScale;

        Vector3 savedPosition = _equipTarget.localPosition;
        Quaternion savedRotation = _equipTarget.localRotation;
        Vector3 savedScale = _equipTarget.localScale;

        // Resize & reposition object so that it doesn't block the 
        _equipTarget.localPosition = equipPosition;
        _equipTarget.localRotation = Quaternion.Euler(equipRotation);
		if(Resources.Load<GameObject>("Pickups/"+_equipTarget.name)){
			_equipTarget.localScale = Resources.Load<GameObject>("Pickups/"+_equipTarget.name).transform.localScale;//equipScale;
		}else{
			_equipTarget.localScale = equipScale;
		}

		//equipScale
        int infinityPrevention = 0;
        bool niceSize = false;
        Camera myCamera = GetComponent<Camera>();
        RaycastHit hit;
        while (!niceSize)
        {
            if (Physics.Raycast(myCamera.ScreenPointToRay(new Vector3(myCamera.pixelWidth * 0.75f, myCamera.pixelHeight * 0.45f, 0f)), out hit, 5f))
            {
                //Debug.Log("hit a thing.");

                if (hit.collider.transform == _equipTarget)
                {
                    //Debug.Log("resized my thing");
                    _equipTarget.localPosition = new Vector3(
                        //_equipTarget.localPosition.x,
                        0f,
                        _equipTarget.localPosition.y - 0.1f,
                        _equipTarget.localPosition.z - 0.1f
                        );
                    _equipTarget.localScale *= 0.99f;
                }
            }
            else
            {
                equipPosition = _equipTarget.transform.localPosition;
                equipScale = _equipTarget.localScale;

                niceSize = true;
            }

            infinityPrevention += 1;
            if (infinityPrevention > 100)
            {
                break;
            }
        }

        _equipTarget.localPosition = savedPosition;
        _equipTarget.localScale = savedScale;
        _equipTarget.localRotation = savedRotation;

        // Tween object to correct position.
        if (equipRotation != Vector3.zero)
        {
            _equipTarget.transform.DOLocalRotate(equipRotation, 1.5f);
            //equippedObject.transform.localRotation = Quaternion.Euler(equippedObject.GetComponentInChildren<InteractionSettings>().equipRotation);
        }
        else
        {
            _equipTarget.transform.DOLocalRotate(Vector3.zero, 1.5f);
            //equippedObject.transform.rotation = equipReference.rotation;
        }

        if (equipPosition != Vector3.zero)
        {
            _equipTarget.transform.DOLocalMove(equipPosition, 1.5f);
            //equippedObject.transform.localPosition = equippedObject.GetComponentInChildren<InteractionSettings>().equipPosition;
        }
        else
        {
            //equipTarget.transform.localScale = equipReference.localScale;
            _equipTarget.transform.DOLocalMove(Vector3.zero, 1.5f);
            //equipTarget.transform.position = equipReference.position;
        }

        _equipTarget.transform.DOScale(equipScale, 1.5f).OnStart(StopPickUpAction);
        //equippedObject.transform.localPosition = equippedObject.GetComponentInChildren<InteractionSettings>().equipPosition;

        tweeningObjects.Add(_equipTarget);

        StartCoroutine("complete", _equipTarget);

		equipTarget = null;
    }


	void StopPickUpAction(){
		GameObject.FindObjectOfType<MouseControllerNew>().isTweening = true;
	}

	IEnumerator complete(Transform _equipTarget){

        yield return new WaitForSeconds(1.5f);
        if (_equipTarget != null)
        {
            //Debug.Log("complete " + _equipTarget.name);
            //Debug.Log("Coroutine finished");
            equippedObjects.Add(_equipTarget);
            Debug.Log(gameObject.name + " Added " + _equipTarget + " to list. Count: " + equippedObjects.Count);
		    _equipTarget.GetComponentInChildren<InteractionSettings>().carryingObject = Services.Player.transform;
            tweeningObjects.Remove(_equipTarget);
        }
        GameObject.FindObjectOfType<MouseControllerNew>().isTweening = false;
    }

    void CompleteInstant(Transform target)
    {
        target.DOKill();
        equippedObjects.Add(target);
        tweeningObjects.Remove(target);
        target.GetComponentInChildren<InteractionSettings>().carryingObject = Services.Player.transform;
    }

    //void myCompleteFunction(Transform _equipTarget){
    //	Debug.Log("on my complete "+equippedObject.name);
    //	equippedObject = _equipTarget;
    //	equippedObject.GetComponentInChildren<InteractionSettings>().carryingObject = Services.Player.transform;
    //	//return new TweenCallback();
    //}

    public void MoveEquippedItemsOutside()
    {
        for (int i = 0; i < equippedObjects.Count; i++)
        {
            if (equippedObjects[i] != null)
            {
                equippedObjects[i].localScale = equippedObjects[i].GetComponentInChildren<InteractionSettings>().savedScale;
                GameObject.Find("Player Camera").GetComponent<EquippableFinder>().MoveToCamera(equippedObjects[i], GameObject.Find("Equip Reference").transform);
            }
        }

        equippedObjects.Clear();

        for (int i = 0; i < tweeningObjects.Count; i++)
        {
            if (tweeningObjects[i] != null)
            {
                CompleteInstant(tweeningObjects[i]);
                equippedObjects[i].localScale = equippedObjects[i].GetComponentInChildren<InteractionSettings>().savedScale;
                GameObject.Find("Player Camera").GetComponent<EquippableFinder>().MoveToCamera(equippedObjects[i], GameObject.Find("Equip Reference").transform);
            }
        }

        tweeningObjects.Clear();
    }

    public void AbandonItem()
    {
        if (equippedObjects.Count <= 0) return;

        if (!active && gameObject.name == "Player Camera")
        {
            GameObject.Find("UpperCamera").BroadcastMessage("AbandonItem");
            return;
        }

        Services.AudioManager.PlaySFX(Services.AudioManager.dropSound);

        //Debug.Log("Started AbandonItem(). Count: " + equippedObjects.Count);

        for (int i = 0; i < equippedObjects.Count; i++)
        {
            if (equippedObjects[i] != null)
            {
                //Debug.Log(equippedObjects[i].name + " examined. Index: " + i);

                if (gameObject.name == "UpperCamera")
                {
                    equippedObjects[i].SetParent(GameObject.Find("INROOMOBJECTS").transform);
                }

                else
                {
                    equippedObjects[i].SetParent(null);
                }

                // Re-enable collision & stuff.
                equippedObjects[i].GetComponent<Collider>().isTrigger = false;
                if (equippedObjects[i].GetComponent<Collider>() != null) Physics.IgnoreCollision(equippedObjects[i].GetComponent<Collider>(), transform.parent.GetComponent<Collider>(), false);
                if (equippedObjects[i].GetComponent<Rigidbody>() != null)
                {
                    equippedObjects[i].GetComponent<Rigidbody>().isKinematic = false;
                    equippedObjects[i].GetComponent<Rigidbody>().useGravity = true;
                    equippedObjects[i].GetComponent<Rigidbody>().AddForce(transform.forward * ASpeed);
                }

                equippedObjects[i].transform.localScale = originalScale;

                if (gameObject.name != "UpperCamera")
                {
                    equippedObjects[i].GetComponentInChildren<InteractionSettings>().carryingObject = null;
                }
            }

            else
            {
                //Debug.Log("equipped object was null. Index: " + i);
            }

            //equippedObjects.Remove(equippedObjects[i]);
        }

        equippedObjects.Clear();
    }
}