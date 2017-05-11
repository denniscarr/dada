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
	public Color originalLowColor;
	float originalLacunarity;
	//float originalDisplacement;

	// Use this for initialization
	void Start () {

		listOfMeshRenderers = new List<Renderer> ();

		GetAllMeshRenderersInChildren (transform.parent.gameObject);
		 

		foreach (Renderer renderer in listOfMeshRenderers) {
            if (renderer.GetComponent<ParticleSystem>() == null)
            {
                renderer.material = audioReactiveMaterial;
                originalLacunarity = audioReactiveMaterial.GetFloat("_Lacunarity");
                originalLowColor = audioReactiveMaterial.GetColor("_LowColor");
            }
			//Debug.Log (renderer.gameObject.name);
		}
		parentTransform = gameObject.transform.parent;
		//Debug.Log (parentTransform.name);
		thisAnalyzer = gameObject.GetComponent<SpectrumAnalysis> ();
		//thisMesh = gameObject.GetComponent<MeshFilter>().mesh;

		//listOfMeshRenderers = gameObject.GetComponentsInChildren<Renderer> ();

	

	}

	// Update is called once per frame
	void FixedUpdate () {
		float spectrumPointOne = thisAnalyzer.bandBuffer [2];
		float spectrumPointTwo = thisAnalyzer.bandBuffer [3];
		float spectrumPointThree = thisAnalyzer.bandBuffer [4];

		foreach (Renderer thisRenderer in listOfMeshRenderers) {
			if (thisRenderer != null) {
				thisRenderer.material.SetFloat ("_Lacunarity", originalLacunarity + spectrumPointOne * displacementStrength);
				thisRenderer.material.SetColor ("_Offset", new Color (spectrumPointOne * displacementStrength * 0.25f, spectrumPointOne * displacementStrength * 0.25f, 
					spectrumPointTwo * displacementStrength * 0.5f, 0f));
				thisRenderer.material.SetColor ("_LowColor", new Color (originalLowColor.r + spectrumPointOne, originalLowColor.g + spectrumPointTwo,
					originalLowColor.b + spectrumPointThree, 1.0f));
				thisRenderer.material.SetFloat ("_Displacement", spectrumPointTwo * displacementStrength * 0.1f);
			}
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
