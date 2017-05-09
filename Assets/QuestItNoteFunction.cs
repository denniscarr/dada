using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class QuestItNoteFunction : D_Function {

	public int questID;
    public bool useOnStart = false; // Whether I should stick to something as soon as I spawn.

    bool carriedByPlayerInLastFrame;

    bool waitToDelete;

	// Use this for initialization
	new void Start () {
		base.Start();

        if (transform.parent.name.Contains("(Clone)"))
        {
            //Debug.Log("Renaming Quest it note");
            transform.parent.name = transform.parent.name.Remove(transform.parent.name.Length - 7, 7);
        }

        // These should always be owned by the player & free.
        intSet.isOwnedByPlayer = true;
        intSet.price = 0;

        if (useOnStart)
        {
            Use();
        }
	}
	
	// Update is called once per frame
	new void Update () {

		base.Update ();

        if (waitToDelete && transform.parent.localScale.y <= 0.0001f)
        {
            Destroy(transform.parent.gameObject);
        }

        // See if I get dropped.
        if (carriedByPlayerInLastFrame && intSet.carryingObject != Services.Player.transform)
        {
            FacePlayer();
        }

        if (intSet.carryingObject == Services.Player.transform)
        {
            carriedByPlayerInLastFrame = true;
        }

        else
        {
            carriedByPlayerInLastFrame = false;
        }

        //If the player is holding me, turn to face the player.
        if (intSet.carryingObject != null && intSet.carryingObject == Services.Player.transform)
        {
            // Unparent from whatever object I was parented to before.
            if (transform.parent.parent != null && transform.parent.parent.name != "Equip Reference" && !intSet.IsInVisor)
            {
                transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                transform.parent.parent = null;
            }

            // Face player.
            //if (transform.parent.parent == null) return;

            //Quaternion newRotation = transform.parent.parent.rotation;
            //transform.parent.rotation = newRotation;

            //if (!intSet.IsInVisor)
            //{
            //    newRotation = transform.parent.parent.rotation;
            //    newRotation = Quaternion.LookRotation(transform.parent.position - Services.Player.GetComponentInChildren<Camera>().transform.position);
            //    transform.parent.rotation = newRotation;
            //    //transform.parent.localEulerAngles = new Vector3(0f, 80f, 0f);
            //}
        }
    }

    void FacePlayer()
    {
        Quaternion newRotation = Quaternion.identity;
        if (intSet.IsInVisor)
        {
            newRotation = Quaternion.LookRotation(transform.parent.position - GameObject.Find("UpperCamera").GetComponent<Camera>().transform.position);
        }

        else
        {
            newRotation = Quaternion.LookRotation(transform.parent.position - Services.Player.GetComponentInChildren<Camera>().transform.position);
        }

        transform.parent.rotation = newRotation;
    }

    public void StickToScreen()
    {
        Transform visorNode = GameObject.Find("INROOMOBJECTS").transform;
        transform.parent.parent = visorNode.transform;
        transform.parent.localPosition = new Vector3(
            Random.Range(-1f, 3.8f),
            Random.Range(1.5f, 3.6f),
            2.5f);

        transform.parent.localRotation = Quaternion.Euler(new Vector3(
            0f,
            0f,
            Random.Range(-1f, 1f)));

        Freeze();
    }

    public void Freeze()
    {
        transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        //// Move my position to the preordained position.
        //transform.parent.localPosition = new Vector3(transform.parent.position.x, transform.parent.position.y, 2.5f);

        //// Move my rotation the the Good rotation.
        //transform.parent.localRotation = Quaternion.Euler(new Vector3(0f, 0f, transform.parent.localRotation.z));
    }

    public override void Use ()
	{
		base.Use ();

        // Raycast from my butt to the next surface.
        RaycastHit hit;
        Debug.DrawRay(transform.parent.position, transform.parent.forward * 5f, Color.red, 1f);
        if (Physics.Raycast(transform.parent.position, transform.parent.forward, out hit, 30f))
        {
            if (!useOnStart) GetDropped();
            transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            // Move my position to the point of ray hit
            transform.parent.position = hit.point;

            transform.parent.SetParent(hit.collider.gameObject.transform);

            // Rotate me to the normal of the hit so it looks like I'm sticking to the thingy I hit.
            if (hit.collider.name == "UpperNode" || hit.collider.name.Contains("QuestItNote"))
            {
                transform.parent.localRotation = Quaternion.identity;
            }

            else
            {
                transform.parent.rotation = Quaternion.FromToRotation(transform.parent.forward, hit.normal);
            }  
        }
    }


    /// <summary>
    /// Should be called when the quest is destroyed because the player went to a new level without finishing it.
    /// </summary>
    public void GetDestroyedNormal()
    {
        transform.parent.DOScale(Vector3.zero, 0.4f);

        waitToDelete = true;
    }

    
    /// <summary>
    /// Should be called when the quest is destroyed because the player completed it.
    /// </summary>
    public void GetDestroyedFlashy()
    {
        transform.parent.DOScale(Vector3.zero, 0.4f);
        GameObject stars = Instantiate(Resources.Load("explosion-noforce", typeof(GameObject))) as GameObject;
        stars.transform.position = transform.parent.position;

        waitToDelete = true;
    }
}
