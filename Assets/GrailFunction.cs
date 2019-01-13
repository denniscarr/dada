using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrailFunction : D_Function {

    bool readyToRunAway;
    bool tutorialMessageSent;
    //bool functionsEnabled;
    //D_Function[] functions;


    //void Awake()
    //{
    //    functions = transform.parent.GetComponentsInChildren<D_Function>();
    //    foreach (D_Function function in functions)
    //    {
    //        if (!function.gameObject.name.Contains("Grail Function"))
    //        {
    //            Debug.Log("Disabling: " + function.gameObject.name);
    //            function.gameObject.SetActive(false);
    //        }
    //    }
    //}


    new void Start()
    {
        base.Start();

        //if (intSet.carryingObject == Services.Player)
        //{
        //    Use();
        //}
    }


    new void Update()
    {
        base.Update();
        //if (intSet.isOwnedByPlayer && !functionsEnabled)
        //{
        //    Debug.Log("Enabling grail functionality.");

        //    foreach(D_Function function in functions)
        //    {
        //        if (!function.gameObject.name.Contains("Grail Function"))
        //        {
        //            Debug.Log("Enabling " + function.gameObject.name + ".");
        //            function.gameObject.SetActive(true);
        //        }
        //    }

        //    functionsEnabled = true;
        //}
    }


    public void ReadyToRunAway()
    {
        if (intSet.isOwnedByPlayer) return;
        if (readyToRunAway) return;

        Invoke("RunAway", 1f);
        readyToRunAway = true;
    }


    void RunAway()
    {
        if (intSet.carryingObject != null && intSet.carryingObject == Services.Player.transform) return;
        if (intSet.isOwnedByPlayer) return;

        //Vector3 newPosition;
        Vector3 directionFromPlayer = transform.parent.position - Services.Player.transform.position;
        directionFromPlayer = directionFromPlayer.normalized;

        foreach (Collider collider in GetComponentsInParent<Collider>())
        {
            collider.enabled = false;
        }

        GetComponentInParent<Rigidbody>().AddForce(directionFromPlayer * 30000f, ForceMode.Impulse);

        Services.AudioManager.PlaySFX(Services.AudioManager.grailRejectionClip);

        //readyToRunAway = false;

        if (Services.LevelGen.levelNum == -1)
        {
            if (tutorialMessageSent) return;
            FindObjectOfType<Tutorial>().OnGrabGrail();
            tutorialMessageSent = true;
        }
    }


    public override void Use()
    {
        base.Use();

        Debug.Log("using grail");

        foreach(MeshRenderer mr in FindObjectsOfType<MeshRenderer>())
        {
            mr.material = transform.parent.GetComponentInChildren<MeshRenderer>().material;
        }

        //InteractionSettings[] futureGrails = FindObjectsOfType<InteractionSettings>();
        //for (int i = 0; i < futureGrails.Length; i++)
        //{
        //    futureGrails[i].transform.parent.gameObject.AddComponent<Grail>();
        //    GameObject particles = Instantiate(transform.parent.FindChild("FireComplex").gameObject);
        //    particles.transform.parent = futureGrails[i].transform.parent;
        //    particles.transform.localPosition = Vector3.zero;
        //    GameObject grailFunction = Instantiate(gameObject);
        //    grailFunction.transform.parent = futureGrails[i].transform.parent;
        //    grailFunction.transform.position = Vector3.zero;
        //}

        GameObject[] futureGrails = FindObjectsOfType<GameObject>();
        for (int i = 0; i < futureGrails.Length; i++)
        {
            futureGrails[i].transform.gameObject.AddComponent<Grail>();
            GameObject particles = Instantiate(transform.parent.Find("FireComplex").gameObject);
            particles.transform.parent = futureGrails[i].transform;
            particles.transform.localPosition = Vector3.zero;
            GameObject grailFunction = Instantiate(gameObject);
            grailFunction.transform.parent = futureGrails[i].transform;
            grailFunction.transform.position = Vector3.zero;
        }

        //Services.IncoherenceManager.globalIncoherence = 0;
        //Services.LevelGen.Create();

        //if (Vector3.Distance(transform.position, Services.Player.transform.position) < 10f)
        //{
        //    Vector3 directionFromPlayer = transform.position - Services.Player.transform.position;
        //    directionFromPlayer = directionFromPlayer.normalized;

        //    GetComponent<Rigidbody>().MovePosition(transform.position + directionFromPlayer * 1.01f);
        //}
    }
}
