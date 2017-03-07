﻿using UnityEngine;
using System.Collections;

public class SpectrumAnalysis : MonoBehaviour {

	//public SpectrumAnalysis instance;

	[HideInInspector]
	public float[] spectrumData;
	[HideInInspector]
	public AudioSource audioSource;




	void Awake(){
		/*Deleting Singleton Stuff for now
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);

		} else if (instance != this) {
			Destroy (gameObject);
		}
		*/
	}

	// Use this for initialization
	void Start () {
		spectrumData = new float[64];
		audioSource = gameObject.GetComponent<AudioSource> ();
		//StartCoroutine (GetSpectrumFrameChanges());
	}




	IEnumerator GetSpectrumFrameChanges(){
		//		while (true) {
		//			
		//			yield return new WaitForSeconds (0.05f);
		//		}
		return null;
	}

	public float GetWholeEnergy(){
		float wholeEnergy = 0;
		for (int i = 0; i < spectrumData.Length; i++) {
			wholeEnergy += spectrumData [i] * spectrumData [i];
		}

		wholeEnergy *= 200f;
		wholeEnergy = CompressorExciter (wholeEnergy, 0.05f, 0.7f);


		return wholeEnergy;


	}

	// Update is called once per frame
	void FixedUpdate () {
		audioSource.GetSpectrumData( spectrumData, 0, FFTWindow.Rectangular );
		float wholeEnergy = 0;
		for (int i = spectrumData.Length / 5 * 2; i < spectrumData.Length / 5 * 2 + spectrumData.Length / 2; i++) {
			wholeEnergy += spectrumData [i];
		}

		//Debug.Log (wholeEnergy);

		//Camera.main.backgroundColor =  HSL2RGB(Time.time / 10 - Mathf.FloorToInt(Time.time/10), 0.8f,lightNow);



	}

	public static float Compressor(float input, float threshold, float ratio, float gain){
		float t = input;
		t = threshold + (t - threshold) * ratio + gain;
		return t;
	}

	public static float CompressorExciter(float input, float threshold, float thresholdDB){


		if (1f - threshold <= 0f)
			return input;

		if (threshold == 0f)
			return input;

		float k = 0; float b = 0;
		if (input > threshold) {
			// k = (1- thresholdDB) / (1-threshold)
			// 1 * k + b = 1; b = 1 - k;

			k = (1f - thresholdDB) / (1f - threshold);
			b = 1f - k;

		} else {
			// k = (thresholdDB) / threshold
			k = thresholdDB / threshold;
			b = 0f;
		}
		return (input * k + b);
	}



}
