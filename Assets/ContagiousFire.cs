using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContagiousFire : MonoBehaviour {

    private void Update()
    {
        if (transform.parent.GetComponentInChildren<NPC>() != null)
        {
            transform.parent.GetComponentInChildren<NPC>().health -= 0.3f * Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponentInChildren<InteractionSettings>() != null)
        {
            other.GetComponentInChildren<InteractionSettings>().heat += 0.3f * Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        transform.parent.GetComponentInChildren<InteractionSettings>().onFire = false;
    }
}
