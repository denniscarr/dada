using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {

	public GameObject PLAYERPREFAB;
	public GameObject TEXTOBJECT;
	public GameObject SPRITE;
	public GameObject TILE;
	public GameObject PARTICLESYSTEM;
	public GameObject[][] PREFABS;
	public GameObject[] STATICPREFABS;
	public GameObject[] NPCPREFABS;
    public GameObject[] FUNCTIONPREFABS;
	public AudioClip[] Tones;
	public Shader FlatShading;
	public Sprite[] _sprites;

	public enum PROPS{NPCs, Pickups, NonPickups, inkSprites, NumberOfTypes};

	void Awake(){

//		PREFABS = new GameObject[PROPS.NumberOfTypes][];
//		int i = 0;
//		foreach(PrefabManager.PROPS e in PROPS){
//			Debug.Log ((string)((PROPS)i));
//			PREFABS [i] = Resources.LoadAll<GameObject> ((string)((PROPS)i));
//			i++;
//		}
		_sprites = Resources.LoadAll<Sprite> ("inkSprites");
		NPCPREFABS = Resources.LoadAll<GameObject> ("Object Prefabs");
        FUNCTIONPREFABS = Resources.LoadAll<GameObject> ("Functions");
		Tones = Resources.LoadAll<AudioClip> ("Tones");
	}
}
