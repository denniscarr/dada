using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SimpleManager.Manager<Level> {

	public Level currentLevel;
	public int width, length, height;
	public float tileScale = 1;

	public float[] NoiseRemapping;

	private float xOffset, yOffset;
	private Texture2D[] maps;

	void Start(){
		maps = Resources.LoadAll<Texture2D> ("maps") as Texture2D[];

		Level.xOrigin = Random.Range (0, 10000);
		Level.yOrigin = Random.Range (0, 10000);

		Create ();
	}

	public override Level Create(){
		NoiseRemapping = new float[Random.Range (8, 15)];
		for(int i = 0; i < NoiseRemapping.Length; i++){
			NoiseRemapping[i] = Random.Range(0.00f, 1.00f);
		}

		GameObject newLevel = new GameObject();
		Level l = newLevel.AddComponent <Level> ();
		currentLevel = l;

		newLevel.transform.position += (Vector3.right) * xOffset;
		newLevel.transform.position -= (Vector3.up) * yOffset;
		newLevel.name = "Level " + ManagedObjects.Count;

		if (maps.Length > 0) {
			l._bitmap = maps [Random.Range (0, maps.Length)];
		}

		l.OnCreated ();

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
