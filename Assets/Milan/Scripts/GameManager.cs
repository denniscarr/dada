using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public void Init(){

		Services.Player = GameObject.FindGameObjectWithTag ("Player");
		Services.Prefabs = gameObject.GetComponent<PrefabManager> ();
		Services.Quests = gameObject.GetComponent<QuestManager> ();
		Services.LevelGen = gameObject.GetComponent<LevelManager> ();

	}
		
	void Awake () {
		Init ();
	}

}
