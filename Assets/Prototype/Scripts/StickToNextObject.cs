using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToNextObject : MonoBehaviour {

    public bool doIt = true;  // Whether I should stick to the next object I touch.

    private void Update()
    {
        // Flicker collision on and off to make sure it calls OnCollisionEnter (I tried OnCollisionStay but it was not working correctly.)
        GetComponent<Rigidbody>().detectCollisions = !GetComponent<Rigidbody>().detectCollisions;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("colliso" + doIt);

        if (doIt)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            doIt = false;
        }
    }

    private void OnDestroy()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
}
