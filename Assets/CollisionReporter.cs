using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionReporter : MonoBehaviour {

    public bool collidedWithSomethingAtLeastOnce;

    private void OnCollisionEnter(Collision collision)
    {
        BroadcastMessage("OnCollisionEnterParent", collision, SendMessageOptions.DontRequireReceiver);
        collidedWithSomethingAtLeastOnce = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        BroadcastMessage("OnTriggerEnterParent", other, SendMessageOptions.DontRequireReceiver);
    }
}
