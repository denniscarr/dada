using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItNoteFunction : D_Function {

	public int questID;
    public bool useOnStart = false; // Whether I should stick to something as soon as I spawn.

	// Use this for initialization
	new void Start () {
		base.Start();

        if (useOnStart)
        {
            Use();
        }
	}
	
	// Update is called once per frame
	new void Update () {

		base.Update ();

        //If the player is holding me, turn to face the player.
        if (intSet.carryingObject == Services.Player.transform)
        {
            // Unparent from whatever object I was parented to before.
            if (transform.parent.parent != null && transform.parent.parent.name != "Equip Reference" && !intSet.IsInVisor)
            {
                transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                transform.parent.parent = null;
            }

            // Face player.
            if (transform.parent.parent == null) return;

            Quaternion newRotation = transform.parent.parent.rotation;
            transform.parent.rotation = newRotation;

            if (!intSet.IsInVisor)
            {
                //newRotation = transform.parent.parent.rotation;
                //newRotation = Quaternion.LookRotation(transform.parent.position - Services.Player.GetComponentInChildren<Camera>().transform.position);
                //transform.parent.rotation = newRotation;
                transform.parent.localEulerAngles = new Vector3(0f, 80f, 0f);
            }

        }
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
}
