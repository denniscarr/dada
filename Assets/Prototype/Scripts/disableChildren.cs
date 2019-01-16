using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableChildren : MonoBehaviour {
    public GameObject canvas;
	// Use this for initialization
	void Start () {
        for (int i = 0; i < canvas.transform.childCount; ++i)
        {
            canvas.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
