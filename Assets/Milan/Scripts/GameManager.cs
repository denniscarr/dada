﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {


	public void Init(){

		Services.Prefabs = gameObject.GetComponent<PrefabManager> ();
		Services.Quests = gameObject.GetComponent<QuestManager> ();
		Services.LevelGen = gameObject.GetComponent<LevelManager> ();
		Services.AudioManager = gameObject.GetComponent<CS_AudioManager> ();
        Services.IncoherenceManager = gameObject.GetComponent<IncoherenceManager>();
        //Instantiate(Services.Prefabs.PLAYERPREFAB, Vector3.zero, Quaternion.identity);
		Services.Player = GameObject.Find ("Player");
        Debug.Log(Services.Player.name);
	}
		
	void Awake () {

		Init ();
	}

	

}
