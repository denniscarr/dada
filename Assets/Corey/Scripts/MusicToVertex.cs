using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (SpectrumAnalysis))]
public class MusicToVertex : MonoBehaviour {

	Transform parentTransform;

	Mesh thisMesh;

	[SerializeField] SpectrumAnalysis thisAnalyzer;

	Material thisMaterial;

	//float lastSpectrumPoint;
	float emissionLevel;

	//array of original Mesh vertices
	Vector3 [] originalMesh;

	//array of mesh vertices
	Vector3 [] vertices;

	Renderer thisRenderer; 

	public float displacementStrength = 20f;
	public float returnSmoothing = 0.01f;

	// Use this for initialization
	void Start () {


		parentTransform = gameObject.transform.parent;
		//Debug.Log (parentTransform.name);
		thisAnalyzer = gameObject.GetComponent<SpectrumAnalysis> ();
		thisMesh = gameObject.GetComponent<MeshFilter>().mesh;

		thisRenderer = gameObject.GetComponent<Renderer> ();

	

	}

	// Update is called once per frame
	void FixedUpdate () {
		float newSpectrumPoint = thisAnalyzer.bandBuffer [2];
		float otherSpectrumPoint = thisAnalyzer.bandBuffer [3];

		thisRenderer.material.SetFloat ("_Lacunarity", newSpectrumPoint * 20f);
		thisRenderer.material.SetColor ("_Offset", new Color (newSpectrumPoint * 5f, newSpectrumPoint * 5f, 
			otherSpectrumPoint * 10f, 0f));
		thisRenderer.material.SetFloat ("_Displacement", otherSpectrumPoint * 20f);

	}

	void ReturnToOriginal() {
		for (int i = 0; i < originalMesh.Length; i++) {
			vertices [i] -= (vertices [i] - originalMesh [i]) * returnSmoothing;
		}
	}
}
