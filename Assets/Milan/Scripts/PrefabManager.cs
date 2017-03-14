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
	public AudioClip[] Tones;

	public Sprite[] _sprites;

	void Awake(){
		_sprites = Resources.LoadAll<Sprite> ("");
		NPCPREFABS = Resources.LoadAll<GameObject> ("ObjectPrefabs");
		Tones = Resources.LoadAll<AudioClip> ("Tones");
	}

	public GameObject ObjectFactory(int x){

		GameObject newObject = null;

		switch (x) {

		case 8:
			newObject = Instantiate (Services.Prefabs.LEVELPREFABS [3], Vector3.zero, Quaternion.identity) as GameObject;
			newObject.GetComponent<AudioSource> ().clip = Tones [Random.Range (0, Tones.Length - 1)];

			break;
		case 7:
			newObject = Instantiate (Services.Prefabs.LEVELPREFABS [2], Vector3.zero, Quaternion.identity) as GameObject;

			break;
		case 6:
			newObject = Instantiate (Services.Prefabs.LEVELPREFABS[2], Vector3.zero, Quaternion.identity) as GameObject;
			newObject.GetComponent<AudioSource> ().clip = Tones [Random.Range (0, Tones.Length-1)];

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
