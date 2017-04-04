using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToNextObject : MonoBehaviour {

    public bool doIt = true;  // Whether I should stick to the next object I touch.


    void OnCollisionStay(Collision collision)
    {
        if (doIt)
        {
            Debug.Log("Colliso");
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            doIt = false;
        }
    }

    private void OnDestroy()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
}
