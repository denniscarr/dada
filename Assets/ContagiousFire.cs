using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContagiousFire : MonoBehaviour {

    private void Update()
    {
        if (transform.parent.GetComponentInChildren<NPC>() != null)
        {
            transform.parent.GetComponentInChildren<NPC>().health -= 20f * Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponentInChildren<InteractionSettings>() != null)
        {
            other.GetComponentInChildren<InteractionSettings>().heat += 1f * Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        transform.parent.GetComponentInChildren<InteractionSettings>().onFire = false;
    }
}
