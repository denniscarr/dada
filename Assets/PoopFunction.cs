using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopFunction : D_Function {

    [SerializeField] GameObject particles;
    [SerializeField] Material poopMat;

    new void Start()
    {
        base.Start();

        //poopMat = Resources.Load("poop") as Material;
    }

    public override void Use()
    {
        base.Use();

        RaycastHit[] hits = Physics.SphereCastAll(transform.parent.position, 2f, transform.parent.forward, 5f);
        foreach (RaycastHit hit in hits)
        {
            if (!hit.collider.gameObject.name.Contains("Feces-Covered") && hit.collider.gameObject.name != "GROUND")
            {
                hit.collider.gameObject.name = "Feces-Covered " + hit.collider.gameObject.name;
            }

            if (hit.collider.GetComponentInChildren<Renderer>() != null)
            {
                hit.collider.GetComponentInChildren<Renderer>().material = poopMat;
            }
        }
        Instantiate(particles, transform.position, transform.rotation);
    }
}
