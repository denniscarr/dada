using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjectManager : SimpleManager.Manager<GameObject> {
	
	protected Sprite[] spriteArray;

	void Awake(){
		spriteArray = Resources.LoadAll<Sprite> ("");
	}

	public override GameObject Create(){
		return null;
	}

	public override void Destroy(GameObject g){
		ManagedObjects.Remove (g);
		Destroy (g);
	}

	public GameObject ObjectFactory(int x){

		GameObject newObject = null;

		switch (x) {

		case 8:
			newObject = Instantiate (PrefabManager.Instance.PROCGENPOOL[0], Vector3.zero, Quaternion.identity) as GameObject;
			break;
		case 6:
			newObject = Instantiate (PrefabManager.Instance.PROCGENPOOL[1], Vector3.zero, Quaternion.identity) as GameObject;
			break;
		case 5:
			newObject = Instantiate (PrefabManager.Instance.PROCGENPOOL[2], Vector3.zero, Quaternion.identity) as GameObject;
			break;
		case 2:
			newObject = Instantiate (PrefabManager.Instance.PROCGENPOOL[3], Vector3.zero, Quaternion.identity) as GameObject;
			break;
		default:
			break;
		}
		if (newObject != null) {
			newObject.GetComponent<Renderer> ().material.mainTexture = spriteArray [Random.Range (0, spriteArray.Length)].texture;
		}

		ManagedObjects.Add (newObject);

		return newObject;
	}
}
