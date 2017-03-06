using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SimpleManager.Manager<Level> {
	
	public float tileScale;
	private float xOffset, yOffset;

	void Start(){
		Create ();
	}

	public override Level Create(){

		GameObject Level = new GameObject();
		Level l = Level.AddComponent <Level> ();
		LevelObjectManager m = Level.AddComponent<LevelObjectManager> ();

		Level.transform.position += (Vector3.right) * xOffset/1.5f;
		Level.transform.position -= (Vector3.up) * yOffset/2;
		Level.name = "Level" + ManagedObjects.Count;

		l.minSize = 50;
		l.maxSize = 50;

		l.xOrigin = Random.Range (0, 10000);
		l.yOrigin = Random.Range (0, 10000);

		//??
//		l.xOrigin = xOffset*l.stepSize;
//		l.yOrigin = yOffset;

		l.OnCreated ();

		xOffset += l._width;
		yOffset += l._height;

		ManagedObjects.Add (l);
		return l;
	}

	public override void Destroy(Level l){
		ManagedObjects.Remove (l);
		Destroy (l);
	}
}
