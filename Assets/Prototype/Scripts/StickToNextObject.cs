using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToNextObject : MonoBehaviour {

    public bool doIt;  // Whether I should stick to the next object I touch.
    public bool dropping = true;

    void Update()
    {
        if (doIt && !dropping)
        {
            dropping = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
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
