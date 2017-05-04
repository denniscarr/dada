using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionReporterNPC : MonoBehaviour {

    NPC npcScript;
    public bool collidedWithSomethingAtLeastOnce;

    private void Start()
    {
        npcScript = GetComponentInChildren<NPC>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // See if this object is no longer an NPC, if so just kill myself.
        if (npcScript == null)
        {
            Destroy(this);
            return;
        }

        npcScript.CollisionInParent(collision);
    }
}
