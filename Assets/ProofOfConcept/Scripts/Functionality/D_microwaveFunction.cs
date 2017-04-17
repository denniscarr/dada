using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_microwaveFunction : D_Function {
	//public GameObject ringWave;
	private ParticleSystem ring;
	//public Transform emitter;
	// Use this for initialization
	new void Start () {
		base.Start ();
		ring = GetComponent<ParticleSystem> ();
		var em = ring.emission;
		em.enabled = false;
	}

    new void Update()
    {
        base.Update();

        var em = ring.emission;

        if (em.enabled)
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.parent.position, 2f, transform.parent.right, 5f);
            Debug.DrawRay(transform.parent.position, transform.parent.right * 2f, Color.cyan);
            foreach (RaycastHit hit in hits)
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.GetComponentInChildren<InteractionSettings>() != null)
                {
                    Debug.Log("heating up: " + hit.collider.gameObject.name);
                    hit.collider.GetComponentInChildren<InteractionSettings>().heat += 0.4f * Time.deltaTime;
                }
            }
        }
    }
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		//Instantiate (ringWave, emitter.transform.right, Quaternion.identity);
		var em = ring.emission;
		em.enabled = !em.enabled;
	}
}
