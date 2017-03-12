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

	public Sprite[] _sprites;

	void Awake(){
		_sprites = Resources.LoadAll<Sprite> ("");
		NPCPREFABS = Resources.LoadAll<GameObject> ("ObjectPrefabs");
	}
}
