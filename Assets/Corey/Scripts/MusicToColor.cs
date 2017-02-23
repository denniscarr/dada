using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rudimentary Synaesthesia Thing
/// Requires a Parent with a Mesh Renderer and an AudioSource
/// </summary>

public class MusicToColor : MonoBehaviour {

	Transform parentTransform;

	[SerializeField] SpectrumAnalysis thisAnalyzer;

	Material thisMaterial;

	// Use this for initialization
	void Start () {
		parentTransform = gameObject.transform.parent;
		//Debug.Log (parentTransform.name);
		thisAnalyzer = gameObject.GetComponent<SpectrumAnalysis> ();
		thisMaterial = parentTransform.gameObject.GetComponent<MeshRenderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {

		parentTransform.gameObject.GetComponent<MeshRenderer> ().material.SetColor("_Color", new Color (thisAnalyzer.spectrumData [4] * 100f, thisAnalyzer.spectrumData [8] * 100f, thisAnalyzer.spectrumData [12] * 100f));
		//Debug.Log(thisMaterial.GetColor("_Color"));
	}
}
