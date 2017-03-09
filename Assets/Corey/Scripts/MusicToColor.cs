﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Rudimentary Synaesthesia Thing
/// Requires a Parent with a Mesh Renderer and an AudioSource
/// </summary>
[RequireComponent (typeof (SpectrumAnalysis))]
public class MusicToColor : MonoBehaviour {

	Transform parentTransform;

	[SerializeField] SpectrumAnalysis thisAnalyzer;

	Material thisMaterial;

	//float lastSpectrumPoint;
	float emissionLevel;

	// Use this for initialization
	void Start () {
		parentTransform = gameObject.transform.parent;
		//Debug.Log (parentTransform.name);
		thisAnalyzer = gameObject.GetComponent<SpectrumAnalysis> ();
		thisMaterial = gameObject.GetComponent<MeshRenderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {
		float newSpectrumPoint = thisAnalyzer.spectrumData [15];
		emissionLevel += newSpectrumPoint;
		gameObject.GetComponent<MeshRenderer> ().material.SetColor("_EmissionColor", new Color (emissionLevel*50f, emissionLevel * 100f, emissionLevel * 75f));
		//Debug.Log(thisMaterial.GetColor("_Color"));
		emissionLevel *= 0.9f;

	}
}
