using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteSpawnerScript : MonoBehaviour {
	// for the questit note 
	public GameObject questItNote; 
	Transform visorNode;

	float ranger;

	public void MakeItRain(){ 
		// make the questit note 
		ranger = Random.Range(10, 150);

		visorNode = GameObject.Find ("UpperNode").GetComponent<Transform>();  
		questItNote = Instantiate(Resources.Load("QuestItNote", typeof (GameObject))) as GameObject; 
		questItNote.transform.position = new Vector3(visorNode.transform.position.x + ranger, 
			visorNode.transform.position.y + 30, 
			visorNode.transform.position.z + ranger); 

		// find shit
		//QuestManager manager = GameObject.Find("QuestManager").GetComponent<QuestManager>();

		// make the actual text appear 
		// fix this
//		Canvas questCanvas = questItNote.GetComponentInChildren<Canvas>(); 
//		Text questText = questCanvas.GetComponentInChildren<Text> ();
//		questText.text = manager.currentQuestList [id].ToString();

		//questItNote.transform.parent = visorNode.transform; 
	} 

	public void AssignID(int id){
		QuestItNoteFunction funk = questItNote.GetComponentInChildren<QuestItNoteFunction> ();
		funk.questID = id;
	}
}
