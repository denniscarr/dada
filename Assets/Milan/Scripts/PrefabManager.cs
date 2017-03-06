using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {

	private static PrefabManager instance = null;

	public static PrefabManager Instance {
		get { 
			return instance;
		}
	}

	public GameObject PLAYER;
	public GameObject TEXT;
	public GameObject SPRITE;
	public GameObject NPC;
	public GameObject TILE;
	public GameObject[] PROCGENPOOL;


	void Awake () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

}
