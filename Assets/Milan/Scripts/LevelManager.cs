using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SimpleManager.Manager<Level> {

	Level currentLevel;
	public float tileScale = 1;
	private float xOffset, yOffset;
	private Texture2D[] maps;

	void Start(){
		maps = Resources.LoadAll<Texture2D> ("maps") as Texture2D[];

		Level.xOrigin = Random.Range (0, 10000);
		Level.yOrigin = Random.Range (0, 10000);

		Create ();
	}

	public override Level Create(){

		GameObject newLevel = new GameObject();
		Level l = newLevel.AddComponent <Level> ();

		newLevel.transform.position += (Vector3.right) * xOffset;
		newLevel.transform.position += (Vector3.forward) * yOffset;
		newLevel.name = "Level " + ManagedObjects.Count;

		if (maps.Length > 0) {
			l.bitmap = maps [Random.Range (0, maps.Length)];
		}

		l._width = 25;
		l._height = 25;

		l.tileScale = tileScale;


		l.OnCreated ();

		Level.xOrigin += l._width / Level.noiseScale;
//		Level.yOrigin += l._height / Level.noiseScale;
		xOffset += l._width * tileScale;
//		yOffset += l._height * tileScale;

		currentLevel = l;

		ManagedObjects.Add (l);
		return l;
	}

	public override void Destroy(Level l){
		ManagedObjects.Remove (l);
		Destroy (l);
	}
}
