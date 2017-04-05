using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCookie : MonoBehaviour {
	public Transform playerHeight;
	public bool useCookie = false;
	float initHeight;
	Sprite[] sprites;
	float cookieSize;
	Light light;
	public Gradient g;
	// Use this for initialization
	void Start () {
		light = GetComponent<Light> ();
		cookieSize = GetComponent<Light> ().cookieSize;
		sprites = Resources.LoadAll<Sprite>("");
		cookieSize = 1;
		initHeight = playerHeight.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		if(cookieSize > 100){
			cookieSize = 0;
		}
		cookieSize = Mathf.Lerp (0, 100, (playerHeight.position.y-initHeight)/100) + 1;

		light.cookieSize = cookieSize;
		light.color = g.Evaluate((playerHeight.position.y-initHeight)/100);
	}
}
