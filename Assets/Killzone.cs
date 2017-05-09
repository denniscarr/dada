using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        // If the grail has fallen off the level, spawn a new grail.
        if (other.name == "Grail")
        {
            Services.LevelGen.Create();
            other.GetComponent<Grail>().GetReadyToDie();
        }

        // If this object is not the grail, put it back on the level.
        else if ((other.GetComponentInChildren<InteractionSettings>() != null && !other.GetComponentInChildren<InteractionSettings>().IsEquipped) || other.gameObject == Services.Player)
        {
            //Debug.Log(other.name);
            float currentLevelRadius = Services.LevelGen.currentLevel._width * Services.LevelGen.tileScale;
            other.transform.position = Services.LevelGen.currentLevel.transform.position;
            other.transform.position += new Vector3(Random.Range(-currentLevelRadius * 0.4f, currentLevelRadius * 0.4f), 75f, Random.Range(-currentLevelRadius*0.4f, currentLevelRadius*0.4f));
            if (other.GetComponent<Rigidbody>() != null) other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
