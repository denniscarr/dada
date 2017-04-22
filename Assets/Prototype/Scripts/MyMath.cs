using UnityEngine;
using System.Collections;

public class MyMath {

	public static float Map(float x, float in_min, float in_max, float out_min, float out_max)
	{
		return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
	}

    public static float LargestCoordinate(Vector3 vector)
    {
        float[] coordinates = new float[3];
        coordinates[0] = vector.x;
        coordinates[1] = vector.y;
        coordinates[2] = vector.z;

        float largest = 0f;

        for (int i = 0; i < coordinates.Length; i++)
        {
            if (Mathf.Abs(coordinates[i]) > largest)
            {
                largest = Mathf.Abs(coordinates[i]);
            }
        }

        return largest;
    }


    public static void ResizeMeshToUnit(Mesh mesh)
    {
        ResizeMesh(mesh, 1f);
    }


    public static void ResizeMesh(Mesh mesh, float targetScale)
    {
        Bounds bounds = mesh.bounds;

        float size = bounds.size.x;
        if (size < bounds.size.y)
            size = bounds.size.y;
        if (size < bounds.size.z)
            size = bounds.size.z;

        //if (Mathf.Abs(1.0f - size) < 0.01f)
        //{
            //Debug.Log("Already unit size");
            //return;
        //}

        float scale = targetScale / size;

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
