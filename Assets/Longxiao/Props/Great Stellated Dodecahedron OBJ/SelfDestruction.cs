using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SelfDestruction : MonoBehaviour {
	private float lifetime = 25f;
	private Vector3 zero; 
	// Use this for initialization
	void Start () {
		zero = new Vector3 (0, 0, 0);
		Invoke ("Disappear", 23f);
        Destroy(gameObject, lifetime);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Disappear () {
		gameObject.transform.DOScale (zero, 2f);
	}
}
