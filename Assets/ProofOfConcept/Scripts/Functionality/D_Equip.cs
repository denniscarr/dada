﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////////
// OBSOLETE SCRIPT. THIS FUNCTIONALITY HAS BEEN TRANSFERRED TO 'EquippableFinder' //
////////////////////////////////////////////////////////////////////////////////////

public class D_Equip : MonoBehaviour {

    Writer writer;

	//no long use during preproduction stage
//	public KeyCode equipKey = KeyCode.Mouse0;
//	public KeyCode abandonKey = KeyCode.Mouse0;

	private bool readyToEquip = false;
    Transform equipReference;

    InteractionSettings intSet;

    // Related to the throw force.
    public float speed = 1;
	public float ASpeed = 10;

    Vector3 originalScale;


    void Start ()
    {
        // Get references to my buddies.
        writer = GetComponent<Writer>();
        equipReference = GameObject.Find("New Equip Reference").transform;
        intSet = transform.parent.GetComponentInChildren<InteractionSettings>();
        originalScale = transform.parent.localScale;
    }


    void OnTriggerExit(Collider collider)
    {
		readyToEquip = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        //if (collider.GetComponent<NPC>() == null && collider.name != "Player") return;

		if (collider.name == "Player") {
            if (writer != null) writer.WriteSpecifiedString("Press E to equip " + transform.parent.name + ".");
            readyToEquip = true;
		}

    }

    // Update is called once per frame
    void Update ()
    {
		if (intSet.carryingObject == Services.Player.transform && Input.GetMouseButtonDown(0))
		{
			abandonItem ();
			intSet.carryingObject = null;
		}
		else if (readyToEquip == true && Input.GetMouseButtonDown(0))
        {
            MoveToCamera();
			intSet.carryingObject = GameObject.Find("Player").transform;
        }
			
    }

    void MoveToCamera ()
    {
        // Disable collision & gravity.
		//GetComponentInParent<Collider>().enabled = false;
       // GetComponent<Collider>().enabled = false;
		if (GetComponentInParent<Rigidbody>() != null) GetComponentInParent<Rigidbody>().isKinematic = true;

        // Set position & parentage.
		transform.parent.position = equipReference.position;
		transform.parent.rotation = equipReference.rotation;
		transform.parent.SetParent(equipReference, true);
    }

	public void abandonItem ()
	{
        transform.parent.SetParent(null);

        // Re-enable collision & stuff.
        GetComponentInParent<Collider>().enabled = true;
        GetComponent<Collider>().enabled = true;
		if (GetComponentInParent<Rigidbody>() != null) GetComponentInParent<Rigidbody>().isKinematic = false;
        transform.parent.localScale = originalScale;

		GetComponentInParent<Rigidbody>().AddForce(transform.forward * ASpeed);
	}
    
}
