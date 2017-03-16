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

	public float displacementStrength = 20f;
	public float returnSmoothing = 0.01f;

	// Use this for initialization
	void Start () {


		parentTransform = gameObject.transform.parent;
		//Debug.Log (parentTransform.name);
		thisAnalyzer = gameObject.GetComponent<SpectrumAnalysis> ();
		thisMesh = gameObject.GetComponent<MeshFilter>().mesh;

		originalMesh = thisMesh.vertices;
		vertices = thisMesh.vertices;

	}

	// Update is called once per frame
	void Update () {
		float newSpectrumPoint = thisAnalyzer.bandBuffer [2];
		float otherSpectrumPoint = thisAnalyzer.bandBuffer [6];
		ReturnToOriginal ();
		for (int i = 0; i < originalMesh.Length; i++) {
			vertices [i] += Random.value > 0.5f ? thisMesh.normals[i] * newSpectrumPoint * displacementStrength : 
				thisMesh.normals[i] * otherSpectrumPoint * displacementStrength;
		}
		thisMesh.vertices = vertices;
		thisMesh.RecalculateBounds ();

	}

	void ReturnToOriginal() {
		for (int i = 0; i < originalMesh.Length; i++) {
			vertices [i] -= (vertices [i] - originalMesh [i]) * returnSmoothing;
		}
	}
}
