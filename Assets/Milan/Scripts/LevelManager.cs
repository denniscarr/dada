using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SimpleManager.Manager<Level> {

	public Level currentLevel;
	public int width, length, height;
	public float tileScale = 1;
	public float[] NoiseRemapping;
	public float perlinFrequency = 0.02f;
	private float xOffset, yOffset;
	private Texture2D[] maps;
	private Gradient gradient;

	void Start(){
		maps = Resources.LoadAll<Texture2D> ("maps") as Texture2D[];

		Level.xOrigin = Random.Range (0, 10000);
		Level.yOrigin = Random.Range (0, 10000);
		Level.noiseScale = perlinFrequency;
		gradient = new Gradient ();

		Create ();
	}

	void Update(){
		
		Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, gradient.Evaluate(((Services.Player.transform.position.y - currentLevel.transform.position.y)/((float)height * (float)tileScale))), Time.deltaTime * 2);
		RenderSettings.fogColor = Camera.main.backgroundColor;

		if (currentLevel.transform.position.y - Services.Player.transform.position.y > 0) {
			currentLevel.enabled = false;
			Create ();
		}
	}

	void SetGradient(){
		GradientColorKey[] gck;
		GradientAlphaKey[] gak;

		gck = new GradientColorKey[8];
		gak = new GradientAlphaKey[2];
		gak[0].alpha = 1.0F;
		gak[0].time = 0.0F;
		gak[1].alpha = 1.0F;
		gak[1].time = 1.0F;

		for (int j = 0; j < gck.Length; j++) {
			gck [j].color = currentLevel.palette [Mathf.RoundToInt(((j)/(float)gck.Length) * (float)currentLevel.palette.Length)];
			gck [j].time = (j)/(float)gck.Length;
		}
		gradient.SetKeys(gck, gak);

		Camera.main.clearFlags = CameraClearFlags.Color;
	}

	public override Level Create(){
		NoiseRemapping = new float[Random.Range (8, 15)];
		for(int i = 0; i < NoiseRemapping.Length; i++){
			NoiseRemapping[i] = Random.Range(0.00f, 1.00f);
		}

		GameObject newLevel = new GameObject();
		Level l = newLevel.AddComponent <Level> ();
		currentLevel = l;

		newLevel.transform.position = Services.Player.transform.position;
		newLevel.name = "Level " + ManagedObjects.Count;

		if (maps.Length > 0) {
			l._bitmap = maps [Random.Range (0, maps.Length)];
		}

		l.OnCreated ();
	
		SetGradient ();
		Level.xOrigin += width / Level.noiseScale;
		Level.yOrigin += height / Level.noiseScale;

		xOffset += width;
		yOffset += height;

		ManagedObjects.Add (l);
		return l;
	}

	public override void Destroy(Level l){
		ManagedObjects.Remove (l);
		Destroy (l);
	}
}
