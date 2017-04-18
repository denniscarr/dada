using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grail : MonoBehaviour {
	
	void Update ()
    {
		// Suck all objects towards me.
        foreach(InteractionSettings interactionSettings in GameObject.FindObjectsOfType<InteractionSettings>())
        {
            if (interactionSettings.transform.parent.GetComponent<Rigidbody>() != null)
            {
                float distance = Vector3.Distance(transform.position, interactionSettings.transform.parent.position);
                if (distance < 200f)
                {
                    Vector3 direction = transform.position - interactionSettings.transform.parent.position;
                    direction = direction.normalized;
                    interactionSettings.transform.parent.GetComponent<Rigidbody>().AddForce(direction * MyMath.Map(distance, 20f, 100f, 0f, 1000f) * MyMath.Map(Mathf.PerlinNoise(Time.time, 0f), 0f, 1f, 0f, 3f), ForceMode.Acceleration);
                }
            }
        }
	}
}
