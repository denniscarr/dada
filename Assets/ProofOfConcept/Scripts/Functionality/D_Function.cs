using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Function : MonoBehaviour {

    public InteractionSettings intSet;
    public KeyCode useKey = KeyCode.Mouse0;

    public void Start()
    {
        intSet = transform.parent.GetComponentInChildren<InteractionSettings>();
    }

    public void Update()
    {
		if (intSet.carryingObject == Services.Player.transform)
		{
			Debug.Log ("Held by player");
		}

        // If we're being carried by the player and the player presses the use key then get used.
		if (intSet.carryingObject == Services.Player.transform && Input.GetKey(useKey))
        {
            Use();
        }
    }

    public virtual void Use()
    {

    }
}
