using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopFunction : D_Function {

    [SerializeField] GameObject particles;

    new void Start()
    {
        base.Start();
    }

    public override void Use()
    {
        base.Use();

        Instantiate(particles, transform.position, transform.rotation);
    }
}
