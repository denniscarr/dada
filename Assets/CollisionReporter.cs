using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionReporter : MonoBehaviour {

    public bool collidedWithSomethingAtLeastOnce;

    private void OnCollisionEnter(Collision collision)
    {
        collidedWithSomethingAtLeastOnce = true;
    }

}
