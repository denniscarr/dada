﻿using System.Collections;
using Enum = System.Enum;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {

	public GameObject PLAYERPREFAB;
	public GameObject TEXTOBJECT;
	public GameObject TILE;
	public GameObject PARTICLESYSTEM;
	public GameObject[][] PREFABS;
	public Sprite[][] SPRITES;
	public GameObject MONEY;
	public GameObject[] KeyAssets;

//	public 
//	public PROPS props;

	void Awake(){
		
		KeyAssets = Resources.LoadAll<GameObject> ("KeyAssets");
		MONEY = Resources.Load ("Pickups/Stack of Money") as GameObject;
		PREFABS = new GameObject[Enum.GetValues(typeof(Services.TYPES)).Length][];
		SPRITES = new Sprite[Enum.GetValues(typeof(Services.SPRITES)).Length][];
		int i = 0;

		foreach (string name in Enum.GetNames(typeof(Services.TYPES))){
			PREFABS [i] = Resources.LoadAll<GameObject> (name);
//			Debug.Log (PREFABS [i].Length + " " + name +" loaded");
			i++;
		}

		i = 0;

		foreach(string name in Enum.GetNames(typeof(Services.SPRITES))){
			SPRITES [i] = Resources.LoadAll<Sprite> ("Sprites/" + name);
			i++;
		}
	}
}
