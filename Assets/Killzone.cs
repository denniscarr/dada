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
        else
        {
            Debug.Log(other.name);
            float currentLevelRadius = Services.LevelGen.currentLevel._width * Services.LevelGen.tileScale;
            other.transform.position = Services.LevelGen.currentLevel.transform.position;
            other.transform.Translate(Random.Range(-currentLevelRadius, currentLevelRadius), 25f, Random.Range(-currentLevelRadius, currentLevelRadius));
        }
    }
}
