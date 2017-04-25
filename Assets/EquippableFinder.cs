using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippableFinder : MonoBehaviour {

    GameObject[] equippables;

    public float equipRange = 5f;   // How close the player needs to be to equip an object.
    public float equipSize = 1f;    // The radius of the capsule used to find objects the player is looking at.

    Transform equipTarget;  // The object that can currently be equipped.
    public Transform equippedObject;

    Transform buyTarget;    // The object that can currently be bought.

    Writer writer;
    Vector3 textPosition;   // Where equip text is written.

    KeyCode equipKey = KeyCode.Mouse0;
    KeyCode abandonKey = KeyCode.G;
    //private bool readyToEquip = false;
    Transform equipReference;

    InteractionSettings intSet;

    // Related to the throw force.
    public float speed = 1;
    public float ASpeed = 10;

    Vector3 originalScale;

	List<Renderer> renderList;
	List<string> shaderList;

    void Start()
    {
		renderList = new List<Renderer>();
		shaderList = new List<string>();
        // Get references to my buddies.
        writer = GetComponent<Writer>();
        equipReference = GameObject.Find("Equip Reference").transform;

        writer.textSize = 0.1f;
        textPosition = transform.position + transform.forward * 20f;
    }


    void Update()
    {
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

        foreach (RaycastHit hit in Physics.CapsuleCastAll(
            transform.position + transform.forward, transform.position + transform.forward*1.5f, equipSize, transform.forward, equipRange
            ))
        {
            if (hit.transform.name != "Player" && hit.transform.GetComponentInChildren<InteractionSettings>() != null &&
                !hit.transform.GetComponentInChildren<InteractionSettings>().IsEquipped && hit.transform.GetComponentInChildren<InteractionSettings>().ableToBeCarried)
            {
                // Get the distance of this object and, if it's the closest to the player then save it.
                float distance = Vector3.Distance(hit.point, transform.position);

                // If the nearest object has not yet been set, then save this object as the nearest object.
                if (nearestObject == null)
                {
                    nearestObject = hit.transform;
                    nearestObjectDistance = Vector3.Distance(hit.point, transform.position);
                }

                // If the nearest object has been saved then see if this object is closer.
                else
                {
                    if (distance < nearestObjectDistance)
                    {
                        nearestObject = hit.transform;
                        nearestObjectDistance = Vector3.Distance(hit.point, transform.position);
                    }
                }
            }
        }

        // Show the equip prompt for the nearest object. (Just debug log for now.)
        if (nearestObject != null)
        {

			DeoutlineTargetObject();
			OutlineTargetObject(nearestObject);

            if (nearestObject.GetComponentInChildren<InteractionSettings>().isOwnedByPlayer)
            {
                writer.WriteAtPoint("Press Left Mouse Button to equip " + nearestObject.name, textPosition);
                equipTarget = nearestObject;
                //Debug.Log(equipTarget.name);
            }

            // If the player does not own this item:
            else
            {
                // If the player has enough money to purchase this object.
                if (nearestObject.GetComponentInChildren<InteractionSettings>().price < GameObject.Find("Bootstrapper").GetComponent<PlayerMoneyManager>().funds)
                {
                    writer.WriteAtPoint("Press Left Mouse Button to purchase " + nearestObject.name + " for $" + nearestObject.GetComponentInChildren<InteractionSettings>().price + ".", textPosition);
                    buyTarget = nearestObject;
                }

                // If the player does not have enough money to purchase this object.
                else
                {
                    writer.WriteAtPoint("You need $" + nearestObject.GetComponentInChildren<InteractionSettings>().price + " to purchase this " + nearestObject.gameObject.name + ".", textPosition);
                }
            }
        }

        else
        {
			DeoutlineTargetObject();
            writer.DeleteTextBox();
        }

        // PLAYER INPUT

        // Buying targetted object.
        if (buyTarget != null && Input.GetKeyDown(equipKey))
        {
            writer.DeleteTextBox();
            if (buyTarget.GetComponentInChildren<InteractionSettings>().price <= GameObject.Find("Bootstrapper").GetComponent<PlayerMoneyManager>().funds)
            {
                buyTarget.GetComponentInChildren<InteractionSettings>().GetPurchased();
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
        if (equippedObject != null && Input.GetKeyDown(abandonKey))
        {
            AbandonItem();
        }
    }

	void DeoutlineTargetObject(){
		//Debug.Log("DeoutlineTargetObject");
		for(int i = 0; i < renderList.Count;i++){
			renderList[i].material.shader = Shader.Find(shaderList[i]);
			//Debug.Log(shaderList[i]);
		}

		renderList.Clear();
		shaderList.Clear();
	}


	void OutlineTargetObject(Transform t_hit){
		//Debug.Log("OutlineTargetObject");


		renderList = new List<Renderer>();
		shaderList = new List<string>();
		Renderer renderer = t_hit.GetComponent<Renderer>();
		if(renderer){
			shaderList.Add(renderer.material.shader.name);
			Debug.Log(renderer.material.shader.name);
			renderList.Add(renderer);
			renderer.material.shader = Shader.Find("Mistral/Outline");
		}else{
			Renderer[] renderers = t_hit.GetComponentsInChildren<Renderer>();
			for(int i = 0;i<renderers.Length;i++){
				if(renderers[i]){
					renderList.Add(renderers[i]);
					//Debug.Log(renderers[i].material.shader.name);
					shaderList.Add(renderers[i].material.shader.name);
					renderers[i].material.shader = Shader.Find("Mistral/Outline");
				}
			}
		}
	}

    void MoveToCamera ()
    {
        // Disable collision & gravity.
        equippedObject = equipTarget;
        //equippedObject.GetComponent<Collider>().enabled = false;
        if (equippedObject.GetComponent<Collider>() != null) Physics.IgnoreCollision(equippedObject.GetComponent<Collider>(), transform.parent.GetComponent<Collider>());
        if (equippedObject.GetComponent<Rigidbody>() != null) equippedObject.GetComponent<Rigidbody>().isKinematic = true;

        // Save object's scale.
        originalScale = equippedObject.transform.localScale;

        equippedObject.transform.SetParent(equipReference, true);

        // Set position & parentage.
        if (equippedObject.GetComponentInChildren<InteractionSettings>().equipRotation != Vector3.zero)
        {
            equippedObject.transform.localRotation = Quaternion.Euler(equippedObject.GetComponentInChildren<InteractionSettings>().equipRotation);
        }

        else
        {
            equippedObject.transform.rotation = equipReference.rotation;
        }

        if (equippedObject.GetComponentInChildren<InteractionSettings>().equipPosition != Vector3.zero)
        {
            equippedObject.transform.localPosition = equippedObject.GetComponentInChildren<InteractionSettings>().equipPosition;
        }

        else
        {
			//equippedObject.transform.localScale = equipReference.localScale;
            equippedObject.transform.position = equipReference.position;
        }

        // Resize & reposition object so that it doesn't block the camera
        int infinityPrevention = 0;
        bool niceSize = false;
        Camera myCamera = GetComponent<Camera>();
        RaycastHit hit;
        while (!niceSize)
        {
            if (Physics.Raycast(myCamera.ScreenPointToRay(new Vector3(myCamera.pixelWidth * 0.75f, myCamera.pixelHeight * 0.45f, 0f)), out hit, 5f))
            {
                Debug.Log("hit a thing.");

                if (hit.collider.transform == equippedObject)
                {
                    Debug.Log("resized my thing");
                    equippedObject.localPosition = new Vector3(
                        equippedObject.localPosition.x,
                        equippedObject.localPosition.y - 0.1f,
                        equippedObject.localPosition.z - 0.1f
                        );
                    equippedObject.localScale *= 0.99f;
                }
            }

            else
            {
                niceSize = true;
            }

            infinityPrevention += 1;
            if (infinityPrevention > 100)
            {
                Debug.Log("Broke out of infinite loop.");
                break;
            }
        }

        equippedObject.GetComponentInChildren<InteractionSettings>().carryingObject = Services.Player.transform;
    }


    public void AbandonItem()
    {
        equippedObject.transform.SetParent(null);

        // Re-enable collision & stuff.
        //equippedObject.GetComponent<Collider>().isTrigger = false;
        if (equippedObject.GetComponent<Collider>() != null) Physics.IgnoreCollision(equippedObject.GetComponent<Collider>(), transform.parent.GetComponent<Collider>(), false);
        if (equippedObject.GetComponent<Rigidbody>() != null)
        {
            equippedObject.GetComponent<Rigidbody>().isKinematic = false;
            equippedObject.GetComponent<Rigidbody>().AddForce(transform.forward * ASpeed);
        }

        equippedObject.transform.localScale = originalScale;

        equippedObject.GetComponentInChildren<InteractionSettings>().carryingObject = null;
    }
}