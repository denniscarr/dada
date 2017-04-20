using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientMusic : MonoBehaviour {

	public AudioSource[] hiSource; 
	public AudioSource[] loSource;
	public AudioSource[] ambienceSource;

	float lHue, lSat, lVal;

	float hue;

	Color currentLevelColor;

	public float fadeOutTime = 5.0f;

	[SerializeField] float crossfadeThreshold;



	// Use this for initialization
	void Start () {

		currentLevelColor = Services.LevelGen.currentLevel.levelTint;

		Color.RGBToHSV( currentLevelColor,out hue, out lSat,out lVal);

		Debug.Log (hue);

		for( int i = 0; i < hiSource.Length; i ++ ) {

			float loBound = ((float)i * (1f / (float)hiSource.Length));
			float hiBound = ((float)(i + 1f) * (1f / (float)hiSource.Length));

			if (hue >= loBound && hue < hiBound) {
				Debug.Log("range is greater than: " + loBound + "and less than " + hiBound);
				hiSource [i].volume = 1.0f;
			} else {
				hiSource [i].volume = 0.0f;
			}

		}
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Services.LevelGen.currentLevel.levelTint != currentLevelColor) {

			Color.RGBToHSV( Services.LevelGen.currentLevel.levelTint, out hue, out lSat, out lVal);


			for( int i = 0; i < hiSource.Length; i ++ ) {

				if (hue >= ((float)i * (1.0f / (float)hiSource.Length)) && hue <= ((float)(i + 1f) * (1f / (float)hiSource.Length))) {
					hiSource [i].volume = 1.0f;
				} else {
					hiSource [i].volume = 0.0f;
				}

			}

		}

	}



}
