﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class refrigeratorFunction : MonoBehaviour {
    public GameObject refrigerator;
    public float refrigeratorSpeed = 10000f;
    public KeyCode useRefrigerator = KeyCode.Mouse0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (refrigerator.transform.parent != null && Input.GetKeyDown(useRefrigerator))
        {
            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().AddForce(transform.right * refrigeratorSpeed);
            transform.SetParent(null);
            refrigerator.transform.GetChild(2).gameObject.GetComponent<Collider>().enabled = true;
            Debug.Log("lol");
        }
	}
}
