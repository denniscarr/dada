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
}
