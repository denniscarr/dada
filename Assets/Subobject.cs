using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subobject : MonoBehaviour {

    Mesh[] potentialForms;
    int numberOfForms = 5;

    Quaternion targetRotation;  // What I'll rotate towards.
    float rotateSpeed = 0.1f;   // Slerp speed for rotating.

    bool iGotToTheTop = false;  // Used for knowing where in the sine curve I am.
    Color topEmiss;
    float sineTime = 0.0f;
    float sineSpeed;
    

    private void Start()
    {
        // Get all the game objects in the scene right now.
        MeshFilter[] allTheMeshes = GameObject.FindObjectsOfType<MeshFilter>();

        // Get all the things so I can turn into them later.
        potentialForms = new Mesh[numberOfForms];
        for (int i = 0; i < numberOfForms; i++)
        {
            potentialForms[i] = allTheMeshes[Random.Range(0, allTheMeshes.Length)].sharedMesh;
        }

        // Turn into one of them now.
        ChangeForm();
    }


    private void Update()
    {
        // Sine it up.
        sineTime += sineSpeed * Time.deltaTime;

        // Get a new transparency based on sine.
        GetComponent<MeshRenderer>().material.color = new Color(0.01f, 0.01f, 0.01f, MyMath.Map(Mathf.Sin(sineTime), -1f, 1f, 0f, 1f));
        //GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color(
        //    MyMath.Map(Mathf.Sin(sineTime), -1f, 1f, 0f, topEmiss.r),
        //    MyMath.Map(Mathf.Sin(sineTime), -1f, 1f, 0f, topEmiss.g),
        //    MyMath.Map(Mathf.Sin(sineTime), -1f, 1f, 0f, topEmiss.b),
        //    MyMath.Map(Mathf.Sin(sineTime), -1f, 1f, 0f, 1f)));


        if (!iGotToTheTop && Mathf.Sin(sineTime) > 0f)
        {
            iGotToTheTop = true;
        }

        else if (iGotToTheTop && Mathf.Sin(sineTime) <= -0.999f)
        {
            ChangeForm();
        }

        // Rotate towards where I'm supposed to rotate towards.
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotateSpeed*Time.deltaTime);
    }


    void ChangeForm()
    {
        // Get a new mesh.
        GetComponent<MeshFilter>().mesh = potentialForms[Random.Range(0, potentialForms.Length)];

        ResizeMeshToUnit();

        // Get a new color
        //GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Random.ColorHSV());
        //GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color(0.1f, 0.1f, 0.1f, 0.01f));
        topEmiss = GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");

        // Get a new rotation
        transform.localRotation = Random.rotation;

        // Get a new target rotation
        targetRotation = Random.rotation;

        // Get a new scale and position.
        //transform.localPosition = Random.insideUnitSphere * Random.Range(0.5f, 1.1f);
        transform.localScale = new Vector3(Random.Range(1f, 10f), Random.Range(1f, 10f), Random.Range(1f, 10f));
        //Bounds newBounds = GetComponent<MeshFilter>().mesh.bounds;
        //newBounds.extents = new Vector3(Random.Range(20f, 25f), Random.Range(20f, 25f), Random.Range(20f, 25f));
        //GetComponent<MeshFilter>().sharedMesh.bounds = newBounds;

        // Get a new rotate speed.
        rotateSpeed = Random.Range(3f, 8f);

        sineSpeed = Random.Range(3f, 8f);

        iGotToTheTop = false;
    }


    void ResizeMeshToUnit()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf == null)
            return;
        Mesh mesh = mf.mesh;
        Bounds bounds = mesh.bounds;

        float size = bounds.size.x;
        if (size < bounds.size.y)
            size = bounds.size.y;
        if (size < bounds.size.z)
            size = bounds.size.z;

        if (Mathf.Abs(1.0f - size) < 0.01f)
        {
            //Debug.Log("Already unit size");
            return;
        }

        float scale = 1.0f / size;

        Vector3[] verts = mesh.vertices;

        for (int i = 0; i < verts.Length; i++)
        {
            verts[i] = verts[i] * scale;
        }

        mesh.vertices = verts;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

}
