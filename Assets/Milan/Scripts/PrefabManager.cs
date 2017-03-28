using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {

	public GameObject PLAYERPREFAB;
	public GameObject TEXTOBJECT;
	public GameObject SPRITE;
	public GameObject TILE;
	public GameObject PARTICLESYSTEM;
	public GameObject[] STATICPREFABS;
	public GameObject[] NPCPREFABS;
    public GameObject[] FUNCTIONPREFABS;
	public AudioClip[] Tones;
	public Shader FlatShading;
	public Sprite[] _sprites;


	void Awake(){
		_sprites = Resources.LoadAll<Sprite> ("inkSprites");
		NPCPREFABS = Resources.LoadAll<GameObject> ("Object Prefabs");
        FUNCTIONPREFABS = Resources.LoadAll<GameObject> ("Functions");
		Tones = Resources.LoadAll<AudioClip> ("Tones");
	}
}
