using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteSpawnerScript : MonoBehaviour {
	// for the questit note 
	GameObject[] notes;
	public GameObject questItNote; 
	Transform visorNode;

	// for storing which notes go where
	public List<GameObject> id1 = new List<GameObject>();
	//public list for id2
	// etc etc

	float ranger;

	public void LateUpdate(){
		// find all notes in scene and add them to categories based on which type of note they are
		notes = GameObject.FindGameObjectsWithTag("QuestItNote");

		for (int i = 0; i < notes.Length; i++) {
			QuestItNoteFunction[] funky = notes[i].GetComponentsInChildren<QuestItNoteFunction> ();
			for (int j = 0; j < funky.Length; j++){
				if (funky [j].questID == 1) {
					id1.Add (funky [j].gameObject);
				}
			}
		}
	}

	public void MakeItRain(){ 
		// make the questit note 
		ranger = Random.Range(0, 150);
		QuestManager manager = GameObject.Find ("QuestManager").GetComponent<QuestManager> ();

		visorNode = GameObject.Find ("UpperNode").GetComponent<Transform>();  
		questItNote = Instantiate(Resources.Load("QuestItNote", typeof (GameObject))) as GameObject; 
		questItNote.transform.position = new Vector3(visorNode.transform.position.x + ranger, 
			visorNode.transform.position.y + 20 + ranger, 
			visorNode.transform.position.z + ranger + 30);


		// make the actual text appear 
		// fix this for more than one quest
		Canvas questCanvas = questItNote.GetComponentInChildren<Canvas>(); 
		Text questText = questCanvas.GetComponentInChildren<Text> ();
		questText.text = manager.GetComponent<PickupQuest> ().questItNote.GetComponentInChildren<Text> ().text;

		//questItNote.transform.parent = visorNode.transform; 
	} 

	public void AssignID(int id){
		QuestItNoteFunction funk = questItNote.GetComponentInChildren<QuestItNoteFunction> ();
		funk.questID = id;
	}

	public void DestroyNotes(int id){


	}
}
