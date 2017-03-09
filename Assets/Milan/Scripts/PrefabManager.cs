using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {

	public GameObject PLAYERPREFAB;
	public GameObject TEXTOBJECT;
	public GameObject SPRITE;
	public GameObject TILE;
	public GameObject[] LEVELPREFABS;
	public GameObject[] NPCPREFABS;

	private Sprite[] _sprites;

	void Awake(){
		_sprites = Resources.LoadAll<Sprite> ("");
		NPCPREFABS = Resources.LoadAll<GameObject> ("ObjectPrefabs");
	}

	public GameObject ObjectFactory(int x){

		GameObject newObject = null;

		switch (x) {

		case 8:
			newObject = Instantiate (Services.Prefabs.LEVELPREFABS[3], Vector3.zero, Quaternion.identity) as GameObject;
			break;
		case 7:
			newObject = Instantiate (Services.Prefabs.LEVELPREFABS[2], Vector3.zero, Quaternion.identity) as GameObject;
			break;
		case 3:
			newObject = Instantiate (Services.Prefabs.LEVELPREFABS[1], Vector3.zero, Quaternion.identity) as GameObject;
			break;
		default:
			return null;
			break;
		}

		newObject.GetComponent<Renderer> ().material.mainTexture = _sprites [Random.Range (0, _sprites.Length)].texture;

		return newObject;
	}
}
