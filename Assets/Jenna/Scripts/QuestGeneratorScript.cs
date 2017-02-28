using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGeneratorScript : MonoBehaviour {

	public QuestFinderScript qfs;
	public QuestObject qo;
	public InteractionSettings iset;

	// Use this for initialization
	void Start () {

		qfs = GameObject.Find ("QuestManager").GetComponent<QuestFinderScript> ();

	}

	// Update is called once per frame
	void Update () {

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		//if (Physics.Raycast (ray, out hit) && hit.collider.gameObject.)){
		// fix this so that if it's clicked and if it's part of the system
		if (qfs.interactables.Contains (gameObject)) {
			if (iset.canBeUsedForQuests == true) {
				qo = GetComponentInParent<QuestObject> ();
				if (qo.assigned == true) {
					Debug.Log ("assigned working");
					}
				}
			}
		}	
	}
//}
