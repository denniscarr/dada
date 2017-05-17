using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrailFunction : D_Function {

    bool readyToRunAway;

    new void Start()
    {
        base.Start();

        if (intSet.carryingObject == Services.Player)
        {
            Use();
        }
    }


    public void ReadyToRunAway()
    {
        if (readyToRunAway) return;

        Invoke("RunAway", 2f);
        readyToRunAway = true;
    }


    void RunAway()
    {
        GetDropped();

        //Run away from player.

        //Vector3 newPosition;
        Vector3 directionFromPlayer = transform.parent.position - Services.Player.transform.position;
        directionFromPlayer = directionFromPlayer.normalized;

        GetComponentInParent<Rigidbody>().AddForce(directionFromPlayer * 30000f, ForceMode.Impulse);

        Services.AudioManager.PlaySFX(Services.AudioManager.grailRejectionClip);

        if (Services.LevelGen.levelNum == -1) FindObjectOfType<Tutorial>().OnGrabGrail();
    }


    public override void Use()
    {
        base.Use();

        //if (Vector3.Distance(transform.position, Services.Player.transform.position) < 10f)
        //{
        //    Vector3 directionFromPlayer = transform.position - Services.Player.transform.position;
        //    directionFromPlayer = directionFromPlayer.normalized;

        //    GetComponent<Rigidbody>().MovePosition(transform.position + directionFromPlayer * 1.01f);
        //}
    }
}
