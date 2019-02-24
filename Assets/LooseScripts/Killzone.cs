using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        //If the grail has fallen off the level, kill it and turn the lights on.
        if (other.gameObject.name.ToLower().Contains("grail"))
        {
            //Services.LevelGen.Create();
            other.GetComponent<Grail>().GetReadyToDie();
        }

        // If this object is not the grail, put it back on the level.
        if ((other.GetComponentInChildren<InteractionSettings>() != null && !other.GetComponentInChildren<InteractionSettings>().IsEquipped) || (Services.LevelGen.levelNum == -1 && other.gameObject == Services.Player))
        {
            //Debug.Log("Killzone wrapped " + other.name);
            float currentLevelRadius = Services.LevelGen.currentLevel._width * Services.LevelGen.tileScale;
            other.transform.position = Services.LevelGen.currentLevel.transform.position;
            other.transform.position += new Vector3(Random.Range(-currentLevelRadius * 0.4f, currentLevelRadius * 0.4f), 150f, Random.Range(-currentLevelRadius*0.4f, currentLevelRadius*0.4f));
            if (other.GetComponent<Rigidbody>() != null) other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
