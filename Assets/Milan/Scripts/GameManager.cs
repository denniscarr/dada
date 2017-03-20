using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public void Init(){

		Services.Prefabs = gameObject.GetComponent<PrefabManager> ();
		Services.Quests = gameObject.GetComponent<QuestManager> ();
		Services.LevelGen = gameObject.GetComponent<LevelManager> ();
		Services.Player = Instantiate (Services.Prefabs.PLAYERPREFAB, Vector3.zero, Quaternion.identity);
		Services.AudioManager = gameObject.GetComponent<CS_AudioManager> ();
	}
		
	void Awake () {
		Init ();
	}

}
