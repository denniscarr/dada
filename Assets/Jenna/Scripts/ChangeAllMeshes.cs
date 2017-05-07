using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAllMeshes : IncoherenceEvent {

    new void Start()
    {
        base.Start();

        threshold = 0.8f;
        instantaneous = true;
    }


    public override void Initiate()
    {
        base.Initiate();

        MeshFilter[] AllThings = Resources.LoadAll<MeshFilter>("");
        foreach (MeshFilter meshFilter in FindObjectsOfType<MeshFilter>())
        {
            Mesh chosenMesh = AllThings[Random.Range(0, AllThings.Length)].sharedMesh;
            meshFilter.mesh = chosenMesh;
        }
    }
}
