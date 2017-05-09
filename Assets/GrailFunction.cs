using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrailFunction : D_Function {


    new void Start()
    {
        base.Start();

        intSet.isOwnedByPlayer = true;

        if (intSet.carryingObject == Services.Player)
        {
            Use();
        }
    }


    public override void Use()
    {
        base.Use();

        GetDropped();

        //Run away from player.

        //Vector3 newPosition;
        Vector3 directionFromPlayer = transform.parent.position - Services.Player.transform.position;
        directionFromPlayer = directionFromPlayer.normalized;

        GetComponentInParent<Rigidbody>().AddForce(directionFromPlayer * 20000f, ForceMode.Impulse);

		Services.AudioManager.PlaySFX (Services.AudioManager.grailRejectionClip);

        //if (Vector3.Distance(transform.position, Services.Player.transform.position) < 10f)
        //{
        //    Vector3 directionFromPlayer = transform.position - Services.Player.transform.position;
        //    directionFromPlayer = directionFromPlayer.normalized;

        //    GetComponent<Rigidbody>().MovePosition(transform.position + directionFromPlayer * 1.01f);
        //}
    }
}
