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

//			if(hit.transform.parent && hit.transform.parent.name.Equals("Viewing Platform")){
//				writer.WriteAtPoint("Click to stand on " + nearestObject.name, textPosition);
//			}

            if (hit.transform.name != "Player" && hit.transform.GetComponentInChildren<InteractionSettings>() != null &&
                !hit.transform.GetComponentInChildren<InteractionSettings>().IsEquipped && hit.transform.name != "GROUND")
            {
				//Debug.Log(hit.transform.name);
                // Get the distance of this object and, if it's the closest to the player then save it.
                float distance = Vector3.Distance(hit.point, transform.position);

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
			DeoutlineTargetObject();
			OutlineTargetObject(nearestObject);

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

        else if (nearestObject == null)
        {
			DeoutlineTargetObject();
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
            MoveToCamera();
        }
			
        // Abandoning equipped items.
        if (equippedObjects.Count > 0 && Input.GetKeyDown(abandonKey))
        {
            AbandonItem();
        }
    }


	void DeoutlineTargetObject(){
		GameObject.FindObjectOfType<MouseControllerNew>().GetComponent<Image>().DOFade(0.2f,0.5f);
		//Debug.Log("DeoutlineTargetObject");
		for(int i = 0; i < renderList.Count;i++){
            if (renderList[i] != null)
            {
                renderList[i].material.shader = Shader.Find(shaderList[i]);
            }
			//Debug.Log(shaderList[i]);
		}

        //lastObjectInspected = null;
		renderList.Clear();
		shaderList.Clear();
	}


	void OutlineTargetObject(Transform t_hit){
		GameObject.FindObjectOfType<MouseControllerNew>().GetComponent<Image>().DOFade(1f,0.5f);
        //Debug.Log("OutlineTargetObject");

        lastObjectInspected = t_hit;

        if (t_hit.GetComponent<Renderer>() == null || t_hit.name.Contains("Grail")) return;

        renderList = new List<Renderer>();
		shaderList = new List<string>();
		Renderer renderer = t_hit.GetComponent<Renderer>();
		if(renderer){
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

	void VisorMoveComplete(){
		GameObject.FindObjectOfType<Tutorial>().SendMessage("TurnUpZoomOutMode");
	}

    void MoveToCamera ()
    {
		Debug.Log(equipTarget.name + "move to camera");
		if(equipTarget.name.Equals("visor")){
			GameObject playerCamera = GameObject.Find("Player Camera");
			equipTarget.SetParent(playerCamera.transform);
			equipTarget.DOScale(100*Vector3.one,2.0f);
			equipTarget.DOLocalMoveY(2,0.5f);
			equipTarget.DOLocalMove(new Vector3(-2.7f,2,22.32f),0.5f).SetDelay(0.5f);
			//equipTarget.DOLocalMove(new Vector3(-2.7f,-4.49f,12.32f),1.0f).SetDelay(0.5f);
			equipTarget.DOLocalRotate(new Vector3(0,180,0),1.0f);
			equipTarget.DOLocalMoveY(-4.49f,1.0f).SetDelay(1.0f);
			equipTarget.DOLocalMoveZ(4.3f,1.0f).SetDelay(1.0f).OnComplete(VisorMoveComplete);
			//equipTarget.DORotate(new Vector3(0,180,0),1.5f);

			return;
		}

        // Special case for trying to equip the grail.
        if (equipTarget.GetComponentInChildren<GrailFunction>() != null)
        {
            equipTarget.GetComponentInChildren<GrailFunction>().Use();
            return;
        }

        // Disable collision & gravity.
        equipTarget.GetComponent<Collider>().isTrigger = true;
        if (equipTarget.GetComponent<Collider>() != null) Physics.IgnoreCollision(equipTarget.GetComponent<Collider>(), transform.parent.GetComponent<Collider>());
        if (equipTarget.GetComponent<Rigidbody>() != null) equipTarget.GetComponent<Rigidbody>().isKinematic = true;

        // Save object's scale.
        originalScale = equipTarget.transform.localScale;

        // Set equip target as child of equip reference.
        equipTarget.SetParent(equipReference, true);
       
		//play equip sound effect
		Services.AudioManager.PlaySFX (Services.AudioManager.equipSound);

        // Get position and size for object.
        Vector3 equipPosition = equipTarget.GetComponentInChildren<InteractionSettings>().equipPosition;
        Vector3 equipRotation = equipTarget.GetComponentInChildren<InteractionSettings>().equipRotation;
        Vector3 equipScale = equipTarget.localScale;

        Vector3 savedPosition = equipTarget.localPosition;
        Quaternion savedRotation = equipTarget.localRotation;
        Vector3 savedScale = equipTarget.localScale;

        // Resize & reposition object so that it doesn't block the 
        equipTarget.localPosition = equipPosition;
        equipTarget.localRotation = Quaternion.Euler(equipRotation);
        equipTarget.localScale = equipScale;

        int infinityPrevention = 0;
        bool niceSize = false;
        Camera myCamera = GetComponent<Camera>();
        RaycastHit hit;
        while (!niceSize)
        {
            if (Physics.Raycast(myCamera.ScreenPointToRay(new Vector3(myCamera.pixelWidth * 0.75f, myCamera.pixelHeight * 0.45f, 0f)), out hit, 5f))
            {
                //Debug.Log("hit a thing.");

                if (hit.collider.transform == equipTarget)
                {
                    //Debug.Log("resized my thing");
                    equipTarget.localPosition = new Vector3(
                        equipTarget.localPosition.x,
                        equipTarget.localPosition.y - 0.1f,
                        equipTarget.localPosition.z - 0.1f
                        );
                    equipTarget.localScale *= 0.99f;
                }
            }
            else
            {
                equipPosition = equipTarget.transform.localPosition;
                equipScale = equipTarget.localScale;

                equipTarget.localPosition = savedPosition;
                equipTarget.localScale = savedScale;
                equipTarget.localRotation = savedRotation;
                niceSize = true;
            }

            infinityPrevention += 1;
            if (infinityPrevention > 100)
            {
                break;
            }
        }

        // Tween object to correct position.
        if (equipRotation != Vector3.zero)
        {
            equipTarget.transform.DOLocalRotate(equipRotation, 1.5f);
            //equippedObject.transform.localRotation = Quaternion.Euler(equippedObject.GetComponentInChildren<InteractionSettings>().equipRotation);
        }
        else
        {
            equipTarget.transform.DOLocalRotate(Vector3.zero, 1.5f);
            //equippedObject.transform.rotation = equipReference.rotation;
        }

        if (equipPosition != Vector3.zero)
        {
            equipTarget.transform.DOLocalMove(equipPosition, 1.5f);
            //equippedObject.transform.localPosition = equippedObject.GetComponentInChildren<InteractionSettings>().equipPosition;
        }
        else
        {
            //equipTarget.transform.localScale = equipReference.localScale;
            equipTarget.transform.DOLocalMove(Vector3.zero, 1.5f);
            //equipTarget.transform.position = equipReference.position;
        }

		equipTarget.transform.DOScale(equipScale, 1.5f).OnStart(StopPickUpAction);
        //equippedObject.transform.localPosition = equippedObject.GetComponentInChildren<InteractionSettings>().equipPosition;

        StartCoroutine("complete", equipTarget);

		equipTarget = null;
    }

	void StopPickUpAction(){
		GameObject.FindObjectOfType<MouseControllerNew>().isTweening = true;
	}

	IEnumerator complete(Transform _equipTarget){
        Debug.Log("complete " + _equipTarget.name);
        yield return new WaitForSeconds(1.5f);
        if (_equipTarget != null)
        {
            //Debug.Log("Coroutine finished");
		    equippedObjects.Add(_equipTarget);
			GameObject.FindObjectOfType<MouseControllerNew>().isTweening = false;
		    _equipTarget.GetComponentInChildren<InteractionSettings>().carryingObject = Services.Player.transform;
        }
	}

	//void myCompleteFunction(Transform _equipTarget){
	//	Debug.Log("on my complete "+equippedObject.name);
	//	equippedObject = _equipTarget;
	//	equippedObject.GetComponentInChildren<InteractionSettings>().carryingObject = Services.Player.transform;
	//	//return new TweenCallback();
	//}

    public void AbandonItem()
    {
        if (equippedObjects.Count <= 0) return;

        if (!active && gameObject.name == "Player Camera")
        {
            GameObject.Find("UpperCamera").BroadcastMessage("AbandonItem");
            return;
        }

        Services.AudioManager.PlaySFX(Services.AudioManager.dropSound);

        for (int i = 0; i < equippedObjects.Count; i++)
        {
            if (equippedObjects[i] != null)
            {
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

                equippedObjects.Remove(equippedObjects[i]);
            }
        }
    }
}