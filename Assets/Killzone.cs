using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        other.transform.position = Services.LevelGen.currentLevel.transform.position;
        other.transform.Translate(Random.Range(-10f, 10f), 40f, Random.Range(-10f, 10f));
    }
}
