using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grail : MonoBehaviour {
	
    bool dying = false;


	void Update ()
    {
        if (dying) return;

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

        // Run away from player.
        //if (Vector3.Distance(transform.position, Services.Player.transform.position) < 11f)
        //{
        //    Vector3 newPosition;
        //    Vector3 directionFromPlayer = transform.position - Services.Player.transform.position;
        //    directionFromPlayer = directionFromPlayer.normalized;

        //    GetComponent<Rigidbody>().AddForce(directionFromPlayer * 1000f, ForceMode.Impulse);
        //}

        //if (Vector3.Distance(transform.position, Services.Player.transform.position) < 10f)
        //{
        //    Vector3 directionFromPlayer = transform.position - Services.Player.transform.position;
        //    directionFromPlayer = directionFromPlayer.normalized;

        //    GetComponent<Rigidbody>().MovePosition(transform.position + directionFromPlayer * 1.01f);
        //}
    }


    public void GetReadyToDie()
    {
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 10f);
        dying = true;
    }
}
