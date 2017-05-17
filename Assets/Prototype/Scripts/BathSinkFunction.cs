using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathSinkFunction : MonoBehaviour {
   // public GameObject bathSink;
    public GameObject explosionParticle;
    float radius = 30.0F;
    float power = 200.0F;
    public float fuseTime = 5f;
    public float bathSinkSpeed = 100f;
    public KeyCode useBathSink = KeyCode.Mouse0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (/*bathSink.transform.parent != null &&*/ Input.GetKeyDown(useBathSink))
        {
            //enable essential components once thrown
            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().AddForce(transform.right * bathSinkSpeed);
			transform.parent.position = GameObject.Find("Equip Reference").transform.position+transform.GetComponentInParent<InteractionSettings>().equipPosition;
			transform.parent.rotation = Quaternion.LookRotation(GameObject.Find("Player").transform.forward);
            //transform.SetParent(null);
            Invoke("Explosion", fuseTime);
        }
	}

    void Explosion()
    {
		
        //Instantiate particle system and add force

		Vector3 explosionPos = transform.position;
			

		Instantiate(explosionParticle, explosionPos, Quaternion.identity);
        
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            if (hit.GetComponentInChildren<NPC>() != null)
            {
                hit.GetComponentInChildren<NPC>().health -= 50f;
            }

            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F, ForceMode.Impulse);

        }
    }
}

