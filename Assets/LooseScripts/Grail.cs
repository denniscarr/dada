using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grail : MonoBehaviour {
	
    bool dying = false;

    [SerializeField] float pulsateSpeedFast = 5f;
    Vector3 originalScale;
    float sine;

    
    private void Start()
    {
        originalScale = Vector3.one * 5f;
    }


    void Update ()
    {
        if (dying) return;

        // Pulsate at speed based on distance to player.
        float pulsateSpeed = MyMath.Map(
            Vector3.Distance(transform.position, Services.Player.transform.position),
            5f,
            Services.LevelGen.currentLevel._width*0.5f,
            1f,
            pulsateSpeedFast
            );

        pulsateSpeed = Mathf.Clamp(pulsateSpeed, 0f, pulsateSpeedFast);

        sine += pulsateSpeedFast * Time.deltaTime;

        Vector3 newScale = new Vector3(
            MyMath.Map(Mathf.Sin(sine), -1f, 1f, originalScale.x * 0.9f, originalScale.x * 1.1f),
            MyMath.Map(Mathf.Sin(sine), -1f, 1f, originalScale.y * 0.9f, originalScale.y * 1.1f),
            MyMath.Map(Mathf.Sin(sine), -1f, 1f, originalScale.y * 0.9f, originalScale.z * 1.1f)
            );

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("Subobject"))
            {
                transform.GetChild(i).localScale = newScale;
            }
        }

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
        Debug.Log("grail being destroyed");
        GetComponent<Collider>().enabled = false;
        Services.IncoherenceManager.timeInLevel = 1000000f;
        FindObjectOfType<GrailSpawner>().TurnOnTheLights();
        Destroy(gameObject, 1f);
        dying = true;
    }
}
