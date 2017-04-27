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
        else if (other == Services.Player)
        {
            other.transform.position = Services.LevelGen.currentLevel.transform.position;
            other.transform.Translate(Random.Range(-20f, 20f), 40f, Random.Range(-20f, 20f));
        }
    }
}
