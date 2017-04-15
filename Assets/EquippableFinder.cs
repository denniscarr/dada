using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippableFinder : MonoBehaviour {

    GameObject[] equippables;

    public float equipRange = 5f;   // How close the player needs to be to equip an object.
    public float equipSize = 1f;    // The radius of the capsule used to find objects the player is looking at.

    Transform equipTarget;  // The object that can currently be equipped.
    public Transform equippedObject;

    Writer writer;

	public KeyCode equipKey = KeyCode.Mouse0;
    public KeyCode abandonKey = KeyCode.G;
    private bool readyToEquip = false;
    Transform equipReference;

    InteractionSettings intSet;

    // Related to the throw force.
    public float speed = 1;
    public float ASpeed = 10;

    Vector3 originalScale;


    void Start()
    {
        // Get references to my buddies.
        writer = GetComponent<Writer>();
        equipReference = GameObject.Find("New Equip Reference").transform;
    }


    void Update()
    {
        Debug.DrawRay(transform.position + transform.forward, transform.forward * equipRange, Color.cyan);

        // CHECK OUT EACH OBJECT IN RANGE.
        equipTarget = null;

        // A variable to save the closest object.
        Transform nearestObject = null;
        float nearestObjectDistance = 0f;

        foreach (RaycastHit hit in Physics.CapsuleCastAll(
            transform.position + transform.forward, transform.position + transform.forward*1.5f, equipSize, transform.forward, equipRange
            ))
        {
            if (hit.transform.name != "Player" && hit.transform.GetComponentInChildren<InteractionSettings>() != null && hit.transform.GetComponentInChildren<InteractionSettings>().ableToBeCarried)
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
            writer.WriteAtPoint("Press E to equip " + nearestObject.name, transform.position + transform.forward*20f);
            equipTarget = nearestObject;
        }

        else
        {
            writer.DeleteTextBox();
        }


        // PLAYER INPUT
        if (equipTarget != null && Input.GetKeyDown(equipKey))
        {
            MoveToCamera();
        }

        if (equippedObject != null && Input.GetKeyDown(abandonKey))
        {
            AbandonItem();
        }
    }

    void MoveToCamera ()
    {
        // Disable collision & gravity.
        equippedObject = equipTarget;
        //equippedObject.GetComponent<Collider>().enabled = false;
        if (equippedObject.GetComponent<Rigidbody>() != null) equippedObject.GetComponent<Rigidbody>().isKinematic = true;

        originalScale = equippedObject.transform.localScale;

		equippedObject.transform.SetParent(equipReference.parent, true);


        if (equippedObject.GetComponentInChildren<InteractionSettings>().equipRotation != Vector3.zero)
        {
            equippedObject.transform.localRotation = Quaternion.Euler(equippedObject.GetComponentInChildren<InteractionSettings>().equipRotation);
        }

        else
        {
            equippedObject.transform.rotation = equipReference.rotation;
        }

        // Set position & parentage.
        if (equippedObject.GetComponentInChildren<InteractionSettings>().equipPosition != Vector3.zero)
        {
            equippedObject.transform.localPosition = equippedObject.GetComponentInChildren<InteractionSettings>().equipPosition;
        }

        else
        {
			equippedObject.transform.localScale = equipReference.localScale;
            equippedObject.transform.position = equipReference.position;
        }

        equippedObject.GetComponentInChildren<InteractionSettings>().carryingObject = Services.Player.transform;
    }


    public void AbandonItem()
    {
        equippedObject.transform.SetParent(null);

        // Re-enable collision & stuff.
        equippedObject.GetComponent<Collider>().enabled = true;
        if (equippedObject.GetComponent<Rigidbody>() != null) equippedObject.GetComponent<Rigidbody>().isKinematic = false;
        equippedObject.transform.localScale = originalScale;

        equippedObject.GetComponent<Rigidbody>().AddForce(transform.forward * ASpeed);

        equippedObject.GetComponentInChildren<InteractionSettings>().carryingObject = null;
    }
}