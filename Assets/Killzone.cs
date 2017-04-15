using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        other.transform.position = Services.LevelGen.currentLevel.transform.position;
        other.transform.Translate(0f, -50f, 0f);
    }
}
