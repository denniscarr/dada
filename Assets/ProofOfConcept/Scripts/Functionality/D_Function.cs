using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Function : MonoBehaviour {

    [HideInInspector] public InteractionSettings intSet;
    public KeyCode useKey = KeyCode.Mouse0;

    public void Start()
    {
        intSet = transform.parent.GetComponentInChildren<InteractionSettings>();
    }

    void Update()
    {
        // If we're being carried by the player and the player presses the use key then get used.
        if (intSet.carryingObject != null && intSet.carryingObject.name == "Player" && Input.GetKey(useKey))
        {
            Use();
        }
    }

    public virtual void Use()
    {

    }
}
