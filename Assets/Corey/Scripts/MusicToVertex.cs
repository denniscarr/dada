using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (SpectrumAnalysis))]
public class MusicToVertex : MonoBehaviour {

	Transform parentTransform;

	Mesh thisMesh;

	[SerializeField] SpectrumAnalysis thisAnalyzer;

	public Material audioReactiveMaterial;

	//float lastSpectrumPoint;
	float emissionLevel;


	[SerializeField] List<Renderer> listOfMeshRenderers; 

	public float displacementStrength = 20f;
	public float returnSmoothing = 0.01f;

	// Use this for initialization
	void Start () {

		listOfMeshRenderers = new List<Renderer> ();

		GetAllMeshRenderersInChildren (transform.parent.gameObject);

		foreach (Renderer renderer in listOfMeshRenderers) {
			renderer.material = audioReactiveMaterial;
			Debug.Log (renderer.gameObject.name);
		}
		parentTransform = gameObject.transform.parent;
		//Debug.Log (parentTransform.name);
		thisAnalyzer = gameObject.GetComponent<SpectrumAnalysis> ();
		//thisMesh = gameObject.GetComponent<MeshFilter>().mesh;

		//listOfMeshRenderers = gameObject.GetComponentsInChildren<Renderer> ();

	

	}

	// Update is called once per frame
	void FixedUpdate () {
		float newSpectrumPoint = thisAnalyzer.bandBuffer [2];
		float otherSpectrumPoint = thisAnalyzer.bandBuffer [3];

		foreach (Renderer thisRenderer in listOfMeshRenderers) {
			thisRenderer.material.SetFloat ("_Lacunarity", newSpectrumPoint * displacementStrength);
			thisRenderer.material.SetColor ("_Offset", new Color (newSpectrumPoint * displacementStrength * 0.25f, newSpectrumPoint * displacementStrength * 0.25f, 
				otherSpectrumPoint * displacementStrength * 0.5f, 0f));
			thisRenderer.material.SetFloat ("_Displacement", otherSpectrumPoint * displacementStrength * 0.1f);
		}

	}

	void GetAllMeshRenderersInChildren(GameObject targetObject)
	{
		// Get mesh renderers in children also.
		if (targetObject.transform.childCount > 0)
		{
			for (int i = 0; i < targetObject.transform.childCount; i++)
			{
				GetAllMeshRenderersInChildren(targetObject.transform.GetChild(i).gameObject);
			}
		}
		if (targetObject.GetComponent<Renderer>() != null)
		{
			
			listOfMeshRenderers.Add(targetObject.GetComponent<Renderer>());
		}
	}


}
