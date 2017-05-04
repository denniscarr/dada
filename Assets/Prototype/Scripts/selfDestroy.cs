using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestroy : MonoBehaviour {
	public float lifetime = 3f;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
