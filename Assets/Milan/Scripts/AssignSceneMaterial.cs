using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignSceneMaterial : MonoBehaviour {

	public Material mat;

	private List<Renderer> renderers;
	// Use this for initialization
	void Awake () {
		renderers = new List<Renderer> ();

		foreach (Renderer r in GameObject.FindObjectsOfType<Renderer>()) {
			renderers.Add (r);
		}
			
	}
	
	// Update is called once per frame
	void Update () {
		ResetTextures ();
	}

	void ResetTextures(){
		renderers[Random.Range(0, renderers.Count)].material = mat;
		renderers [Random.Range (0, renderers.Count)].material.color = new Color (Random.Range (0.00f, 1.00f), Random.Range (0.00f, 1.00f), Random.Range (0.00f, 1.00f));
		}
	}
