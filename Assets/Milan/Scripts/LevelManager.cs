using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SimpleManager.Manager<Level> {

	Level currentLevel;
	public int width, length;
	public float tileScale = 1;
	private float xOffset, yOffset;
	private Texture2D[] maps;

	void Start(){
		maps = Resources.LoadAll<Texture2D> ("maps") as Texture2D[];

		Level.xOrigin = Random.Range (0, 10000);
		Level.yOrigin = Random.Range (0, 10000);
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Space)){
			Create ();
		}
	}

	public override Level Create(){

		GameObject newLevel = new GameObject();
		Level l = newLevel.AddComponent <Level> ();

		newLevel.transform.position += (Vector3.right) * xOffset;
		newLevel.transform.position -= (Vector3.up) * yOffset;
		newLevel.name = "Level " + ManagedObjects.Count;

		if (maps.Length > 0) {
			l.bitmap = maps [Random.Range (0, maps.Length)];
		}

		l._width = width;
		l._height = length;

		l.tileScale = tileScale;


		l.OnCreated ();

		Level.xOrigin += l._width / Level.noiseScale;
//		Level.yOrigin += l._height / Level.noiseScale;
		xOffset += l._width/2;
		yOffset += l._height * 2;

		currentLevel = l;

		ManagedObjects.Add (l);
		return l;
	}

	public override void Destroy(Level l){
		ManagedObjects.Remove (l);
		Destroy (l);
	}
}
