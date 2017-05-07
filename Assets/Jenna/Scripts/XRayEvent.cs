using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRayEvent : IncoherenceEvent {

    // Briefly turns all meshes into skeletons.

    Mesh skelly;

	new void Start ()
    {
        base.Start();

        threshold = 0.7f;
        instantaneous = true;

        GameObject skel = Instantiate(Resources.Load("NPCs/Skeleton")) as GameObject;
        skelly = skel.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
        DestroyImmediate(skel);
	}


    public override void Initiate()
    {
        base.Initiate();

        foreach (MeshFilter meshFilter in FindObjectsOfType<MeshFilter>())
        {
            meshFilter.mesh = skelly;
        }
    }
}
